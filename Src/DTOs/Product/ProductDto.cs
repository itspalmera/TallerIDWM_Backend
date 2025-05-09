using System.ComponentModel.DataAnnotations;
namespace TallerIDWM_Backend.Src.DTOs
{
    public class ProductDto
    {
        [Required]
        public required string Title { get; set; } = string.Empty;
        [Required]
        public required int Price { get; set; }
        [Required]
        public required string ImageUrl { get; set; }   
    }
}