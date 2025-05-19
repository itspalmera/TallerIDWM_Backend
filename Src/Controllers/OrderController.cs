using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.Helpers;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;


namespace TallerIDWM_Backend.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(ILogger<OrderController> logger, UnitOfWork unitOfWork) : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<OrderController> _logger = logger;


        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var address = await _unitOfWork.DirectionRepository.GetByUserIdAsync(userId);
            if (address == null)
                return BadRequest(new ApiResponse<string>(false, "No tienes una dirección registrada. Por favor agrégala antes de comprar."));

            var basketId = Request.Cookies["basketId"];
            if (string.IsNullOrEmpty(basketId))
                return BadRequest(new ApiResponse<string>(false, "No se encontró el carrito"));

            var basket = await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
            if (basket == null || !basket.Items.Any())
                return BadRequest(new ApiResponse<string>(false, "El carrito está vacío"));

            var order = OrderMapper.FromBasket(basket, userId, address.Id);

            // Reducir el stock
            foreach (var item in order.Items)
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock -= item.Quantity;

                    if (product.Stock < 0)
                        return BadRequest(new ApiResponse<string>(false, $"No hay suficiente stock para el producto {product.Title}"));
                    if (product.Stock == 0)
                        product.IsVisible = false;
                }
            }

            await _unitOfWork.OrderRepository.CreateOrderAsync(order);
            _unitOfWork.BasketRepository.DeleteBasket(basket);
            await _unitOfWork.SaveChangesAsync();


            return Ok(new ApiResponse<OrderDto>(true, "Pedido realizado correctamente", OrderMapper.ToOrderDto(order)));
        }


        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderSummaryDto>>>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
            var mapped = orders.Select(OrderMapper.ToSummaryDto).ToList();

            return Ok(new ApiResponse<IEnumerable<OrderSummaryDto>>(true, "Historial de pedidos obtenido", mapped));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(id, userId);
            if (order == null)
                return NotFound(new ApiResponse<OrderDto>(false, "Pedido no encontrado"));

            return Ok(new ApiResponse<OrderDto>(true, "Pedido encontrado", OrderMapper.ToOrderDto(order)));
        }
    }
}