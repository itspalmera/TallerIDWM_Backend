using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.RequestHelpers
{
    public class OrderParams
    {
        public int? Price { get; set; }
        public DateOnly? RegisteredFrom { get; set; }
        public DateOnly? RegisteredTo { get; set; }
        public string? OrderBy { get; set; } = "dateDesc";
    }
}