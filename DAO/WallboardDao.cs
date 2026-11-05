using DKSHApplication.Common;
using DKSHApplication.DAOBusiness;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class WallboardDao
    {
        private GenericRepository<DailyCallDto> DailyCallRepo;
        private GenericRepository<AchivedPerformanceDto> AchivedPerformanceRepo;
        private GenericRepository<FocusCampaignDto> FocusCampaignRepo;
        private OBSEntity obsEntity = null;

        public WallboardDao()
        {
            DailyCallRepo = new GenericRepository<DailyCallDto>(new OBSEntity());
            AchivedPerformanceRepo = new GenericRepository<AchivedPerformanceDto>(new OBSEntity());
            FocusCampaignRepo = new GenericRepository<FocusCampaignDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Save Daily Call Performance
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int SaveDailyCallPerformance(DailyCallDto dto)
        {
            object[] param = { };
            string query = "EXEC sp_SaveDailyCallPerformance";
            return DailyCallRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// VASCampaign
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<FocusCampaignDto> VASCampaign()
        {
            object[] param = { };
            string query = "EXEC sp_VASCampaign";
            return FocusCampaignRepo.ExecuteQueryOBS(query, param);
        }


        /// <summary>
        /// DailyCallMTDSalePerformance
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<AchivedPerformanceDto> DailyCallMTDSalePerformance()
        {
            object[] param = { };
            string query = "EXEC sp_DailyCallMTDSalePerformance";
            return AchivedPerformanceRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// DeleteDailyCallPerformance
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int DeleteDailyCallPerformance()
        {
            object[] param = { };
            string query = "EXEC sp_DeleteDailyCallPerformance";
            return DailyCallRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// DeleteAchivedPerformance
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int DeleteAchivedPerformance()
        {
            object[] param = { };
            string query = "EXEC sp_DeleteDailyCallPerformance";
            return DailyCallRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// Daily Call Tele
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DailyCallDto DailyCallTele(DailyCallDto dto)
        {
            object[] param = { dto.TeleTeam.ToUpper() };
            string query = "EXEC sp_DailyCallTele {0}";
            return DailyCallRepo.ExecuteQuerySingleOBS(query, param);
        }

        /// <summary>
        /// VASCampaign
        /// </summary>
        /// <returns></returns>
        internal int SaveVASCampaign()
        {
            object[] param = { };
            string query = "EXEC sp_SaveCallsVASCampaign";
            return FocusCampaignRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// SaveDailyCallMTDSalePerformance
        /// </summary>
        /// <returns></returns>
        internal int SaveDailyCallMTDSalePerformance()
        {
            object[] param = { };
            string query = "EXEC sp_SaveDailyCallMTDSalePerformance";
            return AchivedPerformanceRepo.ExecuteCommandOBS(query, param);
        }
    }
}