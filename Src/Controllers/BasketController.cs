using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public class BasketController(ILogger<BasketController> logger, UnitOfWork unitOfWork) : ControllerBase()
    {
        private readonly ILogger<BasketController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();
            if (basket == null) { return NoContent(); }
            return Ok(new ApiResponse<BasketDto>(
                true,
                "Carrito obtenido correctamente",
                basket.MapToDto()
            ));
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            _logger.LogWarning("Entrando a AddItemToBasket con productId: {ProductId}, quantity: {Quantity}", productId, quantity);
            var basket = await RetrieveBasket();

            if (basket == null)
            {
                basket = CreateBasket();
                await _context.SaveChangesAsync(); 
            }

            var product = await _context.ProductRepository.GetProductByIdAsync(productId);
            if (product == null)
                return BadRequest(new ApiResponse<string>(false, "Producto no encontrado"));

            if (product.Stock == 0)
                return BadRequest(new ApiResponse<string>(false, $"El producto '{product.Title}' no tiene stock disponible."));

            _logger.LogWarning("Stock Producto: {Product}", product.Stock);
            _logger.LogWarning("Cantidad a agregar: {Quantity}", quantity);
            _logger.LogWarning("Cantidad en carrito: {QuantityInBasket}", basket.Items.Select(i => i.Quantity).Sum());
            if (product.Stock < basket.Items.Select(i => i.Quantity).Sum() + quantity || product.Stock < quantity)
                return BadRequest(new ApiResponse<string>(false, $"Solo hay {product.Stock} unidades disponibles de '{product.Title}'"));

            basket.AddItem(product, quantity);

            var changes = await _context.SaveChangesAsync();
            var success = changes > 0;

            return success
                ? CreatedAtAction(nameof(GetBasket), new ApiResponse<BasketDto>(true, "Producto añadido al carrito", basket.MapToDto()))
                : BadRequest(new ApiResponse<string>(false, "Ocurrió un problema al actualizar el carrito"));
        }

        [HttpDelete]
        public async Task<ActionResult<BasketDto>> RemoveItemFromBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) return NotFound("No se encontró el carrito.");

            basket.RemoveItem(productId, quantity);

            var changes = await _context.SaveChangesAsync();
            var success = changes > 0;
            return success
                ? Ok(new ApiResponse<BasketDto>(
                    true,
                    "Producto eliminado del carrito",
                    basket.MapToDto()
                ))
                : BadRequest(new ApiResponse<string>(false, "Error al actualizar el carrito"));
        }

        private async Task<Basket?> RetrieveBasket()
        {
            var basketId = Request.Cookies["basketId"];
            _logger.LogInformation("BasketId recibido desde cookie: {BasketId}", basketId);
            return string.IsNullOrEmpty(basketId) ? null : await _context.BasketRepository.GetBasketAsync(basketId);
        }    

        private Basket CreateBasket()
        {
            var basketId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(14),

            };
            Response.Cookies.Append("basketId", basketId, cookieOptions);
            _logger.LogWarning("Nuevo basket creado con ID: {BasketId}", basketId);
            return _context.BasketRepository.CreateBasket(basketId);
        }
    }
}