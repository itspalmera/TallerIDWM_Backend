using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bogus;

using Microsoft.AspNetCore.Identity;

using TallerIDWM_Backend.Src.DTOs.Auth;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;



namespace TallerIDWM_Backend.Src.Data.Seeders
{
    public class UserSeeder
    {
        public static List<RegisterDto> GenerateUserDtos(int quantity = 10)
        {
            var users = new Faker<RegisterDto>()
                .RuleFor(u => u.name, f => f.Person.FirstName)
                .RuleFor(u => u.email, f => f.Internet.Email())
                .RuleFor(u => u.password, f => "User" + f.Random.Number(1000, 9999).ToString())
                .RuleFor(u => u.lastName, f => f.Person.LastName)
                .RuleFor(u => u.phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.confirmPassword, (f, u) => u.password)
                .RuleFor(u => u.street, f => f.Address.StreetName())
                .RuleFor(u => u.number, f => f.Address.BuildingNumber())
                .RuleFor(u => u.commune, f => f.Address.City())
                .RuleFor(u => u.region, f => f.Address.State())
                .RuleFor(u => u.postalCode, f => f.Address.ZipCode())
                .Generate(quantity);

            return users;
        }


        public static async Task CreateUsers(UserManager<User> userManager, List<RegisterDto> userDtos)
        {
            var admin = new User
            {
                UserName = "ignacio.mancilla@gmail.com",
                Email = "ignacio.mancilla@gmail.com",
                FirstName = "Ignacio",
                LastName = "Mancilla",
                PhoneNumber = "999999999",
                Registered = DateOnly.FromDateTime(DateTime.UtcNow),
                Active = true,
                Direction = new Direction
                {
                    Street = "Central",
                    Number = "1000",
                    Commune = "Santiago",
                    Region = "RM",
                    PostalCode = "0000000"
                }
            };

            var userExample = new User
            {
                UserName = "samuel.fuentes@gmail.com",
                Email = "samuel.fuentes@gmail.com",
                FirstName = "Samuel",
                LastName = "Fuentes",
                PhoneNumber = "111111111",
                Registered = DateOnly.FromDateTime(DateTime.UtcNow),
                Active = true,
                Direction = new Direction
                {
                    Street = "",
                    Number = "",
                    Commune = "",
                    Region = "",
                    PostalCode = ""
                }
            };

            var existingAdmin = await userManager.FindByEmailAsync(admin.Email);
            if (existingAdmin == null)
            {
                var result = await userManager.CreateAsync(admin, "Pa$$word2025");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    throw new Exception($"Error creating required admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            var existingUSer = await userManager.FindByEmailAsync(userExample.Email);
            if (existingUSer == null)
            {
                var result = await userManager.CreateAsync(userExample, "Pa$$word2025");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userExample, "User");
                }
                else
                {
                    throw new Exception($"Error creating required : {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }


            foreach (var userDto in userDtos)
            {
                var user = UserMapper.RegisterToUser(userDto);

                var result = await userManager.CreateAsync(user, userDto.password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    throw new Exception($"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}