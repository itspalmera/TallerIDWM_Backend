using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Dtos
{
    public class UserDto
    {
        public string name { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;

        public string? street { get; set; }
        public string? number { get; set; }
        public string? commune { get; set; }
        public string? region { get; set; }
        public string? postalCode { get; set; }

        public DateOnly registered { get; set; }
        public DateOnly? lastAccess { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool active { get; set; }

    }
}