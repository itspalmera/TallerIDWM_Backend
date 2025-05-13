using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.DTOs.Basket;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Mappers
{
    public static class BasketMapper
    {
        public static BasketDto MapToDto(this Basket basket)
        {
            return new BasketDto
            {
                BasketId = basket.BasketId,
                Items = [.. basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Title,
                    ImageUrl = item.Product.ProductImages.FirstOrDefault()?.Url ?? string.Empty,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.Quantity * item.Product.Price
                })]
            };
        }
    }
}