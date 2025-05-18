using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.RequestHelpers
{
    public class UserParams : PaginationParams
    {
        public bool? Active { get; set; }
        public DateOnly? RegisteredFrom { get; set; }
        public DateOnly? RegisteredTo { get; set; }
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "dateDesc";
    }
}