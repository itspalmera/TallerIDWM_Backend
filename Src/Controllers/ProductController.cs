using Microsoft.AspNetCore.Mvc;

using TallerIDWM_Backend.Src.Interfaces;

namespace TallerIDWM_Backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductRepository productRepository) : ControllerBase()
    {
        private readonly IProductRepository _productRepository = productRepository;
        
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }
    }
}