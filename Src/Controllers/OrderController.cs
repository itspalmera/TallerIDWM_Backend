using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.Extensions;
using TallerIDWM_Backend.Src.Helpers;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;
using TallerIDWM_Backend.Src.Repository;
using TallerIDWM_Backend.Src.RequestHelpers;


namespace TallerIDWM_Backend.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(ILogger<OrderController> logger, UnitOfWork unitOfWork) : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<OrderController> _logger = logger;

        //TODO: CREATE ORDER
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var address = await _unitOfWork.DirectionRepository.GetByUserIdAsync(userId);
            _logger.LogInformation($"Dirección obtenida: {address}");
            _logger.LogInformation("Shippings addres id {addressId}", address?.Id);
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
            foreach (var item in basket.Items)
            {
                if (item.Product == null)
                    return BadRequest(new ApiResponse<string>(false, $"Producto no disponible en el carrito"));

                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(item.Product.Id);

                if (product == null)
                    return BadRequest(new ApiResponse<string>(false, $"El producto '{item.Product.Title}' ya no existe"));

                if (product.Stock < item.Quantity)
                    return BadRequest(new ApiResponse<string>(false, $"No hay suficiente stock para el producto '{product.Title}'"));
            }

            await _unitOfWork.OrderRepository.CreateOrderAsync(order);
            _unitOfWork.BasketRepository.DeleteBasket(basket);
            await _unitOfWork.SaveChangesAsync();


            return Ok(new ApiResponse<OrderDto>(true, "Pedido realizado correctamente", OrderMapper.ToOrderDto(order)));
        }

        //TODO: GET ORDERS
        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<OrderDto>>> GetMyOrders([FromQuery] OrderParams orderParams)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var query = _unitOfWork.OrderRepository.GetQueryableOrdersByUserId(userId);

            query = query
                .Search(orderParams.Price)
                .FilterByDate(orderParams.RegisteredFrom, orderParams.RegisteredTo)
                .Sort(orderParams.OrderBy);

            // Aquí mapea a OrderDto
            var orders = query.ToList();
            var mapped = orders.Select(OrderMapper.ToOrderDto).ToList();

            var response = new ApiResponse<IEnumerable<OrderDto>>(
                true,
                "Historial de pedidos obtenido",
                mapped
            );

            return Ok(response);
        }

        //TODO: GET ORDER BY ID
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
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