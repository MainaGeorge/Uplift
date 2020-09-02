using System;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        ICategoryRepository Category { get; }

        IFrequencyRepository Frequency { get; }

        IServiceRepository Service { get; }

        IOrderHeaderRepository OrderHeader { get;  }

        IOrderDetailsRepository OrderDetails { get;  }

        IUserRepository User { get;  }

    }
}