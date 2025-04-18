using Core.Entities;

namespace Core.Interfaces;

public interface ISessionRepository : IBaseRepository<Session>
{
    Task<IEnumerable<Session>> GetActiveSessionsAsync();
    Task<Session?> GetActiveSessionByStationAsync(int stationId);
    Task<Session?> GetActiveSessionByCustomerAsync(int customerId);
    Task<decimal> CalculateSessionCostAsync(int sessionId);
    Task EndSessionAsync(int sessionId);
    Task<IEnumerable<Session>> GetSessionsByDateRangeAsync(DateTime startDate, DateTime endDate);
}