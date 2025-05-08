using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.DTOs.Product
{
    public class ProductDtoAdmin
    {
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public bool IsNew { get; set; } // true = nuevo, false = usado       
        public DateTime CreatedAt { get; set; }        
        public DateTime? UpdatedAt { get; set; }
    }
}