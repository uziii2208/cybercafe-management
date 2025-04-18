using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class ServiceOrderManagementService : IServiceOrderManagementService
{
    private readonly IServiceOrderRepository _orderRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IServiceRepository _serviceRepository;

    public ServiceOrderManagementService(
        IServiceOrderRepository orderRepository,
        ISessionRepository sessionRepository,
        IServiceRepository serviceRepository)
    {
        _orderRepository = orderRepository;
        _sessionRepository = sessionRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceOrder> CreateOrderAsync(int sessionId, int serviceId, int quantity)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new ArgumentException("Session not found");

        var service = await _serviceRepository.GetByIdAsync(serviceId)
            ?? throw new ArgumentException("Service not found");

        if (!await _serviceRepository.IsInStockAsync(serviceId, quantity))
            throw new InvalidOperationException("Insufficient stock");

        var order = new ServiceOrder
        {
            SessionId = sessionId,
            ServiceId = serviceId,
            Quantity = quantity,
            UnitPrice = service.Price,
            TotalPrice = service.Price * quantity,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _serviceRepository.UpdateStockAsync(serviceId, -quantity);
        return await _orderRepository.AddAsync(order);
    }

    public async Task<ServiceOrder> CompleteOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order not found");

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is not pending");

        order.Status = OrderStatus.Completed;
        order.UpdatedAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order);

        return order;
    }

    public async Task<ServiceOrder> CancelOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order not found");

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is not pending");

        // Return items to stock
        await _serviceRepository.UpdateStockAsync(order.ServiceId, order.Quantity);

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order);

        return order;
    }

    public async Task<ServiceOrder?> GetOrderAsync(int orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task<IEnumerable<ServiceOrder>> GetSessionOrdersAsync(int sessionId)
    {
        return await _orderRepository.GetBySessionAsync(sessionId);
    }

    public async Task<IEnumerable<ServiceOrder>> GetPendingOrdersAsync()
    {
        return await _orderRepository.GetPendingOrdersAsync();
    }

    public async Task<decimal> CalculateOrderTotalAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order not found");

        return order.TotalPrice;
    }
}