using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;

        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Frequency frequency)
        {
            var frequencyToUpdate = _db.Frequency.FirstOrDefault(f => f.Id == frequency.Id);
            if (frequencyToUpdate != null)
            {
                frequencyToUpdate.Name = frequency.Name;
                frequencyToUpdate.FrequencyCount = frequency.FrequencyCount;

                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return _db.Frequency.Select(f => new SelectListItem()
            {
                Text = f.Name,
                Value = f.Id.ToString()
            });
        }
    }
}
