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
            _logger.LogInformation("Solicitud para obtener el carrito recibida.");
            var basket = await RetrieveBasket();
            if (basket == null)
            {
                _logger.LogWarning("No se encontró ningún carrito para el usuario.");
                return NoContent();
            }
            _logger.LogInformation("Carrito obtenido correctamente.");
            return Ok(new ApiResponse<BasketDto>(
                true,
                "Carrito obtenido correctamente",
                basket.MapToDto()
            )); 
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            try 
            {
                _logger.LogInformation("Intentando agregar producto al carrito. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);
                var basket = await RetrieveBasket();

                if (basket == null)
                {
                    _logger.LogInformation("No se encontró carrito, creando uno nuevo.");
                    basket = CreateBasket();
                    await _context.SaveChangesAsync(); 
                }

                var product = await _context.ProductRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Producto no encontrado. ProductId: {ProductId}", productId);
                    return BadRequest(new ApiResponse<BasketDto>(false, "Producto no encontrado"));
                }

                if (product.Stock == 0)
                {
                    _logger.LogWarning("Producto sin stock. ProductId: {ProductId}", productId);
                    return BadRequest(new ApiResponse<BasketDto>(false, $"El producto '{product.Title}' no tiene stock disponible."));
                }

                var productInBasket = basket.Items.FirstOrDefault(i => i.ProductId == productId);
                if (productInBasket != null)
                {
                    _logger.LogWarning("Producto ya existe en el carrito. ID: {ProductId}", productId);
                    _logger.LogWarning("Stock Producto: {Product}", product.Stock);
                    _logger.LogWarning("Cantidad a agregar: {Quantity}", quantity);
                    _logger.LogWarning("Cantidad en carrito: {QuantityInBasket}", productInBasket.Quantity);
                    _logger.LogWarning("Cantidad total: {TotalQuantity}", productInBasket.Quantity + quantity);
                    if (productInBasket != null && (product.Stock < productInBasket.Quantity + quantity || product.Stock < quantity))
                    {
                        _logger.LogWarning("No hay suficiente stock para agregar la cantidad solicitada.");
                        return BadRequest(new ApiResponse<BasketDto>(false, $"Solo hay {product.Stock} unidades disponibles de '{product.Title}'"));
                    }
                    else
                    {
                        _logger.LogInformation("Producto no existe en el carrito. Se agregará. ProductId: {ProductId}", productId);
                    }
                }

                _logger.LogInformation("Agregando producto al carrito. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);
                basket.AddItem(product, quantity);

                var changes = await _context.SaveChangesAsync();
                var success = changes > 0;

                if (success)
                {
                    _logger.LogInformation("Producto añadido al carrito correctamente. ProductId: {ProductId}, BasketId: {BasketId}", productId, basket.BasketId);
                    return CreatedAtAction(nameof(GetBasket), new ApiResponse<BasketDto>(true, "Producto añadido al carrito", basket.MapToDto()));
                }
                else
                {
                    _logger.LogWarning("No se pudo actualizar el carrito tras intentar añadir el producto.");
                    return BadRequest(new ApiResponse<BasketDto>(false, "Ocurrió un problema al actualizar el carrito"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al carrito");
                return BadRequest(new ApiResponse<BasketDto>(false, "Ocurrió un error inesperado"));
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult<BasketDto>> RemoveItemFromBasket(int productId, int quantity)
        {
            try
            {
                _logger.LogInformation("Intentando eliminar producto del carrito. ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);
                var basket = await RetrieveBasket();
                if (basket == null)
                {
                    _logger.LogWarning("No se encontró el carrito al intentar eliminar producto.");
                    return NotFound("No se encontró el carrito.");
                }

                _logger.LogInformation("Carrito encontrado. BasketId: {BasketId}", basket.BasketId);
                basket.RemoveItem(productId, quantity);

                var changes = await _context.SaveChangesAsync();
                var success = changes > 0;
                if (success)
                {
                    _logger.LogInformation("Producto eliminado del carrito correctamente. ProductId: {ProductId}, BasketId: {BasketId}", productId, basket.BasketId);
                    return Ok(new ApiResponse<BasketDto>(
                        true,
                        "Producto eliminado del carrito",
                        basket.MapToDto()
                    ));
                }
                else
                {
                    _logger.LogWarning("No se pudo actualizar el carrito tras intentar eliminar el producto.");
                    return BadRequest(new ApiResponse<BasketDto>(false, "Error al actualizar el carrito"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto del carrito");
                return BadRequest(new ApiResponse<BasketDto>(false, "Ocurrió un error inesperado"));
            }
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