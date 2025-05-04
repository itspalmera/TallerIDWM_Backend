using TallerIDWM_Backend.Src.Interfaces;

namespace TallerIDWM_Backend.Src.Data
{
    public class UnitOfWork(DataContext context, IProductRepository productRepository, IUserRepository userRepository)
    {
        private readonly DataContext _context = context;
        public IProductRepository ProductRepository { get; set; } = productRepository;
        public IUserRepository UserRepository { get; set; } = userRepository;
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}