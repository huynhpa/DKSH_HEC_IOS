using DKSHApplication.Common;
using DKSHApplication.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CustGroupCodeDao
    {
        private GenericRepository<CustGroupCode> custGroupRepository;
        private CCCToolsEntity cccToolsEntity = null;
        public CustGroupCodeDao()
        {
            custGroupRepository = new GenericRepository<CustGroupCode>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Get CustGroup
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustGroupCode> GetCustGroup()
        {
            object[] param = { };
            string spQuery = @"[GetCustGroup]";
            return custGroupRepository.ExecuteQuery(spQuery, param);
        }
    }
}