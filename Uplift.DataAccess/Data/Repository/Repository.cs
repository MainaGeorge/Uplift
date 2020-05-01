using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly DbContext Context;
        internal DbSet<T> DbSet;


        public Repository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }
        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(int id)
        {
            var itemToRemove = Get(id);
            Remove(itemToRemove);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includedProperties = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includedProperties != null)
            {
                query = includedProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, property) => current.Include(property));
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includedProperties = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includedProperties != null)
            {
                query = includedProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, property) => current.Include(property));
            }

            return query.FirstOrDefault();
        }
    }
}
