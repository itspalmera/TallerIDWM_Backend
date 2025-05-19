using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.DTOs.User
{
    public class UpdateProfileDto
    {
        public string? name { get; set; } = string.Empty;
        public string? lastName { get; set; } = string.Empty;
        public string? phone { get; set; }
        public string? email { get; set; } = string.Empty;
        public DateOnly? birthDate { get; set; }
    }
}