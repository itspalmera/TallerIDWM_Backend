using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.DTOs
{
    public class OrderDto
    {
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public Direction address { get; set; } = null!;
        public int total { get; set; } // En CLP, sin decimales
        public List<OrderItemDto> items { get; set; } = [];
    }
}