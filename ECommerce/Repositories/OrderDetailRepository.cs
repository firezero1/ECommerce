using ECommerce.Context;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>
    {
        private readonly DbSet<OrderDetail> _dbSet;
        private readonly ECommerceDbContext _context; // Add this field to store the context  

        public OrderDetailRepository(ECommerceDbContext context) : base(context)
        {
            _dbSet = context.Set<OrderDetail>();
            _context = context; // Assign the context to the field  
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet.Where(od => od.OrderId == orderId).ToListAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<int> ids)
        {
            var entities = await _dbSet.Where(e => ids.Contains(e.OrderDetailId)).ToListAsync();
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync(); // Use the _context field here  
        }
    }
}
