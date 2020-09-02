using System.Linq;
using Microsoft.EntityFrameworkCore;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void ChangeOrderStatus(int orderHeaderId, string status)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(h => h.Id == orderHeaderId);

            if (orderFromDb == null) return;

            orderFromDb.Status = status;
            _db.SaveChanges();
        }
    }
}
