using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StationRepository : BaseRepository<Station>, IStationRepository
{
    public StationRepository(CybercafeDbContext context) : base(context)
    {
    }

    public async Task<Station?> GetByStationNumberAsync(int stationNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.StationNumber == stationNumber);
    }

    public async Task<IEnumerable<Station>> GetByStatusAsync(StationStatus status)
    {
        return await _dbSet
            .Where(s => s.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Station>> GetAvailableStationsAsync()
    {
        return await GetByStatusAsync(StationStatus.Free);
    }

    public async Task UpdateStationStatusAsync(int stationId, StationStatus status)
    {
        var station = await GetByIdAsync(stationId)
            ?? throw new ArgumentException("Station not found");

        station.Status = status;
        await UpdateAsync(station);
    }

    public async Task<IEnumerable<Session>> GetStationSessionHistoryAsync(int stationId)
    {
        return await _context.Sessions
            .Include(s => s.Customer)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .Where(s => s.StationId == stationId)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<bool> IsStationAvailableAsync(int stationId)
    {
        var station = await GetByIdAsync(stationId);
        return station?.Status == StationStatus.Free;
    }

    public override async Task<Station?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Sessions)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}