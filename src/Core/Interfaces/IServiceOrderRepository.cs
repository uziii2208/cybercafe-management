using Core.Entities;

namespace Core.Interfaces;

public interface IServiceOrderRepository : IBaseRepository<ServiceOrder>
{
    Task<IEnumerable<ServiceOrder>> GetBySessionAsync(int sessionId);
    Task<IEnumerable<ServiceOrder>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<ServiceOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalOrderAmountAsync(int orderId);
    Task<IEnumerable<ServiceOrder>> GetPendingOrdersAsync();
}