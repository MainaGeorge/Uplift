using System;
using System.Collections.Generic;
using Dapper;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface ICallStoredProcedure : IDisposable
    {
        IEnumerable<T> GetList<T>(string procedureName, DynamicParameters param = null);
    }
}
