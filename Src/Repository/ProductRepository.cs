using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Repository
{
    public class ProductRepository(DataContext dataContext) : IProductRepository
    {
        private readonly DataContext _dataContext = dataContext;
        public async Task AddProductAsync(Product product)
        {
            await _dataContext.Products.AddAsync(product);
        }
        public Task DeleteProduct(Product product)
        {
            _dataContext.Products.Remove(product);
            return Task.CompletedTask;
        }
        public Task RemoveProductAsync(Product product)
        {
            product.IsVisible = false;
            _dataContext.Products.Update(product);
            return Task.CompletedTask;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dataContext.Products.Include(p => p.ProductImages).ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dataContext.Products.Include(p => p.ProductImages).FirstAsync(p => p.Id == id);
        }
        public Task UpdateProductAsync(Product product)
        {
            _dataContext.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task RemoveAllProductsImagesAsync(Product product)
        {
            product.ProductImages.Clear();
            _dataContext.Products.Update(product);
            return Task.CompletedTask;
        }

        public IQueryable<Product> GetQueryableProducts()
        {
            return _dataContext.Products.Include(p => p.ProductImages).Where(p => p.IsVisible == true).AsQueryable();
        }
    }
}