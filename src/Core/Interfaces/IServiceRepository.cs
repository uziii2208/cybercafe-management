using Core.Entities;

namespace Core.Interfaces;

public interface IServiceRepository : IBaseRepository<Service>
{
    Task<IEnumerable<Service>> GetByCategory(ServiceCategory category);
    Task<IEnumerable<Service>> GetLowStockServicesAsync(int threshold = 0);
    Task UpdateStockAsync(int serviceId, int quantity);
    Task<bool> IsInStockAsync(int serviceId, int requiredQuantity);
    Task<IEnumerable<Service>> SearchServicesByNameAsync(string searchTerm);
}