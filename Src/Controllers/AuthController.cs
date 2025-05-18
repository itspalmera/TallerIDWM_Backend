using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TallerIDWM_Backend.Src.DTOs.Auth;
using TallerIDWM_Backend.Src.DTOs.User;
using TallerIDWM_Backend.Src.Dtos;
using TallerIDWM_Backend.Src.Helpers;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;
using TallerIDWM_Backend.Src.Data;

namespace TallerIDWM_Backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(ILogger<AuthController> logger, UserManager<User> userManager, ITokenServices tokenService, UnitOfWork unitOfWork) : ControllerBase

    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenServices _tokenService = tokenService;
        private readonly UnitOfWork _unitOfWork = unitOfWork;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                var user = UserMapper.RegisterToUser(newUser);
                if (string.IsNullOrEmpty(newUser.password) || string.IsNullOrEmpty(newUser.confirmPassword))
                {
                    return BadRequest(new ApiResponse<string>(false, "La contraseña y la confirmación son requeridas"));
                }

                var createUser = await _userManager.CreateAsync(user, newUser.password);

                if (!createUser.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al crear el usuario", null, createUser.Errors.Select(e => e.Description).ToList()));
                }

                var roleUser = await _userManager.AddToRoleAsync(user, "User");
                if (!roleUser.Succeeded)
                {
                    return StatusCode(500, new ApiResponse<string>(false, "Error al asignar el rol", null, roleUser.Errors.Select(e => e.Description).ToList()));
                }

                var role = await _userManager.GetRolesAsync(user);
                var roleName = role.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthenticatedDto(user, token);

                return Ok(new ApiResponse<AuthenticatedUserDto>(true, "Usuario registrado exitosamente", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDto.email);

                
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña inválidos"));
                }

                if (!user.Active)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Tu cuenta está deshabilitada. Contacta al administrador."));
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.password);
                if (!result)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña inválidos"));
                }

                
                user.LastAccess = DateOnly.FromDateTime(DateTime.UtcNow);
                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthenticatedDto(user, token);

                return Ok(new ApiResponse<AuthenticatedUserDto>(true, "Login exitoso", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

    }
}