using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public required string email { get; set; }
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        public required string password { get; set; }
    }
}