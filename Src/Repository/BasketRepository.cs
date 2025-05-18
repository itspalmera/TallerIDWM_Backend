using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Repository
{
    public class BasketRepository(DataContext dataContext) : IBasketRepository
    {
        private readonly DataContext _dataContext = dataContext;
        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            return await _dataContext.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.ProductImages)
                .AsSplitQuery()
                .FirstAsync(b => b.BasketId == basketId);
        }
        
        public Basket CreateBasket(string basketId)
        {
            var basket = new Basket
            {
                BasketId = basketId,
            };

            _dataContext.Baskets.Add(basket);
            return basket;
        }

        public void DeleteBasket(Basket basket)
        {
            _dataContext.Baskets.Remove(basket);
        }
        public void UpdateBasket(Basket basket)
        {
            _dataContext.Baskets.Update(basket);
        }
    }
}