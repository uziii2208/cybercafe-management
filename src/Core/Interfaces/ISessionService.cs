using Core.Entities;

namespace Core.Interfaces;

public interface ISessionService
{
    Task<Session> StartSessionAsync(int customerId, int stationId);
    Task<Session> EndSessionAsync(int sessionId);
    Task<IEnumerable<Session>> GetActiveSessionsAsync();
    Task<decimal> CalculateSessionCostAsync(int sessionId);
    Task<Session?> GetActiveSessionByCustomerAsync(int customerId);
    Task<Session?> GetActiveSessionByStationAsync(int stationId);
    Task<IEnumerable<Session>> GetSessionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> IsStationAvailableAsync(int stationId);
}