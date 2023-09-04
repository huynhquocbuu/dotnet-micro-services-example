using Contracts.Domains.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories;

public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer, int, CustomerDbContext>
{
    Task<Entities.Customer> GetCustomerByUserNameAsync(string username);
}