using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<Direction> Directions { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación uno a uno entre User y Direction
            modelBuilder.Entity<User>()
                .HasOne(u => u.Direction)
                .WithOne(d => d.User)
                .HasForeignKey<Direction>(d => d.UserId);

            List<IdentityRole> roles =
            [
                new IdentityRole { Id = "1" ,Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2" ,Name = "User", NormalizedName = "USER" }
            ];
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}