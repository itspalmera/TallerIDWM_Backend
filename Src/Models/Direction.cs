using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Models
{
    public class Direction
    {
        public string street { get; set; } = string.Empty;
        public string number { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        public string zipCode { get; set; } = string.Empty;
    }
}