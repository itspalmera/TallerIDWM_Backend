using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
         public string Phone { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}