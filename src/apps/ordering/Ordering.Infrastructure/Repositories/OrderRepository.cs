using Contracts.Domains.Interfaces;
using Infrastructure.Domains;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Repositories;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order, long, OrderDbContext>, IOrderRepository
{
    public OrderRepository(OrderDbContext dbContext, IUnitOfWork<OrderDbContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName) => 
        await FindByCondition(x => x.UserName.Equals(userName))
            .ToListAsync();

    public void CreateOrder(Order order) => Create(order);

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        await UpdateAsync(order);
        return order;
    }

    public void DeleteOrder(Order order) => Delete(order);
}