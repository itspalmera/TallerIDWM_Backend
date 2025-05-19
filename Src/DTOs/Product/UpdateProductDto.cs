using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.DTOs.Product
{
    public class UpdateProductDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 100 caracteres.")]
        public string? Title { get; set; }

        [StringLength(250, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 250 caracteres.")]
        public string? Description { get; set; }

        [Range(100, 100000000, ErrorMessage = "El precio debe estar entre 100 y 100,000,000 en pesos chilenos.")]
        public int? Price { get; set; }

        [Range(1, 100000, ErrorMessage = "El stock debe estar entre 1 y 100000 unidades.")]
        public int? Stock { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "La categoría debe tener entre 3 y 50 caracteres.")]
        public string? Category { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "La marca debe tener entre 3 y 50 caracteres.")]
        public string? Brand { get; set; }
        [EnumDataType(typeof(ProductCondition), ErrorMessage = "El estado del producto no es válido.")]
        public ProductCondition? Condition { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}