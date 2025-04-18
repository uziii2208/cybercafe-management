using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ServiceOrderRepository : BaseRepository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(CybercafeDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceOrder>> GetBySessionAsync(int sessionId)
    {
        return await _dbSet
            .Include(o => o.Service)
            .Include(o => o.Session)
                .ThenInclude(s => s.Customer)
            .Where(o => o.SessionId == sessionId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByCustomerAsync(int customerId)
    {
        return await _dbSet
            .Include(o => o.Service)
            .Include(o => o.Session)
            .Where(o => o.Session.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(o => o.Service)
            .Include(o => o.Session)
                .ThenInclude(s => s.Customer)
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalOrderAmountAsync(int orderId)
    {
        var order = await GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order not found");
            
        return order.TotalPrice;
    }

    public async Task<IEnumerable<ServiceOrder>> GetPendingOrdersAsync()
    {
        return await _dbSet
            .Include(o => o.Service)
            .Include(o => o.Session)
                .ThenInclude(s => s.Customer)
            .Where(o => o.Status == OrderStatus.Pending)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public override async Task<ServiceOrder?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Service)
            .Include(o => o.Session)
                .ThenInclude(s => s.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}