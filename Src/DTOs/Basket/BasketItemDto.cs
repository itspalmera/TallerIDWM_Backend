namespace TallerIDWM_Backend.Src.DTOs.Basket
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string[] ImageUrl { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}