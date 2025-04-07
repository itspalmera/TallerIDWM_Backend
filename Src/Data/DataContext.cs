using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Data
{
    public class DataContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación uno a uno entre User y Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithOne()
                .HasForeignKey<User>(u => u.RoleId);

            // Relación uno a uno entre User y Direction
            modelBuilder.Entity<User>()
                .HasOne(u => u.Direction)
                .WithOne(d => d.User)
                .HasForeignKey<Direction>(d => d.Id);
        }
        
    }
}