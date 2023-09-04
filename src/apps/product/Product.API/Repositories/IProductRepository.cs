using Contracts.Domains.Interfaces;
using Product.API.Persistence;

namespace Product.API.Repositories;

public interface IProductRepository : IRepositoryBase<Entities.Product, long, ProductDbContext>
{
    Task<IEnumerable<Entities.Product>> GetProductsAsync();
    Task<Entities.Product> GetProductAsync(long id);
    Task<Entities.Product> GetProductByNoAsync(string productNo);
    Task CreateProductAsync(Entities.Product product);
    Task UpdateProductAsync(Entities.Product product);
    Task DeleteProductAsync(long id);
}