using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;

namespace TallerIDWM_Backend.Src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Active == false)
            {
                return false;
            }

            user.Active = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}