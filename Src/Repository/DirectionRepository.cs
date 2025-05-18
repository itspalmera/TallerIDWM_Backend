using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Models;


namespace TallerIDWM_Backend.Src.Repository
{
    public class DirectionRepository(DataContext context) : IDirectionRepository
    {
        private readonly DataContext _context = context;

        public async Task<Direction?> GetByUserIdAsync(string userId)
        {
            return await _context.Directions
                 .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task AddAsync(Direction address)
        {
            await _context.Directions.AddAsync(address);
        }

        public void UpdateDirectionAsync(Direction address)
        {
            _context.Directions.Update(address);
        }

    }
}