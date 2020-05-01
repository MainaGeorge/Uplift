using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includedProperties = null
            );
    }
}
