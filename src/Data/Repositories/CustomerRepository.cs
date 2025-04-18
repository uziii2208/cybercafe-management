using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(CybercafeDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer?> GetByPhoneAsync(string phone)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Phone == phone);
    }

    public async Task<IEnumerable<Customer>> GetByMembershipTierAsync(MembershipTier tier)
    {
        return await _dbSet
            .Where(c => c.MembershipTier == tier)
            .ToListAsync();
    }

    public async Task UpdateMembershipPointsAsync(int customerId, int points)
    {
        var customer = await GetByIdAsync(customerId)
            ?? throw new ArgumentException("Customer not found");

        customer.MembershipPoints += points;
        await UpdateAsync(customer);
    }

    public async Task<IEnumerable<Session>> GetCustomerSessionHistoryAsync(int customerId)
    {
        return await _context.Sessions
            .Include(s => s.Station)
            .Include(s => s.ServiceOrders)
                .ThenInclude(o => o.Service)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public override async Task<Customer?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Sessions)
            .Include(c => c.ServiceOrders)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}