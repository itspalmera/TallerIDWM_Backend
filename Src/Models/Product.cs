using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.Models
{
    /// <summary>
    /// Clase que representa un producto.
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor positivo.")]
        public int Stock { get; set; }
        public required string Category { get; set; } = string.Empty;
        public required string Brand { get; set; } = string.Empty;
        public required ProductCondition ProductCondition { get; set; } 
        public bool IsVisible { get; set; }        
        public DateTime CreatedAt { get; set; }        
        public DateTime? UpdatedAt { get; set; }

        // Relación uno a muchos con ProductImage
        public List<ProductImage> ProductImages { get; set; } = [];
    }
}