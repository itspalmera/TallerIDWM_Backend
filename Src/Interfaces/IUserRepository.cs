using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using TallerIDWM_Backend.Src.Dtos;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IUserRepository
    {

        IQueryable<User> GetUsersQueryable();
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByNameAsync(string username);
        Task UpdateUserAsync(User user); // Save status change or profile update
        Task<bool> CheckPasswordAsync(User user, string password);
        //Task<User> CreateUserAsync(UserCreateDto userCreateDto);

        Task<User?> GetUserWithAddressByIdAsync(string userId);

        Task <IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword);

        Task<IList<string>> GetUserRolesAsync(User user);

    }
}