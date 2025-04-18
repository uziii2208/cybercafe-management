using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class CybercafeDbContext : DbContext
{
    public CybercafeDbContext(DbContextOptions<CybercafeDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Sessions)
            .WithOne(s => s.Customer)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Station configuration
        modelBuilder.Entity<Station>()
            .HasMany(s => s.Sessions)
            .WithOne(s => s.Station)
            .HasForeignKey(s => s.StationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Session configuration
        modelBuilder.Entity<Session>()
            .HasMany(s => s.ServiceOrders)
            .WithOne(o => o.Session)
            .HasForeignKey(o => o.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Service configuration
        modelBuilder.Entity<Service>()
            .HasMany(s => s.Orders)
            .WithOne(o => o.Service)
            .HasForeignKey(o => o.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Service Order configuration
        modelBuilder.Entity<ServiceOrder>()
            .Property(o => o.UnitPrice)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ServiceOrder>()
            .Property(o => o.TotalPrice)
            .HasPrecision(10, 2);
    }
}