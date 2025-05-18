using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bogus.DataSets;

using Microsoft.AspNetCore.Identity;

namespace TallerIDWM_Backend.Src.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public DateOnly LastAccess { get; set; }
        public DateOnly Registered { get; set; }
        public bool Active { get; set; }

        public string? DeactivationReason { get; set; }


        // Relación uno a uno con Direction
        public Direction Direction { get; set; } = null!;
    }
}