using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DKSHApplication.Common
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> ExecuteQuery(string spQuery, object[] parameters);
        T ExecuteQuerySingle(string spQuery, object[] parameters);
        int ExecuteCommand(string spQuery, object[] parameters);
        int ExecuteNonQuery(string spQuery, SqlParameter[] parameters);
    }
}
