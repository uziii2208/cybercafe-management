using Core.Entities;

namespace Core.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByPhoneAsync(string phone);
    Task<IEnumerable<Customer>> GetByMembershipTierAsync(MembershipTier tier);
    Task UpdateMembershipPointsAsync(int customerId, int points);
    Task<IEnumerable<Session>> GetCustomerSessionHistoryAsync(int customerId);
}