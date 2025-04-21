using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TallerIDWM_Backend.Src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userRepository.DeleteUser(id);
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