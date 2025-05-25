using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Dtos;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.DTOs.User;
using TallerIDWM_Backend.Src.Extensions;
using TallerIDWM_Backend.Src.Helpers;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;
using TallerIDWM_Backend.Src.RequestHelpers;

namespace TallerIDWM_Backend.Src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;



        //TODO: GET ALL WITH PAGINATION AND FILTERING
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll([FromQuery] UserParams userParams)
        {
            var query = _unitOfWork.UserRepository.GetUsersQueryable()
                .Filter(userParams.Active, userParams.RegisteredFrom, userParams.RegisteredTo)
                .Search(userParams.SearchTerm)
                .Sort(userParams.OrderBy);

            var total = await query.CountAsync();

            var users = await query
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var dtos = users.Select(UserMapper.UserToUserDto).ToList();

            Response.AddPaginationHeader(new PaginationMetaData
            {
                CurrentPage = userParams.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)userParams.PageSize),
                PageSize = userParams.PageSize,
                TotalCount = total
            });

            return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Usuarios obtenidos correctamente", dtos));
        }


        //TODO: GET BY EMAIL OR NAME
        [Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById([FromBody] UserSearchDto search)
        {
            if (string.IsNullOrEmpty(search.email) && string.IsNullOrEmpty(search.name))
            {
                return BadRequest(new ApiResponse<string>(false, "Se requiere un email o nombre para buscar el usuario"));
            }

            if (!string.IsNullOrEmpty(search.name))
            {
                var user = await _unitOfWork.UserRepository.GetUserByNameAsync(search.name);
                if (user == null)
                    return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

                var dto = UserMapper.UserToUserDto(user);
                return Ok(new ApiResponse<UserDto>(true, "Usuario encontrado", dto));
            }

            else
            {
                var userByEmail = await _unitOfWork.UserRepository.GetUserByEmailAsync(search.email);
                if (userByEmail == null)
                    return NotFound(new ApiResponse<string>(false, "Usuario no encontrado (email)"));

                var dtoByEmail = UserMapper.UserToUserDto(userByEmail);
                return Ok(new ApiResponse<UserDto>(true, "Usuario encontrado", dtoByEmail));

            }


        }


        //TODO: UPDATE USER PROFILE (DELETE)
        [Authorize(Roles = "Admin")]
        // PUT /users/{id}/status
        [HttpPatch("{email}/status")]
        public async Task<ActionResult<ApiResponse<string>>> ToggleStatus(string email, [FromBody] ToggleStatusDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            var roles = await _unitOfWork.UserRepository.GetUserRolesAsync(user);
            if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new ApiResponse<string>(
                    false,
                    "No se puede deshabilitar una cuenta con rol de administrador."
                ));
            }

            user.Active = !user.Active;
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var message = user.Active ? "Usuario habilitado correctamente" : "Usuario deshabilitado correctamente";
            return Ok(new ApiResponse<string>(true, message));
        }

        //TODO: CREATE DIRECCTION
        [Authorize(Roles = "User")]
        [HttpPost("address")]
        public async Task<ActionResult<ApiResponse<Direction>>> CreateDirection([FromBody] CreateDirectionDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));
            var existing = await _unitOfWork.DirectionRepository.GetByUserIdAsync(userId);
            var hasExistingData = existing != null && !string.IsNullOrWhiteSpace(existing.Street) &&
                !string.IsNullOrWhiteSpace(existing.Number) &&
                !string.IsNullOrWhiteSpace(existing.Commune) &&
                !string.IsNullOrWhiteSpace(existing.Region) &&
                !string.IsNullOrWhiteSpace(existing.PostalCode);

            if (hasExistingData)
                return BadRequest(new ApiResponse<string>(false, "Ya tienes una dirección registrada válida"));

            var address = DirectionMapper.FromDto(dto, userId);

            await _unitOfWork.DirectionRepository.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();
            var addressDto = DirectionMapper.ToDto(address);
            return Ok(new ApiResponse<DirectionDto>(true, "Dirección creada exitosamente", addressDto));
        }



        //TODO: UPDATE USER
        [Authorize(Roles = "User")]
        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            UserMapper.UpdateUserFromDto(user, dto);

            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<UserDto>(true, "Perfil actualizado correctamente", UserMapper.UserToUserDto(user)));
        }


        //TODO: UPDATE PASSWORD
        [Authorize(Roles = "User")]
        [HttpPatch("profile/password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest(new ApiResponse<string>(false, "La nueva contraseña y la confirmación no coinciden"));
            if (dto.NewPassword == dto.CurrentPassword) return BadRequest(new ApiResponse<string>(false, "La nueva contraseña no puede ser igual a la actual"));

            var result = await _unitOfWork.UserRepository.UpdatePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse<string>(
                    false,
                    "Error al cambiar la contraseña",
                    null,
                    result.Errors.Select(e => e.Description).ToList()
                ));
            }
            return Ok(new ApiResponse<string>(true, "Contraseña actualizada correctamente"));
        }


        //TODO: GET USER PROFILE
        [Authorize(Roles = "User")]
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            var dto = UserMapper.UserToUserDto(user);
            return Ok(new ApiResponse<UserDto>(true, "Perfil del usuario obtenido", dto));
        }



        //TODO: UPDATE DIRECTION
        [Authorize(Roles = "User")]
        [HttpPatch("address")]
        public async Task<ActionResult<ApiResponse<Direction>>> UpdateDirection([FromBody] UpdateDirectionDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var address = await _unitOfWork.DirectionRepository.GetByUserIdAsync(userId);
            if (address == null)
                return NotFound(new ApiResponse<string>(false, "No tienes una dirección registrada. Usa el método POST para crear una."));

            DirectionMapper.UpdateDirectionFromDto(address, dto);

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.DirectionRepository.UpdateDirectionAsync(address);
            return Ok(new ApiResponse<Direction>(true, "Dirección actualizada correctamente", address));
        }

    }


}