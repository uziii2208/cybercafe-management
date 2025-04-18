using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class SessionRepository : BaseRepository<Session>, ISessionRepository
{
    public SessionRepository(CybercafeDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Session>> GetActiveSessionsAsync()
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Include(s => s.Station)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .Where(s => !s.EndTime.HasValue)
            .ToListAsync();
    }

    public async Task<Session?> GetActiveSessionByStationAsync(int stationId)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .FirstOrDefaultAsync(s => s.StationId == stationId && !s.EndTime.HasValue);
    }

    public async Task<Session?> GetActiveSessionByCustomerAsync(int customerId)
    {
        return await _dbSet
            .Include(s => s.Station)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .FirstOrDefaultAsync(s => s.CustomerId == customerId && !s.EndTime.HasValue);
    }

    public async Task<decimal> CalculateSessionCostAsync(int sessionId)
    {
        var session = await GetByIdAsync(sessionId)
            ?? throw new ArgumentException("Session not found");

        if (!session.EndTime.HasValue)
            throw new InvalidOperationException("Session is still active");

        var duration = (session.EndTime.Value - session.StartTime).TotalHours;
        var stationCost = session.Station.HourlyRate * (decimal)duration;

        var servicesCost = session.ServiceOrders
            .Where(o => o.Status == OrderStatus.Completed)
            .Sum(o => o.TotalPrice);

        return stationCost + servicesCost;
    }

    public async Task EndSessionAsync(int sessionId)
    {
        var session = await GetByIdAsync(sessionId)
            ?? throw new ArgumentException("Session not found");

        if (session.EndTime.HasValue)
            throw new InvalidOperationException("Session is already ended");

        session.EndTime = DateTime.UtcNow;
        session.TotalCost = await CalculateSessionCostAsync(sessionId);
        await UpdateAsync(session);
    }

    public async Task<IEnumerable<Session>> GetSessionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Include(s => s.Station)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .Where(s => s.StartTime >= startDate && s.StartTime <= endDate)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public override async Task<Session?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Include(s => s.Station)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}