using Core.Entities;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public class DbSeeder
{
    private readonly CybercafeDbContext _context;

    public DbSeeder(CybercafeDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!await _context.Customers.AnyAsync())
        {
            await SeedCustomersAsync();
        }

        if (!await _context.Stations.AnyAsync())
        {
            await SeedStationsAsync();
        }

        if (!await _context.Services.AnyAsync())
        {
            await SeedServicesAsync();
        }
    }

    private async Task SeedCustomersAsync()
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                Name = "John Smith",
                Email = "john@example.com",
                Phone = "123-456-7890",
                MembershipTier = MembershipTier.Regular,
                MembershipPoints = 0,
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                Phone = "098-765-4321",
                MembershipTier = MembershipTier.Gold,
                MembershipPoints = 500,
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Name = "Mike Johnson",
                Email = "mike@example.com",
                Phone = "555-555-5555",
                MembershipTier = MembershipTier.Silver,
                MembershipPoints = 250,
                CreatedAt = DateTime.UtcNow
            }
        };

        await _context.Customers.AddRangeAsync(customers);
        await _context.SaveChangesAsync();
    }

    private async Task SeedStationsAsync()
    {
        var stations = new List<Station>
        {
            new Station
            {
                StationNumber = 1,
                HardwareSpecification = "RTX 4080, i9-13900K, 32GB RAM",
                Status = StationStatus.Free,
                HourlyRate = 5.00m,
                CreatedAt = DateTime.UtcNow
            },
            new Station
            {
                StationNumber = 2,
                HardwareSpecification = "RTX 4070, i7-13700K, 32GB RAM",
                Status = StationStatus.Free,
                HourlyRate = 4.00m,
                CreatedAt = DateTime.UtcNow
            },
            new Station
            {
                StationNumber = 3,
                HardwareSpecification = "RTX 4060, i5-13600K, 16GB RAM",
                Status = StationStatus.Free,
                HourlyRate = 3.00m,
                CreatedAt = DateTime.UtcNow
            }
        };

        await _context.Stations.AddRangeAsync(stations);
        await _context.SaveChangesAsync();
    }

    private async Task SeedServicesAsync()
    {
        var services = new List<Service>
        {
            new Service
            {
                Name = "Cola",
                Description = "Refreshing carbonated drink",
                Category = ServiceCategory.Beverage,
                Price = 2.00m,
                CurrentStock = 50,
                MinimumStock = 10,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Name = "Chips",
                Description = "Potato chips snack",
                Category = ServiceCategory.Food,
                Price = 1.50m,
                CurrentStock = 30,
                MinimumStock = 5,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Name = "Gaming Mouse",
                Description = "High-performance gaming mouse",
                Category = ServiceCategory.Peripheral,
                Price = 15.00m,
                CurrentStock = 10,
                MinimumStock = 2,
                CreatedAt = DateTime.UtcNow
            }
        };

        await _context.Services.AddRangeAsync(services);
        await _context.SaveChangesAsync();
    }
}