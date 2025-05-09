using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.DTOs.Product
{
    public class UpdateProductDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 100 caracteres.")]
        public string? Title { get; set; }

        [StringLength(250, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 250 caracteres.")]
        public string? Description { get; set; }

        [Range(100, 10000000, ErrorMessage = "El precio debe estar entre 100 y 10,000,000 en pesos chilenos.")]
        public int? Price { get; set; }

        [Range(1, 10000, ErrorMessage = "El stock debe estar entre 1 y 10000 unidades.")]
        public int? Stock { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "La categoría debe tener entre 3 y 50 caracteres.")]
        public string? Category { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "La marca debe tener entre 3 y 50 caracteres.")]
        public string? Brand { get; set; }
        public bool? IsNew { get; set; }
        public string[]? ImagesToAdd { get; set; }
        public string[]? ImagesToDelete { get; set; }
    }
}