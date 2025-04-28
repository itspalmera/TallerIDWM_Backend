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
        public required bool IsNew { get; set; } // true = nuevo, false = usado
        public bool IsVisible { get; set; }        
        public DateTime CreatedAt { get; set; }        
        public DateTime? UpdatedAt { get; set; }

        // Relación uno a muchos con ProductImage
        public List<ProductImage> ProductImages { get; set; } = [];

        // Relación muchos a uno con Category y Brand
        //public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        //public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
    }
}