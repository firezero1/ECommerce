using ECommerce.Context;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository
{
    public class OrderNoSeqRepository : Repository<OrderNoSeq>
    {
        private readonly DbSet<OrderNoSeq> _dbSet;

        public OrderNoSeqRepository(ECommerceDbContext context) : base(context)
        {
            _dbSet = context.Set<OrderNoSeq>();
        }

        public async Task<OrderNoSeq> GetOrderNoSeq(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                throw new ArgumentException("Date string cannot be null or empty.", nameof(dateString));
            }
            else if (!DateTime.TryParseExact(dateString, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Date string must be in 'yyyyMMdd' format.", nameof(dateString));

            }
            else
            {
                var query = _dbSet.AsQueryable();
                var result = await query.Where(o => o.OrderDate == dateString).FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
