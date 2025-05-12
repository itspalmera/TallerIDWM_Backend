using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TallerIDWM_Backend.Src.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int PublicId { get; set; }

        // Relación uno a muchos con Product
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; } = null!;
    }
}