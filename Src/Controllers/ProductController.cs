using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.DTOs;
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
        
        // Get all products with pagination and filtering for 
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts([FromQuery] ProductParams productParams)
        {
            try 
            {
                var query = _context.ProductRepository.GetQueryableProducts()
                                                      .Where(p => p.IsVisible == true);

                // Apply filtering based on the query parameters
                query = query.Search(productParams.Search)
                             .Filter(productParams.Categories, productParams.Brands)
                             .Sort(productParams.SortBy);

                // Get pagination 
                var pagedList = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
                
                // Set the pagination headers
                Response.AddPaginationHeader(pagedList.Metadata);

                // 
                var response = new ApiResponse<IEnumerable<Product>>(
                    true, 
                    "Products retrieved successfully", 
                    pagedList); 

                return Ok(response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error retrieving products.");
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
    }
}