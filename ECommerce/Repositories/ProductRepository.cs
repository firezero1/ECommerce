using ECommerce.Context;
using ECommerce.Models;

namespace ECommerce.Repository
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(ECommerceDbContext context) : base(context) { }
    }
}
