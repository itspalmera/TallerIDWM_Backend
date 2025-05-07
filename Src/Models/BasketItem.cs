using System.ComponentModel.DataAnnotations.Schema;

namespace TallerIDWM_Backend.Src.Models
{
    [Table("BasketItems")]
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        // Relación con Product
        public int ProductId { get; set; }
        public required Product Product { get; set; } 
        // Relación con Basket
        public int BasketId { get; set; } 
        public Basket Basket { get; set; } = null!;
    }
}