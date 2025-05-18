using Microsoft.EntityFrameworkCore;

using Serilog;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Repository;
using TallerIDWM_Backend.Src.Services;

Log.Logger = new LoggerConfiguration()

    .CreateLogger();
try
{
    Log.Information("Inicializando la aplicación...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    builder.Services.AddScoped<IPhotoService, PhotoService>();
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName();

    });

    var corsSettings = builder.Configuration.GetSection("CorsSettings");
    var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
    var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>();
    var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DefaultCorsPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins!)
                  .WithHeaders(allowedHeaders!)
                  .WithMethods(allowedMethods!)
                  .AllowCredentials();
        });
    });

    // Configurar logging
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataContext>();

        // Migraciones pendientes
        await context.Database.MigrateAsync();

        // Poblar la base de datos con el DataSeeder
        DataSeeder.Initialize(services);
    }

    app.UseCors("DefaultCorsPolicy");
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
