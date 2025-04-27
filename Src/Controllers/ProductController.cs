using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.Data;

namespace TallerIDWM_Backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(UnitOfWork unitOfWork) : ControllerBase()
    {
        private readonly UnitOfWork _context = unitOfWork;
        
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.ProductRepository.GetAllProductsAsync();
            return Ok(products);
        }
    }
}