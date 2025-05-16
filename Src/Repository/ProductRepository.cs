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
        public void DeleteProduct(Product product)
        {
            _dataContext.Products.Remove(product);
        }
        public async Task RemoveProduct(Product product)
        {
            var existingProduct = await _dataContext.Products.FindAsync(product.Id) ?? throw new Exception("El producto no fue encontrado.");
            existingProduct.IsVisible = false;
            _dataContext.Products.Update(existingProduct);
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dataContext.Products.Include(p => p.ProductImages).ToListAsync() ?? throw new Exception("No se encontraron productos.");
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dataContext.Products.Include(p => p.ProductImages).FirstAsync(p => p.Id == id) ?? throw new Exception("El producto no fue encontrado.");
        }
        public void UpdateProduct(Product product)
        {
            _dataContext.Products.Update(product);
        }
        public IQueryable<Product> GetQueryableProducts()
        {
            return _dataContext.Products.Include(p => p.ProductImages).Where(p => p.IsVisible == true).AsQueryable();
        }
    }
}