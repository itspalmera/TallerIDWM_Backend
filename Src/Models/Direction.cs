using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Models
{
    public class Direction
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Commune { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        // Relación uno a uno con User
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}