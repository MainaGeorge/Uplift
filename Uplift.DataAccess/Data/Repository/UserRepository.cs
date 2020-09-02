using System;
using System.Linq;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void LockUser(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();

        }

        public void UnlockUser(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.LockoutEnd = DateTime.Now;
            }

            _db.SaveChanges();
        }
    }
}
