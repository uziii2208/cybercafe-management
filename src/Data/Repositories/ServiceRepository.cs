using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(CybercafeDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Service>> GetByCategory(ServiceCategory category)
    {
        return await _dbSet
            .Where(s => s.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetLowStockServicesAsync(int threshold = 0)
    {
        return await _dbSet
            .Where(s => s.CurrentStock <= (threshold == 0 ? s.MinimumStock : threshold))
            .ToListAsync();
    }

    public async Task UpdateStockAsync(int serviceId, int quantity)
    {
        var service = await GetByIdAsync(serviceId)
            ?? throw new ArgumentException("Service not found");

        var newStock = service.CurrentStock + quantity;
        if (newStock < 0)
            throw new InvalidOperationException("Stock cannot be negative");

        service.CurrentStock = newStock;
        await UpdateAsync(service);
    }

    public async Task<bool> IsInStockAsync(int serviceId, int requiredQuantity)
    {
        var service = await GetByIdAsync(serviceId);
        return service?.CurrentStock >= requiredQuantity;
    }

    public async Task<IEnumerable<Service>> SearchServicesByNameAsync(string searchTerm)
    {
        return await _dbSet
            .Where(s => s.Name.Contains(searchTerm))
            .ToListAsync();
    }

    public override async Task<Service?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Orders)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}