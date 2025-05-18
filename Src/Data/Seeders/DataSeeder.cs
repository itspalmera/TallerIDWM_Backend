using Bogus;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Data.Seeders;
using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>()
                    ?? throw new InvalidOperationException("No se pudo obtener el contexto de la base de datos.");

                // Si ya hay usuarios, no hacer nada
                if (context.Users.Any() || context.Products.Any())
                    return;

                var faker = new Faker("es");

                var products = new List<Product>();
                for (int i = 0; i < 10; i++)
                {
                    // Crear imágenes aleatorias para el producto
                    var productImages = new List<ProductImage>
                    {
                        new ProductImage { Url = faker.Image.PicsumUrl() },
                        new ProductImage { Url = faker.Image.PicsumUrl() }
                    };

                    var product = new Product
                    {
                        Title = faker.Commerce.ProductName(),
                        Description = faker.Commerce.ProductDescription(),
                        Price = faker.Random.Int(1000, 100000),
                        Stock = faker.Random.Int(0, 100),
                        Category = faker.Commerce.Categories(1).First(),
                        Brand = faker.Company.CompanyName(),
                        IsNew = faker.Random.Bool(),
                        ProductImages = productImages,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = faker.Date.Recent(30),
                        IsVisible = faker.Random.Bool()
                    };

                    products.Add(product);
                }

                // Guardar en base de datos
                context.Products.AddRange(products);
                context.SaveChanges();
            }

        }



        public static async Task InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
                ?? throw new InvalidOperationException("Could not get UserManager");

            var context = scope.ServiceProvider.GetRequiredService<DataContext>()
                ?? throw new InvalidOperationException("Could not get StoreContext");

            await SeedData(context, userManager);
        }

        private static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            await context.Database.MigrateAsync();

            if (!context.Users.Any())
            {
                var userDtos = UserSeeder.GenerateUserDtos(10);
                await UserSeeder.CreateUsers(userManager, userDtos);
            }

            await context.SaveChangesAsync();
        }
    }
}