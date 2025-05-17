using System.ComponentModel.DataAnnotations;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "El título es requerido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 100 caracteres.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 250 caracteres.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "El precio es requerido.")]
        [Range(100, 10000000, ErrorMessage = "El precio debe estar entre 100 y 10,000,000 en pesos chilenos.")]
        public required int Price { get; set; }

        [Required(ErrorMessage = "El stock es requerido.")]
        [Range(1, 10000, ErrorMessage = "El stock debe estar entre 1 y 10000 unidades.")]
        public required int Stock { get; set; }

        [Required(ErrorMessage = "La categoría es requerida.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La categoría debe tener entre 3 y 50 caracteres.")]
        public required string Category { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La marca debe tener entre 3 y 50 caracteres.")]
        public required string Brand { get; set; }

        [Required(ErrorMessage = "El estado del producto es requerido.")]
        public required ProductCondition Condition { get; set; } = ProductCondition.Nuevo;

        [Required]
        [MinLength(1, ErrorMessage = "Se requiere al menos una imagen.")]
        public List<IFormFile> Images { get; set; } = [];
    }
}