using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IDirectionRepository
    {
        Task<Direction?> GetByUserIdAsync(string userId);
        Task AddAsync(Direction address);

        void UpdateDirectionAsync(Direction address);
    }
}