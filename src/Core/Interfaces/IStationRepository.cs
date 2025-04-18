using Core.Entities;

namespace Core.Interfaces;

public interface IStationRepository : IBaseRepository<Station>
{
    Task<Station?> GetByStationNumberAsync(int stationNumber);
    Task<IEnumerable<Station>> GetByStatusAsync(StationStatus status);
    Task<IEnumerable<Station>> GetAvailableStationsAsync();
    Task UpdateStationStatusAsync(int stationId, StationStatus status);
    Task<IEnumerable<Session>> GetStationSessionHistoryAsync(int stationId);
    Task<bool> IsStationAvailableAsync(int stationId);
}