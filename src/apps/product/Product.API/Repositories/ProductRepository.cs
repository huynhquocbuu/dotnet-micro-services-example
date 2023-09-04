using Contracts.Domains.Interfaces;
using Infrastructure.Domains;
using Microsoft.EntityFrameworkCore;
using Product.API.Persistence;

namespace Product.API.Repositories;

public class ProductRepository : RepositoryBase<Entities.Product, long, ProductDbContext>, IProductRepository
{
    public ProductRepository(ProductDbContext dbContext, IUnitOfWork<ProductDbContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }
   
    public async Task<IEnumerable<Entities.Product>> GetProductsAsync() => await FindAll().ToListAsync();

    public Task<Entities.Product> GetProductAsync(long id) => GetByIdAsync(id);

    public Task<Entities.Product> GetProductByNoAsync(string productNo) =>
        FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();

    public Task CreateProductAsync(Entities.Product product) => CreateAsync(product);

    public Task UpdateProductAsync(Entities.Product product) => UpdateAsync(product);

    public async Task DeleteProductAsync(long id)
    {
        var product = await GetProductAsync(id);
        if (product != null) await DeleteAsync(product);
    }
}