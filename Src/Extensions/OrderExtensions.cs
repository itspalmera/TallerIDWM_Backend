using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Extensions
{
    public static class OrderExtensions
    {
        public static IQueryable<Order> Search(this IQueryable<Order> query, int? price)
        {
            // Realizamos la búsqueda en base a todos los pedidos con un monto menor o igual a un total dado
            if (price.HasValue)
                query = query.Where(o => o.Total <= price.Value);
            return query;
        }
        public static IQueryable<Order> FilterByDate(this IQueryable<Order> query, DateOnly? from, DateOnly? to)
        {
            // Realizamos la búsqueda a partir de una fecha de inicio y una fecha de fin
            if (from.HasValue)
                query = query.Where(o => o.OrderDate >= from.Value);
            if (to.HasValue)
                query = query.Where(o => o.OrderDate <= to.Value);
            return query;
        }

        public static IQueryable<Order> Sort(this IQueryable<Order> query, string? sortBy)
        {
            // Ordenamos los pedidos por fecha de creación
            if (string.IsNullOrEmpty(sortBy) || sortBy == "dateDesc")
                return query.OrderByDescending(o => o.OrderDate);
            else if (sortBy == "dateAsc")
                return query.OrderBy(o => o.OrderDate);
            else throw new ArgumentException("Ordenamiento no válido");
        }
    }
}