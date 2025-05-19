using TallerIDWM_Backend.Src.DTOs.Basket;

namespace TallerIDWM_Backend.Src.DTOs
{
    public class BasketDto
    {
        public required string BasketId { get; set; } = null!;
        public List<BasketItemDto> Items { get; set; } = [];
    }
}