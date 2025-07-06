namespace TallerIDWM_Backend.Src.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string? SortBy { get; set; }
        public string? Search { get; set; }
        public string? Categories { get; set; }
        public string? Brands { get; set; }
        public string? Conditions { get; set; } // Para filtrar por condición del producto (nuevo, usado, etc.)
        public bool? IsNew { get; set; } // Para filtrar por nuevo o usado
        public int? MinPrice { get; set; }   
        public int? MaxPrice { get; set; }
    }
}