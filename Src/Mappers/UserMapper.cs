using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Dtos;
using TallerIDWM_Backend.Src.DTOs.Auth;
using TallerIDWM_Backend.Src.DTOs.User;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Mappers
{
    public class UserMapper
    {
        public static User RegisterToUser(RegisterDto dto) =>
            new()
            {
                UserName = dto.name,
                Email = dto.email,
                FirstName = dto.name,
                LastName = dto.lastName,
                PhoneNumber = dto.phone,
                Registered = DateOnly.FromDateTime(DateTime.UtcNow),
                Active = true,
                Direction = new Direction
                {
                    Street = dto.street ?? string.Empty,
                    Number = dto.number ?? string.Empty,
                    Commune = dto.commune ?? string.Empty,
                    Region = dto.region ?? string.Empty,
                    PostalCode = dto.postalCode ?? string.Empty
                }
            };


        public static UserDto UserToUserDto(User user) =>
            new()
            {
                name = user.FirstName,
                lastName = user.LastName,
                email = user.Email ?? string.Empty,
                phone = user.PhoneNumber ?? string.Empty,
                street = user.Direction?.Street,
                number = user.Direction?.Number,
                commune = user.Direction?.Commune,
                region = user.Direction?.Region,
                postalCode = user.Direction?.PostalCode,
                registered = user.Registered,
                lastAccess = user.LastAccess,
                active = user.Active
            };


        public static AuthenticatedUserDto UserToAuthenticatedDto(User user, string token) =>
            new()
            {
                name = user.FirstName,
                lastName = user.LastName,
                email = user.Email ?? string.Empty,
                phone = user.PhoneNumber ?? string.Empty,
                token = token,
                street = user.Direction.Street,
                number = user.Direction.Number,
                commune = user.Direction.Commune,
                region = user.Direction.Region,
                postalCode = user.Direction.PostalCode,
                registered = user.Registered,
                lastAccess = user.LastAccess,
                active = user.Active
            };


        public static void UpdateUserFromDto(User user, UpdateProfileDto dto)
        {
            if (dto.name is not null)
                user.FirstName = dto.name;

            if (dto.lastName is not null)
                user.LastName = dto.lastName;

            if (dto.email is not null)
                user.Email = dto.email;

            if (dto.phone is not null)
                user.PhoneNumber = dto.phone;

            if (dto.birthDate.HasValue)
                user.BirthDate = dto.birthDate.Value;
        }
    }
}