using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class StationService : IStationService
{
    private readonly IStationRepository _stationRepository;
    private readonly ISessionRepository _sessionRepository;

    public StationService(IStationRepository stationRepository, ISessionRepository sessionRepository)
    {
        _stationRepository = stationRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<Station> AddStationAsync(Station station)
    {
        station.CreatedAt = DateTime.UtcNow;
        station.Status = StationStatus.Free;
        return await _stationRepository.AddAsync(station);
    }

    public async Task<Station?> GetStationAsync(int id)
    {
        return await _stationRepository.GetByIdAsync(id);
    }

    public async Task<Station?> GetStationByNumberAsync(int stationNumber)
    {
        return await _stationRepository.GetByStationNumberAsync(stationNumber);
    }

    public async Task<IEnumerable<Station>> GetAllStationsAsync()
    {
        return await _stationRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Station>> GetAvailableStationsAsync()
    {
        return await _stationRepository.GetAvailableStationsAsync();
    }

    public async Task UpdateStationAsync(Station station)
    {
        station.UpdatedAt = DateTime.UtcNow;
        await _stationRepository.UpdateAsync(station);
    }

    public async Task DeleteStationAsync(int id)
    {
        var station = await _stationRepository.GetByIdAsync(id);
        if (station == null) return;

        var activeSession = await _sessionRepository.GetActiveSessionByStationAsync(id);
        if (activeSession != null)
            throw new InvalidOperationException("Cannot delete station with active session");

        await _stationRepository.DeleteAsync(station);
    }

    public async Task<bool> UpdateStationStatusAsync(int stationId, StationStatus status)
    {
        var station = await _stationRepository.GetByIdAsync(stationId);
        if (station == null) return false;

        if (status == StationStatus.Free && station.Status != StationStatus.Free)
        {
            var activeSession = await _sessionRepository.GetActiveSessionByStationAsync(stationId);
            if (activeSession != null)
                throw new InvalidOperationException("Cannot set station as free while session is active");
        }

        await _stationRepository.UpdateStationStatusAsync(stationId, status);
        return true;
    }

    public async Task<IEnumerable<Session>> GetStationHistoryAsync(int stationId)
    {
        return await _stationRepository.GetStationSessionHistoryAsync(stationId);
    }
}