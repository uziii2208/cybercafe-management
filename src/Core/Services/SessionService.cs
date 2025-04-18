using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IStationRepository _stationRepository;
    private readonly ICustomerRepository _customerRepository;

    public SessionService(
        ISessionRepository sessionRepository,
        IStationRepository stationRepository,
        ICustomerRepository customerRepository)
    {
        _sessionRepository = sessionRepository;
        _stationRepository = stationRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Session> StartSessionAsync(int customerId, int stationId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId)
            ?? throw new ArgumentException("Customer not found");

        var station = await _stationRepository.GetByIdAsync(stationId)
            ?? throw new ArgumentException("Station not found");

        if (station.Status != StationStatus.Free)
            throw new InvalidOperationException("Station is not available");

        var activeSession = await GetActiveSessionByCustomerAsync(customerId);
        if (activeSession != null)
            throw new InvalidOperationException("Customer already has an active session");

        var session = new Session
        {
            CustomerId = customerId,
            StationId = stationId,
            StartTime = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _stationRepository.UpdateStationStatusAsync(stationId, StationStatus.Occupied);
        return await _sessionRepository.AddAsync(session);
    }

    public async Task<Session> EndSessionAsync(int sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new ArgumentException("Session not found");

        if (session.EndTime.HasValue)
            throw new InvalidOperationException("Session is already ended");

        session.EndTime = DateTime.UtcNow;
        session.TotalCost = await CalculateSessionCostAsync(sessionId);
        session.UpdatedAt = DateTime.UtcNow;

        await _sessionRepository.UpdateAsync(session);
        await _stationRepository.UpdateStationStatusAsync(session.StationId, StationStatus.Free);

        // Add membership points (1 point per hour)
        var hours = (session.EndTime.Value - session.StartTime).TotalHours;
        var points = (int)Math.Ceiling(hours);
        await _customerRepository.UpdateMembershipPointsAsync(session.CustomerId, points);

        return session;
    }

    public async Task<IEnumerable<Session>> GetActiveSessionsAsync()
    {
        return await _sessionRepository.GetActiveSessionsAsync();
    }

    public async Task<decimal> CalculateSessionCostAsync(int sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new ArgumentException("Session not found");

        var station = await _stationRepository.GetByIdAsync(session.StationId)
            ?? throw new ArgumentException("Station not found");

        var customer = await _customerRepository.GetByIdAsync(session.CustomerId)
            ?? throw new ArgumentException("Customer not found");

        var endTime = session.EndTime ?? DateTime.UtcNow;
        var duration = (endTime - session.StartTime).TotalHours;

        var baseCost = station.HourlyRate * (decimal)duration;

        // Apply membership discount
        var discount = customer.MembershipTier switch
        {
            MembershipTier.VIP => 0.20m,
            MembershipTier.Gold => 0.15m,
            MembershipTier.Silver => 0.10m,
            _ => 0m
        };

        return baseCost * (1 - discount);
    }

    public async Task<Session?> GetActiveSessionByCustomerAsync(int customerId)
    {
        return await _sessionRepository.GetActiveSessionByCustomerAsync(customerId);
    }

    public async Task<Session?> GetActiveSessionByStationAsync(int stationId)
    {
        return await _sessionRepository.GetActiveSessionByStationAsync(stationId);
    }

    public async Task<IEnumerable<Session>> GetSessionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _sessionRepository.GetSessionsByDateRangeAsync(startDate, endDate);
    }

    public async Task<bool> IsStationAvailableAsync(int stationId)
    {
        return await _stationRepository.IsStationAvailableAsync(stationId);
    }
}