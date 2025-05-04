using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

                // Crear roles
                var roleAdmin = new Role { Name = "Administrador" };
                var roleClient = new Role { Name = "Cliente" };

                // Crear administrador fijo
                var adminUser = new User
                {
                    Name = "Ignacio Mancilla",
                    Email = "ignacio.mancilla@gmail.com",
                    Phone = "+56 9 1234 5678",
                    BirthDate = new DateOnly(1990, 1, 1),
                    Password = "Pa$$word2025", // ¡No olvides hashear esto en producción!
                    Active = true,
                    Role = roleAdmin,
                    Direction = new Direction
                    {
                        street = "Av. Principal",
                        number = "100",
                        city = "Santiago",
                        state = "RM",
                        zipCode = "8320000"
                    }
                };

                var users = new List<User> { adminUser };

                // Crear 10 usuarios cliente aleatorios
                for (int i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        Name = faker.Name.FullName(),
                        Email = faker.Internet.Email(),
                        Phone = faker.Phone.PhoneNumber(),
                        BirthDate = DateOnly.FromDateTime(faker.Date.Past(30, DateTime.Now.AddYears(-18))),
                        Password = faker.Internet.Password(),
                        Active = faker.Random.Bool(),
                        Role = roleClient,
                        Direction = new Direction
                        {
                            street = faker.Address.StreetName(),
                            number = faker.Address.BuildingNumber(),
                            city = faker.Address.City(),
                            state = faker.Address.State(),
                            zipCode = faker.Address.ZipCode()
                        }
                    };

                    users.Add(user);
                }

                // Crear categorías únicas
                var categories = new List<Category>();
                for (int i = 0; i < 5; i++) // Generar 5 categorías únicas
                {
                    categories.Add(new Category
                    {
                        Name = faker.Commerce.Categories(1).First()
                    });
                }

                // Crear marcas únicas
                var brands = new List<Brand>();
                for (int i = 0; i < 5; i++) // Generar 5 marcas únicas
                {
                    brands.Add(new Brand
                    {
                        Name = faker.Company.CompanyName()
                    });
                }

                var products = new List<Product>();
                for (int i = 0; i < 10; i++)
                {
                    // Seleccionar una categoría y una marca aleatoria
                    var category = categories[faker.Random.Int(0, categories.Count - 1)];
                    var brand = brands[faker.Random.Int(0, brands.Count - 1)];

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
                        Category = category,
                        Brand = brand,
                        IsNew = faker.Random.Bool(),
                        ProductImages = productImages,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = faker.Date.Recent(30),
                        IsVisible = faker.Random.Bool()
                    };

                    products.Add(product);
                }

                // Guardar en base de datos
                context.Roles.AddRange(roleAdmin, roleClient);
                context.Users.AddRange(users);
                context.Categories.AddRange(categories);
                context.Brands.AddRange(brands);
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}