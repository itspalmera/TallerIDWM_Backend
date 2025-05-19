namespace TallerIDWM_Backend.Src.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string? SortBy { get; set; }
        public string? Search { get; set; }
        public string? Categories { get; set; }
        public string? Brands { get; set; }
        public bool? IsNew { get; set; } // Para filtrar por nuevo o usado   
    }
}