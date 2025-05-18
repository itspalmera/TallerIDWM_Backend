using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.DTOs
{
    public class CreateDirectionDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public required string street { get; set; }


        [Required(ErrorMessage = "El número es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número debe ser un valor numérico")]
        public required string number { get; set; }
        
        
        [Required(ErrorMessage = "La comuna es requerida")]
        [StringLength(100, ErrorMessage = "La comuna no puede exceder los 100 caracteres")]
        public required string commune { get; set; }


        [Required(ErrorMessage = "La región es requerida")]
        [StringLength(100, ErrorMessage = "La región no puede exceder los 100 caracteres")]
        public required string region { get; set; }


        [Required(ErrorMessage = "El código postal es requerido")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "El código postal debe tener 7 dígitos")]
        public required string postalCode { get; set; }
    }
}