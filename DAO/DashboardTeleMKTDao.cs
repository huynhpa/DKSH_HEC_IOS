using DKSHApplication.Common;
using DKSHApplication.Controllers.Dashboard;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class DashboardTeleMKTDao
    {
        private GenericRepository<TLMDashboardDto> dashboardRepo;

        private CCCToolsEntity cccToolsEntity = null;

        public DashboardTeleMKTDao()
        {
            dashboardRepo = new GenericRepository<TLMDashboardDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        public TLMDashboardDto DashboardTeleMKT(TLMDashboardParam dto)
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_GetAutoScheduleTasks]";
            return dashboardRepo.ExecuteQuerySingle(spQuery, parameter);
        }
    }
}