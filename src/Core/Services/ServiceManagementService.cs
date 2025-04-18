using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class ServiceManagementService : IServiceManagementService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceManagementService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Service> AddServiceAsync(Service service)
    {
        service.CreatedAt = DateTime.UtcNow;
        if (service.MinimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative");

        return await _serviceRepository.AddAsync(service);
    }

    public async Task<Service?> GetServiceAsync(int id)
    {
        return await _serviceRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Service>> GetAllServicesAsync()
    {
        return await _serviceRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Service>> GetServicesByCategoryAsync(ServiceCategory category)
    {
        return await _serviceRepository.GetByCategory(category);
    }

    public async Task UpdateServiceAsync(Service service)
    {
        if (service.MinimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative");

        service.UpdatedAt = DateTime.UtcNow;
        await _serviceRepository.UpdateAsync(service);
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service != null)
        {
            await _serviceRepository.DeleteAsync(service);
        }
    }

    public async Task<bool> UpdateStockAsync(int serviceId, int quantity)
    {
        var service = await _serviceRepository.GetByIdAsync(serviceId);
        if (service == null) return false;

        var newStock = service.CurrentStock + quantity;
        if (newStock < 0)
            throw new InvalidOperationException("Stock cannot be negative");

        service.CurrentStock = newStock;
        service.UpdatedAt = DateTime.UtcNow;
        await _serviceRepository.UpdateAsync(service);

        return true;
    }

    public async Task<IEnumerable<Service>> GetLowStockServicesAsync()
    {
        return await _serviceRepository.GetLowStockServicesAsync();
    }

    public async Task<bool> IsServiceAvailableAsync(int serviceId, int quantity)
    {
        return await _serviceRepository.IsInStockAsync(serviceId, quantity);
    }
}