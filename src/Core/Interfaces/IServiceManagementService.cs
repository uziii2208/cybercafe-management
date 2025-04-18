using Core.Entities;

namespace Core.Interfaces;

public interface IServiceManagementService
{
    Task<Service> AddServiceAsync(Service service);
    Task<Service?> GetServiceAsync(int id);
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task<IEnumerable<Service>> GetServicesByCategoryAsync(ServiceCategory category);
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(int id);
    Task<bool> UpdateStockAsync(int serviceId, int quantity);
    Task<IEnumerable<Service>> GetLowStockServicesAsync();
    Task<bool> IsServiceAvailableAsync(int serviceId, int quantity);
}