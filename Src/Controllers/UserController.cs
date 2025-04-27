using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TallerIDWM_Backend.Src.Data;

namespace TallerIDWM_Backend.Src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UnitOfWork unitOfWork) : ControllerBase()
    {
        private readonly UnitOfWork _context = unitOfWork;
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _context.UserRepository.DeleteUser(id);
                if (result)
                {
                    return Ok("Usuario desactivado con éxito");
                }
                return BadRequest("Usuario no encontrado o ya desactivado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        

    }
}