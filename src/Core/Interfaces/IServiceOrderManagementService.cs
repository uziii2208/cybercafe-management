using Core.Entities;

namespace Core.Interfaces;

public interface IServiceOrderManagementService
{
    Task<ServiceOrder> CreateOrderAsync(int sessionId, int serviceId, int quantity);
    Task<ServiceOrder> CompleteOrderAsync(int orderId);
    Task<ServiceOrder> CancelOrderAsync(int orderId);
    Task<ServiceOrder?> GetOrderAsync(int orderId);
    Task<IEnumerable<ServiceOrder>> GetSessionOrdersAsync(int sessionId);
    Task<IEnumerable<ServiceOrder>> GetPendingOrdersAsync();
    Task<decimal> CalculateOrderTotalAsync(int orderId);
}