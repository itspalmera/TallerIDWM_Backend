using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Dtos
{
    public class UserGetDto
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string dataRegister { get; set; } = string.Empty;
        public string active { get; set; } = string.Empty;
        //fecha del último acceso.

    }
}