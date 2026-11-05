using DKSHApplication.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace DKSHApplication.Common
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        CCCToolsEntity cccContext = null;
        OBSEntity contextOBS = null;
        iMindEntity contextIMind = null;
        CCCReportEntity cccReportContext = null;
        InHouseEntity inHouseContext = null;
        private DbSet<T> entities = null;

        public GenericRepository(CCCToolsEntity cccContext)
        {
            this.cccContext = cccContext;
            entities = cccContext.Set<T>();
        }

        public GenericRepository(OBSEntity contextOBS)
        {
            this.contextOBS = contextOBS;
            entities = contextOBS.Set<T>();
        }

        public GenericRepository(iMindEntity contextIMind)
        {
            this.contextIMind = contextIMind;
            entities = contextIMind.Set<T>();
        }

        public GenericRepository(CCCReportEntity cccReportContext)
        {
            this.cccReportContext = cccReportContext;
            entities = cccReportContext.Set<T>();
        }

        public GenericRepository(InHouseEntity inHouseContext)
        {
            this.inHouseContext = inHouseContext;
            entities = inHouseContext.Set<T>();
        }

        /// <summary>
        /// Get Data From Database CDI
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<T> ExecuteQuery(string spQuery, object[] parameters)
        {
            try
            {
                using (cccContext = new CCCToolsEntity())
                {
                    cccContext.Database.CommandTimeout = 1000;
                    return cccContext.Database.SqlQuery<T>(spQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Data From Database OBS
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<T> ExecuteQueryOBS(string spQuery, object[] parameters)
        {
            try
            {
                using (contextOBS = new OBSEntity())
                {
                    contextOBS.Database.CommandTimeout = 1000;
                    return contextOBS.Database.SqlQuery<T>(spQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        /// <summary>
        /// IMIND
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteQueryiMind(string spQuery, object[] parameters)
        {
            try
            {
                using (contextIMind = new iMindEntity())
                {
                    return contextIMind.Database.SqlQuery<T>(spQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Single Data From Database CCC
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public T ExecuteQuerySingle(string spQuery, object[] parameters)
        {
            try
            {
                using (cccContext = new CCCToolsEntity())
                {
                    return cccContext.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteQuerySingle OBS
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteQuerySingleOBS(string spQuery, object[] parameters)
        {
            try
            {
                using (contextOBS = new OBSEntity())
                {
                    return contextOBS.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Select count from table
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns>integer</returns>
        public int ExecuteQueryCountOBS(string spQuery, object[] parameters)
        {
            try
            {
                using (contextOBS = new OBSEntity())
                {
                    return contextOBS.Database.SqlQuery<T>(spQuery, parameters).Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executive command query single iMind dksh_db
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteQuerySingleIMind(string spQuery, object[] parameters)
        {
            try
            {
                using (contextIMind = new iMindEntity())
                {
                    return contextIMind.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Insert/Update/Delete Data To Database CCC Tools
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public int ExecuteCommand(string spQuery, object[] parameters)
        {
            int result = 0;
            try
            {
                using (cccContext = new CCCToolsEntity())
                {
                    result = cccContext.Database.ExecuteSqlCommand(spQuery, parameters);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database OB
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommandOBS(string spQuery, object[] parameters)
        {
            int result = 0;
            try
            {
                using (contextOBS = new OBSEntity())
                {
                    contextOBS.Database.CommandTimeout = 5000;
                    result = contextOBS.Database.ExecuteSqlCommand(spQuery, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string spQuery, SqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                using (SqlConnection cnn = new SqlConnection(Constant.CONNECTION_CDI))
                {
                    if (cnn.State != ConnectionState.Open)
                    {
                        cnn.Open();
                    }
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = spQuery;
                    foreach (SqlParameter parm in parameters)
                    {
                        cmd.Parameters.Add(parm);
                    }
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 10.210.148.48
        /// CCCReport
        /// userid & password: reportuser
        /// Return IEnumerable<Object>
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteQueryCCCReport(string spQuery, object[] parameters)
        {
            try
            {
                using (cccReportContext = new CCCReportEntity())
                {
                    return cccReportContext.Database.SqlQuery<T>(spQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 10.210.148.48
        /// CCCReport
        /// userid & password: reportuser
        /// Return Object
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteQuerySingleCCCReport(string spQuery, object[] parameters)
        {
            try
            {
                using (cccReportContext = new CCCReportEntity())
                {
                    return cccReportContext.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database CCCReport 10.210.148.48
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public int ExecuteCommandCCCReport(string spQuery, object[] parameters)
        {
            int result = 0;
            try
            {
                using (cccReportContext = new CCCReportEntity())
                {
                    result = cccReportContext.Database.ExecuteSqlCommand(spQuery, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 10.210.128.36
        /// DB IMIND
        /// userid & password: iminduser
        /// Return IEnumerable<Object>
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteQueryInHouse(string spQuery, object[] parameters)
        {
            try
            {
                using (inHouseContext = new InHouseEntity())
                {
                    return inHouseContext.Database.SqlQuery<T>(spQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 10.210.128.36
        /// DB IMIND
        /// userid & password: iminduser
        /// Return Object
        /// </summary>
        /// <param name="spQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteQuerySingleInHouse(string spQuery, object[] parameters)
        {
            try
            {
                using (inHouseContext = new InHouseEntity())
                {
                    return inHouseContext.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database CCCReport 10.210.148.48
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public int ExecuteCommandInHouse(string spQuery, object[] parameters)
        {
            int result = 0;
            try
            {
                using (inHouseContext = new InHouseEntity())
                {
                    result = inHouseContext.Database.ExecuteSqlCommand(spQuery, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    cccContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        
    }
}