using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIDWM_Backend.Src.Dtos;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IUserRepository
    {
        //Task<User> AddUserAsync(UserCreateDto userCreateDto);

        //falta rango de fechas
        //Task<List<UserGetDto>> GetAllUserAsync(bool? Active, string? name, string? email);
        //Task<User?> GetUserByIdAsync(int id);
        //Task UpdateUserAsync(User user);
        //Task UpdateUserActive();
        Task<bool> DeleteUser(int id);
    }
}