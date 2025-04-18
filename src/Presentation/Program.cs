using Core.Interfaces;
using Core.Services;
using Data.Context;
using Data.Repositories;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;
using Serilog;

var services = new ServiceCollection();

// Configure logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cybercafe.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Configure services
services.AddDbContext<CybercafeDbContext>(options =>
    options.UseSqlite("Data Source=cybercafe.db"));

// Register repositories
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<IStationRepository, StationRepository>();
services.AddScoped<ISessionRepository, SessionRepository>();
services.AddScoped<IServiceRepository, ServiceRepository>();
services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();

// Register services
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IStationService, StationService>();
services.AddScoped<ISessionService, SessionService>();
services.AddScoped<IServiceManagementService, ServiceManagementService>();
services.AddScoped<IServiceOrderManagementService, ServiceOrderManagementService>();

// Register presentation services
services.AddScoped<MenuService>();
services.AddScoped<DbSeeder>();

// Build service provider
var serviceProvider = services.BuildServiceProvider();

try
{
    // Ensure database is created and seeded
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CybercafeDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    Log.Information("Database created successfully");

    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();
    Log.Information("Database seeded successfully");

    // Run the menu service
    var menuService = scope.ServiceProvider.GetRequiredService<MenuService>();
    await menuService.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
