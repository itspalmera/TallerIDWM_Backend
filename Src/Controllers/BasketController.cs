using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(ILogger<BasketController> logger, UnitOfWork unitOfWork) : ControllerBase()
    {
        private readonly ILogger<BasketController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;

        // [HttpGet]
        // public async Task<ActionResult<BasketDto>> GetBasket()
        // {

        // }
        // private async Task<Basket?> RetrieveBasket()
        // {
        //     var basketId = Request.Cookies["basketId"];
            
        // }    
    }
}