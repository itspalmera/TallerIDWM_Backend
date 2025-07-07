using System.ComponentModel.DataAnnotations;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.DTOs.Product
{
    public class ProductDtoAdmin
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public DateTime LastModification { get; set; }
    }
}