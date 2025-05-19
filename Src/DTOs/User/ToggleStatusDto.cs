using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.DTOs.User
{
    public class ToggleStatusDto
    {
        [StringLength(255)]
        public string? reason { get; set; }
    }
}