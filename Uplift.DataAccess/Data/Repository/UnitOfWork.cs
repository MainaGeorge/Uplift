using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
            Frequency = new FrequencyRepository(db);
            Service = new ServiceRepository(db);
            OrderDetails = new OrderDetailsRepository(db);
            OrderHeader = new OrderHeaderRepository(db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void SaveChanges()
        {

            _db.SaveChanges();
        }

        public ICategoryRepository Category { get; private set; }
        public IFrequencyRepository Frequency { get; private set; }
        public IServiceRepository Service { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
    }
}