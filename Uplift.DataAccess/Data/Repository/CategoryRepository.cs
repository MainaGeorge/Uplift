using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var catToUpdate = _db.Categories.FirstOrDefault(cat => cat.Id == category.Id);

            catToUpdate.Name = category.Name;
            catToUpdate.DisplayOrder = category.DisplayOrder;

            _db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetCategoriesForDropdown()
        {
            return _db.Categories.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()

            });
        }
    }
}