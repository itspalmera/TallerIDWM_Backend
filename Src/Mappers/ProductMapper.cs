using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Mappers
{
    public static class ProductMapper
    {
        public static Product MapToProduct(this CreateProductDto createProductDto)
        {
            return new Product
            {
                Title = createProductDto.Title,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Category = createProductDto.Category,
                Brand = createProductDto.Brand,
                IsNew = createProductDto.IsNew,
                ProductImages = createProductDto.ImageUrl.Select(url => new ProductImage { Url = url }).ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null, // No se actualiza al crear un nuevo producto
                IsVisible = true // Valor por defecto al crear un nuevo producto
            };
        }
    }
}