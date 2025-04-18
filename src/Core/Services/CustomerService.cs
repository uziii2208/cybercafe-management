using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> RegisterCustomerAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        customer.MembershipTier = MembershipTier.Regular;
        customer.MembershipPoints = 0;
        return await _customerRepository.AddAsync(customer);
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer != null)
        {
            await _customerRepository.DeleteAsync(customer);
        }
    }

    public async Task<bool> UpdateMembershipTierAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) return false;

        var previousTier = customer.MembershipTier;
        customer.MembershipTier = customer.MembershipPoints switch
        {
            >= 1000 => MembershipTier.VIP,
            >= 500 => MembershipTier.Gold,
            >= 200 => MembershipTier.Silver,
            _ => MembershipTier.Regular
        };

        if (previousTier != customer.MembershipTier)
        {
            await UpdateCustomerAsync(customer);
            return true;
        }

        return false;
    }

    public async Task<int> AddMembershipPointsAsync(int customerId, int points)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new ArgumentException("Customer not found");

        customer.MembershipPoints += points;
        await UpdateCustomerAsync(customer);
        await UpdateMembershipTierAsync(customerId);

        return customer.MembershipPoints;
    }

    public async Task<IEnumerable<Session>> GetCustomerHistoryAsync(int customerId)
    {
        return await _customerRepository.GetCustomerSessionHistoryAsync(customerId);
    }
}