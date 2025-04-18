using Core.Entities;

namespace Core.Interfaces;

public interface ICustomerService
{
    Task<Customer> RegisterCustomerAsync(Customer customer);
    Task<Customer?> GetCustomerAsync(int id);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<bool> UpdateMembershipTierAsync(int customerId);
    Task<int> AddMembershipPointsAsync(int customerId, int points);
    Task<IEnumerable<Session>> GetCustomerHistoryAsync(int customerId);
}