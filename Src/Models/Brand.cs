using System.ComponentModel.DataAnnotations;

namespace TallerIDWM_Backend.Src.Models
{
    /// <summary>
    /// Clase que representa una categoría de productos.
    /// </summary>
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}