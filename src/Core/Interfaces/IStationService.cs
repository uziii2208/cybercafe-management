using Core.Entities;

namespace Core.Interfaces;

public interface IStationService
{
    Task<Station> AddStationAsync(Station station);
    Task<Station?> GetStationAsync(int id);
    Task<Station?> GetStationByNumberAsync(int stationNumber);
    Task<IEnumerable<Station>> GetAllStationsAsync();
    Task<IEnumerable<Station>> GetAvailableStationsAsync();
    Task UpdateStationAsync(Station station);
    Task DeleteStationAsync(int id);
    Task<bool> UpdateStationStatusAsync(int stationId, StationStatus status);
    Task<IEnumerable<Session>> GetStationHistoryAsync(int stationId);
}