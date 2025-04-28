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
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Direction> Directions { get; set; } = null!;

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     // Relación uno a uno entre User y Role
        //     modelBuilder.Entity<User>()
        //         .HasOne(u => u.Role)
        //         .WithMany() // Si un Role puede estar asociado a múltiples usuarios, usa WithMany()
        //         .HasForeignKey(u => u.RoleId)
        //         .OnDelete(DeleteBehavior.Restrict); // Evita eliminaciones en cascada

        //     // Relación uno a uno entre User y Direction
        //     modelBuilder.Entity<User>()
        //         .HasOne(u => u.Direction)
        //         .WithOne(d => d.User)
        //         .HasForeignKey<Direction>(d => d.Id)
        //         .OnDelete(DeleteBehavior.Cascade); // Elimina la dirección si se elimina el usuario

        //     // Relación uno a muchos entre Product y Category
        //     modelBuilder.Entity<Product>()
        //         .HasOne(p => p.Category)
        //         .WithMany(c => c.Products)
        //         .HasForeignKey(p => p.CategoryId)
        //         .OnDelete(DeleteBehavior.Restrict);

        //     // Relación uno a muchos entre Product y Brand
        //     modelBuilder.Entity<Product>()
        //         .HasOne(p => p.Brand)
        //         .WithMany(b => b.Products)
        //         .HasForeignKey(p => p.BrandId)
        //         .OnDelete(DeleteBehavior.Restrict);

        //     // Relación uno a muchos entre Product y ProductImage
        //     modelBuilder.Entity<ProductImage>()
        //         .HasOne(pi => pi.Product)
        //         .WithMany(p => p.ProductImages)
        //         .HasForeignKey(pi => pi.ProductId)
        //         .OnDelete(DeleteBehavior.Cascade); // Elimina las imágenes si se elimina el producto
        // }
        
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