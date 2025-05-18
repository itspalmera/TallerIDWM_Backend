using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.DTOs.Direction
{
    public class UpdateDirectionDto
    {
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string? street { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "El número debe ser un valor numérico")]
        public string? number { get; set; }


        [StringLength(100, ErrorMessage = "La comuna no puede exceder los 100 caracteres")]
        public string? commune { get; set; }


        [StringLength(100, ErrorMessage = "La región no puede exceder los 100 caracteres")]

        public string? region { get; set; }


        [RegularExpression(@"^\d{7}$", ErrorMessage = "El código postal debe tener 7 dígitos")]
        public string? postalCode { get; set; }
        
    }
}