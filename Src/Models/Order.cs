using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIDWM_Backend.Src.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Relación con Usuario
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        // Relación con Dirección de envío (la única FK hacia Direction)
        public int DirectionId { get; set; }
        public Direction Address { get; set; } = null!; // Navigation property

        // Fecha de la orden
        public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        // Estado de la orden
        public string Status { get; set; } = "Creado";

        // Total congelado
        public decimal Total { get; set; }

        // Detalles de productos
        public List<OrderItem> Items { get; set; } = new();

    }
}