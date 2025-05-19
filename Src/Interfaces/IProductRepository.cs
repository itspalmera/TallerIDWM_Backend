using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task RemoveAllProductsImagesAsync(Product product);
        Task DeleteProduct(Product product);
        Task RemoveProductAsync(Product product);
        IQueryable<Product> GetQueryableProducts();
    }
}