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
    public class SFEReportDao
    {
        private GenericRepository<SFEReportDto> SFEReportRepo;
        private GenericRepository<SFEReportConstDto> SFEReportConstRepo;
        private GenericRepository<SFEOrgchartDto> SFEOrgchartRepo;
        private GenericRepository<TargetCallTeleSaleDto> TargetCallTeleSaleRepo;

        private OBSEntity obsEntity = null;

        /// <summary>
        /// initialize
        /// </summary>
        public SFEReportDao()
        {
            SFEReportRepo = new GenericRepository<SFEReportDto>(new OBSEntity());
            SFEReportConstRepo = new GenericRepository<SFEReportConstDto>(new OBSEntity());
            SFEOrgchartRepo = new GenericRepository<SFEOrgchartDto>(new OBSEntity());
            TargetCallTeleSaleRepo = new GenericRepository<TargetCallTeleSaleDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Save SFEReport Const
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal int SaveSFEReportConst(SFEReportConstDto json)
        {
            object[] parameter = {  json.ConstCode
                                    ,json.ConstName
                                    ,json.MinTarget
                                    ,json.MaxTarget
                                    ,json.UpdateBy
                                    ,json.Note  };
            string spQuery = @"EXEC [sp_SaveSFEReportConst] {0}, {1}, {2}, {3}, {4}, {5}";
            return SFEReportConstRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// SFEOrgchart
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SFEOrgchartDto> SFEOrgchart(SFEOrgchartDto json)
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_SFEOrgchart]";
            return SFEOrgchartRepo.ExecuteQueryOBS(spQuery, parameter);
        }


        /// <summary>
        /// SFEPerformanceReport
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SFEReportDto> SFEPerformanceReport(SFEOrgchartDto dto)
        {
            object[] parameter = { dto.months, dto.PDACode };
            string spQuery = @"EXEC [sp_SFEPerformanceReport] {0},{1}";
            return SFEReportRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// SFEReportConst
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SFEReportConstDto> SFEReportConst(SFEReportConstDto json)
        {
            object[] parameter = {  json.ConstName };
            string spQuery = @"EXEC [sp_SFEReportConst] {0}";
            return SFEReportConstRepo.ExecuteQueryOBS(spQuery, parameter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal int SaveTargetCallTeleSale(TargetCallTeleSaleDto json)
        {
            object[] parameter = { json.sap_code, json.call_day, json.working_day, json.working_month, json.update_by };
            string spQuery = @"EXEC [sp_SaveTargetCallTeleSale] {0},{1},{2},{3},{4}";
            return TargetCallTeleSaleRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// DeleteTargetCallTeleSale
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal int DeleteTargetCallTeleSale(TargetCallTeleSaleDto json)
        {
            object[] parameter = { json.working_month };
            string spQuery = @"EXEC [sp_DeleteTargetCallTeleSale] {0}";
            return TargetCallTeleSaleRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal int SFEPerformanceProcess(SFEReportDto json)
        {
            object[] parameter = { json.create_by };
            string spQuery = @"EXEC [sp_SFEPerformanceProcess] {0}";
            return SFEReportRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal int UpdateTeleSalePerformance(SFEReportDto json)
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_UpdateTeleSalePerformance]";
            return SFEReportRepo.ExecuteCommandOBS(spQuery, parameter);
        }
    }
}