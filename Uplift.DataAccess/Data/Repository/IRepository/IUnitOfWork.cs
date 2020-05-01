using System;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        ICategoryRepository Category { get; }

    }
}