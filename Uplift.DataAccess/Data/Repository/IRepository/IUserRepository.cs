using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void LockUser(string userId);

        void UnlockUser(string userId);
    }
}
