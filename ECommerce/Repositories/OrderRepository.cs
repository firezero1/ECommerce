using ECommerce.Context;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository
{
    public class OrderRepository : Repository<Order>
    {
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(ECommerceDbContext context) : base(context)
        {
            _dbSet = context.Set<Order>();
        }

        /// <summary>  
        /// 根據 OrderNo、Customer 和 CreateDate 查詢訂單資料  
        /// </summary>  
        /// <param name="orderNo">訂單編號 (可為 null)</param>  
        /// <param name="customer">客戶姓名 (可為 null)</param>  
        /// <param name="startDate">建立日期 (可為 null)</param>  
        /// <returns>符合條件的訂單列表</returns>  
        public async Task<IEnumerable<Order>> GetOrders(string? orderNo, string? customer, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(orderNo))
            {
                query = query.Where(o => o.OrderNo.Contains(orderNo));
            }

            if (!string.IsNullOrEmpty(customer))
            {
                query = query.Where(o => o.Customer.Contains(customer));
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreateDate.HasValue && o.CreateDate.Value.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreateDate.HasValue && o.CreateDate.Value.Date <= endDate.Value.Date);
            }

            return await query.ToListAsync();
        }
    }
}
