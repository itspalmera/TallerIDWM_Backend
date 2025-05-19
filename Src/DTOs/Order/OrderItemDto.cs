using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.DTOs
{
    public class OrderItemDto
    {
        public int productId { get; set; }
        public string name { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public int quantity { get; set; }
        public int price { get; set; }
    }
}