using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TallerIDWM_Backend.Src.Repository
{
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        
        public IQueryable<User> GetUsersQueryable()
        {
            return _userManager.Users.Include(u => u.Direction).AsQueryable();
        }



        public async Task<User?> GetUserByIdAsync(string id){
            return await _userManager.Users
                .Include(u => u.Direction)
                .FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User?> GetUserByEmailAsync(string email){
            return await _userManager.Users
                .Include(u => u.Direction)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User?> GetUserByNameAsync(string username){
            return await _userManager.Users
                .Include(u => u.Direction)
                .FirstOrDefaultAsync(u => u.FirstName == username);
        }
        

        public async Task<bool> CheckPasswordAsync(User user, string password){
             return await Task.Run(() =>
            {
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash!, password);
                return result == PasswordVerificationResult.Success;
            });
        }


        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
    

        public async Task<IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public Task<User?> GetUserWithAddressByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }


        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

    }

}