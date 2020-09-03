using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class CallStoredProcedure : ICallStoredProcedure
    {
        private readonly ApplicationDbContext _db;

        public static string ConnectionString = string.Empty;
        public CallStoredProcedure(ApplicationDbContext db)
        {
            _db = db;

            ConnectionString = _db.Database.GetDbConnection().ConnectionString;
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public IEnumerable<T> GetList<T>(string procedureName, DynamicParameters param = null)
        {
            using var sqlCon = new SqlConnection(connectionString:ConnectionString);

            sqlCon.Open();

            return sqlCon.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
        }
    }
}
