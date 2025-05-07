using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;

namespace TallerIDWM_Backend.Src.Models
{
    public class User: IdentityUser
    {
        public DateOnly BirthDate { get; set; }
        public DateOnly LastAccess { get; set; }
        public DateOnly Created { get; set; }
        public bool Active { get; set; } 

        public string? DeactivationReason { get; set; }
 

        // Relación uno a uno con Direction
        public Direction Direction { get; set; } = null!;
    }
}