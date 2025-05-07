using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.DTOs
{
    public class CreateProductDto
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required int Price { get; set; }
        [Required]
        public required int Stock { get; set; }
        [Required]
        public required string Category { get; set; }
        [Required]
        public required string Brand { get; set; }
        [Required]
        public required bool IsNew { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Debe proporcionar al menos una URL de imagen.")]
        public required string[] ImageUrl { get; set; }
    }
}