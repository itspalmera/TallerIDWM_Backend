using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
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
                var context = services.GetRequiredService<DataContext>();

                // Si ya hay usuarios, no hacer nada
                if (context.Users.Any())
                    return;

                var faker = new Faker("es");

                // Crear roles
                var roleAdmin = new Role { Name = "Administrador" };
                var roleClient = new Role { Name = "Cliente" };

                // Crear administrador fijo
                var adminUser = new User
                {
                    Name = "Admin General",
                    Email = "admin@tienda.cl",
                    Phone = "+56 9 1234 5678",
                    BirthDate = new DateOnly(1990, 1, 1),
                    Password = "Admin123*", // ¡No olvides hashear esto en producción!
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

                // Guardar en base de datos
                context.Roles.AddRange(roleAdmin, roleClient);
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}