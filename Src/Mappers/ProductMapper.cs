using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.DTOs.Product;
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
        
        public static ProductDto MapToProductDto(this Product product)
        {
            return new ProductDto
            {
                Title = product.Title,
                Price = product.Price,
                ImageUrl = product.ProductImages.FirstOrDefault()?.Url ?? string.Empty // Asignar la primera imagen o una cadena vacía si no hay imágenes
            };
        }

        public static ProductDtoAdmin MapToProductDtoAdmin(this Product product)
        {
            return new ProductDtoAdmin
            {
                Title = product.Title,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Brand = product.Brand,
                IsNew = product.IsNew,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }
}