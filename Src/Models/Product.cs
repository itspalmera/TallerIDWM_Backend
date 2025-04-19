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
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor positivo.")]
        public int Stock { get; set; }
        public required string Category { get; set; }
        public required string Brand { get; set; }
        public required bool IsNew { get; set; } // true = nuevo, false = usado
        public string[]? Urls { get; set; }
        public DateTime CreatedAt { get; set; }        
        public DateTime? UpdatedAt { get; set; }
        public bool IsVisible { get; set; }
    }
}