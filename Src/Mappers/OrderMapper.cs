using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.Models;


namespace TallerIDWM_Backend.Src.Mappers
{
    public class OrderMapper
    {

        public static Order FromBasket(Basket basket, string userId, int directionId)
        {
            return new Order
            {
                UserId = userId,
                ShippingAddressId = directionId,
                Total = basket.Items.Sum(i => i.Quantity * i.Product.Price),
                Items = basket.Items.Select(i => new OrderItem
                {
                    ProductId = i.Product.Id,
                    ProductName = i.Product.Title,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };
        }

        public static OrderDto ToOrderDto(Order order)
        {
            return new OrderDto
            {
                id = order.Id,
                createdAt = order.OrderDate,
                address = order.ShippingAddress,
                total = (int)Math.Floor(order.Total),
                items = order.Items.Select(i => new OrderItemDto
                {
                    productId = i.ProductId,
                    name = i.ProductName,
                    quantity = i.Quantity,
                    price = (int)Math.Floor(i.Price),
                    imageUrl = "" // Puedes ajustar si decides guardar o mapear imágenes
                }).ToList()
            };
        }

        public static OrderSummaryDto ToSummaryDto(Order order)
        {
            return new OrderSummaryDto
            {
                Id = order.Id,
                CreatedAt = order.OrderDate,
                Total = (int)Math.Floor(order.Total)
            };
        }
    }
}
