using TallerIDWM_Backend.Src.Interfaces;

namespace TallerIDWM_Backend.Src.Data
{
    public class UnitOfWork(DataContext context, IProductRepository productRepository, IUserRepository userRepository, IBasketRepository basketRepository)
    {
        private readonly DataContext _context = context;
        public IProductRepository ProductRepository { get; set; } = productRepository;
        public IUserRepository UserRepository { get; set; } = userRepository;
        public IBasketRepository BasketRepository { get; set; } = basketRepository;
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}