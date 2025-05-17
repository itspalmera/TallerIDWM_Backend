using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Data
{
    public class DataContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<Direction> Directions { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;

        // Genera problemas al ejecutar el proyecto
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     // Relación uno a uno entre User y Role
        //     modelBuilder.Entity<User>()
        //         .HasOne(u => u.Role)
        //         .WithOne()
        //         .HasForeignKey<User>(u => u.RoleId);

        //     // Relación uno a uno entre User y Direction
        //     modelBuilder.Entity<User>()
        //         .HasOne(u => u.Direction)
        //         .WithOne(d => d.User)
        //         .HasForeignKey<Direction>(d => d.Id);
        // }
    }
}