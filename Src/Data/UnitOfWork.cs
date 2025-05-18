using TallerIDWM_Backend.Src.Interfaces;

namespace TallerIDWM_Backend.Src.Data
{
    public class UnitOfWork(DataContext context, IProductRepository productRepository, IUserRepository userRepository, IDirectionRepository directionRepository)
    {
        private readonly DataContext _context = context;
        public IProductRepository ProductRepository { get; set; } = productRepository;
        public IUserRepository UserRepository { get; set; } = userRepository;
        public IDirectionRepository DirectionRepository { get; set; } = directionRepository;
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}