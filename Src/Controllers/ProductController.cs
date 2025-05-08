using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.DTOs.Product;
using TallerIDWM_Backend.Src.Extensions;
using TallerIDWM_Backend.Src.Helpers;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;
using TallerIDWM_Backend.Src.RequestHelpers;

namespace TallerIDWM_Backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork) : ControllerBase()
    {
        private readonly ILogger<ProductController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;
        
        // Obtener todos los productos en vista de cliente 
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts([FromQuery] ProductParams productParams)
        {
            try 
            {
                // Obtener todos los productos visibles
                var query = _context.ProductRepository.GetQueryableProducts()
                                                      .Where(p => p.IsVisible == true);

                // Se aplican los filtros según los parámetros de consulta
                query = query.Search(productParams.Search)
                             .Filter(productParams.Categories, productParams.Brands)
                             .Sort(productParams.SortBy);

                // Obtener la paginación
                var pagedList = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
                
                // Establecer los encabezados de paginación
                Response.AddPaginationHeader(pagedList.Metadata);

                // Crear la respuesta
                var response = new ApiResponse<IEnumerable<Product>>(
                    true, 
                    "Productos obtenidos correctamente.", 
                    pagedList); 

                return Ok(response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error obteniendo los productos.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Obtener todos los productos en vista de administrador
        [HttpGet("admin")]
        // [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDtoAdmin>>>> GetAllProducts([FromQuery] ProductParams productParams)
        {
            try 
            {
                // Obtener todos los productos 
                var query = _context.ProductRepository.GetQueryableProducts();

                // Se aplican los filtros según los parámetros de consulta
                query = query.Search(productParams.Search)
                             .Filter(productParams.Categories, productParams.Brands)
                             .Sort(productParams.SortBy);

                // Mapear los productos a DTOs
                var mappedQuery = query.Select(p => p.MapToProductDtoAdmin());

                // Obtener la paginación
                var pagedList = await PagedList<ProductDtoAdmin>.ToPagedList(mappedQuery, productParams.PageNumber, productParams.PageSize = 20);
                
                // Establecer los encabezados de paginación
                Response.AddPaginationHeader(pagedList.Metadata);

                // Crear la respuesta
                var response = new ApiResponse<IEnumerable<ProductDtoAdmin>>(
                    true, 
                    "Productos obtenidos correctamente.", 
                    pagedList); 

                return Ok(response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error obteniendo los productos.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(string title)
        {
            try 
            {
                var product = await _context.ProductRepository.GetProductByTitleAsync(title);
                var response = new ApiResponse<Product>(
                    true, 
                    "Producto encontrado correctamente.", 
                    product);
                return Ok(response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error al obtener el producto mediante el título de este.");
                return NotFound(new ApiResponse<Product>(false, "El producto no fue encontrado."));
            }
        }

        [HttpPost]
        // Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<Product>>> AddProduct([FromBody] CreateProductDto createProductDto)
        {
            try 
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(new ApiResponse<Product>(false, "Error en los datos de entrada.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }
                var product = createProductDto.MapToProduct();

                await _context.ProductRepository.AddProductAsync(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<Product>(
                    true, 
                    "Producto agregado correctamente.", 
                    product);

                return CreatedAtAction(nameof(GetProduct), new {title = product.Title}, response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error al agregar el producto.");
                return BadRequest(new ApiResponse<Product>(false, "Error al agregar el producto.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> RemoveProduct(string title)
        {
            try 
            {
                var product = await _context.ProductRepository.GetProductByTitleAsync(title);
                if (product == null) 
                {
                    return NotFound(new ApiResponse<Product>(false, "El producto no fue encontrado."));
                }
                // Lógica para verificar si el producto tiene órdenes asociadas
                // if (product.Orders.Count > 0) 
                // {
                //     // Cambiar el estado del producto a no visible
                //     product.IsVisible = false;
                //     return Ok(new ApiResponse<ProductDtoAdmin>(
                //         true, 
                //         "Producto removido correctamente, pero tiene órdenes asociadas. Se ha cambiado su estado a no visible.", 
                //         product.MapToProductDtoAdmin()));
                // }

                // Si no tiene órdenes asociadas, eliminar el producto
                _context.ProductRepository.DeleteProductAsync(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<Product>(
                    true, 
                    "Producto eliminado correctamente.", 
                    product);

                return Ok(response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error al eliminar el producto.");
                return BadRequest(new ApiResponse<Product>(false, "Error al eliminar el producto.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }
        }
    }
}