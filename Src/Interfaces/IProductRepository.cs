using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IQueryable<Product> GetQueryableProducts();
    }
}