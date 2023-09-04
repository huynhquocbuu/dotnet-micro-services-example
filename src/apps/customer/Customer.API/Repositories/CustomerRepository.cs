using Customer.API.Persistence;
using Infrastructure.Domains;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryQueryBase<Entities.Customer, int, CustomerDbContext>, ICustomerRepository
{
    public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Entities.Customer> GetCustomerByUserNameAsync(string username) =>
        FindByCondition(x => x.UserName.Equals(username))
            .SingleOrDefaultAsync();
}