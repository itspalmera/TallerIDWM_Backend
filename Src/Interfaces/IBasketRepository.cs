using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketAsync(string basketId);
        Basket CreateBasket(string basketId);
        void DeleteBasket(Basket basket);
        void UpdateBasket(Basket basket);
    }
}