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
        public void DeleteProductAsync(Product product)
        {
            _dataContext.Products.Remove(product);
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dataContext.Products.ToListAsync() ?? throw new Exception("No products found.");
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dataContext.Products.FirstAsync(p => p.Id == id) ?? throw new Exception("Product not found.");
        }
        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _dataContext.Products.FindAsync(product.Id) ?? throw new Exception("Product not found.");
            existingProduct.Title = product.Title;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Category = product.Category;
            existingProduct.Brand = product.Brand;
            existingProduct.IsNew = product.IsNew;
            existingProduct.IsVisible = product.IsVisible;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.ProductImages = product.ProductImages;
            _dataContext.Products.Update(existingProduct);
        }
        public IQueryable<Product> GetQueryableProducts()
        {
            return _dataContext.Products.AsQueryable();
        }
    }
}