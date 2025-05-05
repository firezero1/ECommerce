using ECommerce.Context;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository
{
    public class ProductRepository : Repository<Product>
    {

        private readonly DbSet<Product> _dbSet;

        public ProductRepository(ECommerceDbContext context) : base(context)
        {
            _dbSet = context.Set<Product>();
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string? productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return await _dbSet.ToListAsync();
            }

            return await _dbSet
                .Where(p => EF.Functions.Like(p.ProductName, $"%{productName}%"))
                .ToListAsync();
        }
    }
}
