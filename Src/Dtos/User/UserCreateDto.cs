using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; } = string.Empty;

        [Required]
        [Phone(ErrorMessage = "Número de teléfono no válido")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos")]
        public string phone { get; set; } = string.Empty;
        public DateOnly birthDate;

        [Required]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos 6 caracteres", MinimumLength = 6)]
        public string password { get; set; } = string.Empty;


        //public string confirmPassword; ????


    }
}