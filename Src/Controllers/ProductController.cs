using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Extensions;
using TallerIDWM_Backend.Src.Helpers;
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
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(int id)
        {
            try 
            {
                var product = await _context.ProductRepository.GetProductByIdAsync(id);
                var response = new ApiResponse<Product>(
                    true, 
                    "Product retrieved successfully", 
                    product);
                return Ok(response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error retrieving product with ID {Id}.", id);
                return NotFound(new ApiResponse<Product>(false, "Product not found."));
            }
        }

        [HttpPost]
        // Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<Product>>> AddProduct(Product product)
        {
            try 
            {
                await _context.ProductRepository.AddProductAsync(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<Product>(
                    true, 
                    "Product added successfully", 
                    product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error adding product.");
                return BadRequest(new ApiResponse<Product>(false, "Error adding product."));
            }
        }
    }
}