using System.Linq;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Service service)
        {
            var toUpdate = _db.Service.FirstOrDefault(s => s.Id == service.Id);
            if (toUpdate != null)
            {
                toUpdate.Name = service.Name;
                toUpdate.CategoryId = service.CategoryId;
                toUpdate.ImageUrl = service.ImageUrl;
                toUpdate.Price = service.Price;
                toUpdate.LongDescription = service.LongDescription;
                toUpdate.FrequencyId = service.FrequencyId;


                _db.SaveChanges();
            }
        }
    }
}
