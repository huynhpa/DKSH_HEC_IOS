using DKSHApplication.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.Common;
using MP.Core.Library;

namespace DKSHApplication.DAO
{
    public class MonitorDao
    {
        private GenericRepository<MonitorHistoryDto> MonitorHistoryRepo;
        private GenericRepository<EvaluationMarkDto> EvalMarkRepo;
        private GenericRepository<AgentByLeaderDto> AgentByLeaderRepo;
        private GenericRepository<MonitoringFormDto> MonitorFormRepo;
        private GenericRepository<CriterialEvaluationCallDto> CriterialEvalRepo;
        private GenericRepository<MonitorDetailDto> MonitorDetailRepo;
        private GenericRepository<Records46Dto> RecordRepo;
        private GenericRepository<MonitorLogDto> MonitorLogRepo;
        private OBSEntity obsEntity = null;
        public MonitorDao()
        {
            obsEntity = new OBSEntity();
            MonitorHistoryRepo = new GenericRepository<MonitorHistoryDto>(new OBSEntity());
            EvalMarkRepo = new GenericRepository<EvaluationMarkDto>(new OBSEntity());
            AgentByLeaderRepo = new GenericRepository<AgentByLeaderDto>(new OBSEntity());
            MonitorFormRepo = new GenericRepository<MonitoringFormDto>(new OBSEntity());
            CriterialEvalRepo = new GenericRepository<CriterialEvaluationCallDto>(new OBSEntity());
            MonitorDetailRepo = new GenericRepository<MonitorDetailDto>(new OBSEntity());
            RecordRepo = new GenericRepository<Records46Dto>(new OBSEntity());
            MonitorLogRepo = new GenericRepository<MonitorLogDto>(new OBSEntity());
        }

        /// <summary>
        /// Get Agent By Leader
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AgentByLeaderDto> GetAgentByLeader(MonitorHistoryDto dto)
        {
            object[] para = { dto.LeaderExtension };
            string query = @"EXEC sp_GetAgentByLeader {0}";
            return AgentByLeaderRepo.ExecuteQueryOBS(query, para);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<AgentByLeaderDto> GetLeaderList()
        {
            object[] para = { };
            string query = @"EXEC sp_SelectLeaders";
            return AgentByLeaderRepo.ExecuteQueryOBS(query, para);
        }

        /// <summary>
        /// Evaluation Mark
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<EvaluationMarkDto> EvaluationMark()
        {
            object[] para = { };
            string query = @"EXEC sp_EvaluationMark";
            return EvalMarkRepo.ExecuteQueryOBS(query, para);
        }

        
        /// <summary>
        /// save evaluation call
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveEvaluationCall(MonitorHistoryDto dto)
        {
            object[] para = { dto.MonitorID, dto.MonitorBy, dto.CreateBy, dto.uniqueid, 
                            dto.TotalMark, dto.IsUpdate, dto.UpdateBy, dto.Campaign, dto.RecordID,
                            dto.duration, dto.destination, dto.calldate, dto.cnum };
            string query = @"EXEC sp_SaveEvaluationCall {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}";
            return MonitorHistoryRepo.ExecuteCommandOBS(query, para);
        }

        /// <summary>
        /// Save Evaluation Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveEvaluationDetail(MonitorHistoryDto dto)
        {
            object[] para = { FGuid.ToGuid(dto.MonitorID), dto.uniqueid, dto.RecordID,
                FGuid.ToGuid(dto.CriterialID), dto.CreateBy, dto.EvalID, dto.Note, dto.IsUpdate, dto.UpdateBy };
            string query = @"EXEC sp_SaveMonitorDetail {0},{1},{2},{3},{4},{5},{6},{7},{8}";
            return MonitorHistoryRepo.ExecuteCommandOBS(query, para);
        }

        /// <summary>
        /// Criterial Evaluation Call
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<CriterialEvaluationCallDto> CriterialEvaluationCall()
        {
            object[] para = { };
            string query = @"EXEC sp_CriterialEvaluationCall";
            return CriterialEvalRepo.ExecuteQueryOBS(query, para);
        }

        /// <summary>
        /// Get Evaluation Call
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal Records46Dto GetRecordInfoToMonitor(MonitorHistoryDto dto)
        {
            object[] para = { dto.RecordID };
            string query = @"EXEC sp_GetEvaluationCall {0}";
            return RecordRepo.ExecuteQuerySingleOBS(query, para);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveMonitorLog(MonitorLogDto dto)
        {
            object[] para = { dto.phone_number, dto.agent_extension, dto.call_date, dto.duration , dto.monitor_extension };
            string query = @"EXEC sp_SaveMonitorLog {0},{1},{2},{3},{4}";
            return MonitorLogRepo.ExecuteCommandOBS(query, para);
        }

        /// <summary>
        /// Get Monitor Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal MonitorHistoryDto GetMonitorDetail(MonitorHistoryDto dto)
        {
            object[] para = { dto.MonitorID };
            string query = @"EXEC sp_GetMonitorDetail {0}";
            return MonitorHistoryRepo.ExecuteQuerySingleOBS(query, para);
        }

        /// <summary>
        /// get total Item evaluation result
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal IEnumerable<MonitorDetailDto> GetTotalItemEvaluationResultByMonitorID(MonitorHistoryDto item)
        {
            object[] para = { item.MonitorID };
            string query = @"EXEC sp_GetEvaluationResultByMonitorID {0}";
            return MonitorDetailRepo.ExecuteQueryOBS(query, para);
        }


        /// <summary>
        /// Get Evaluation Result Detail
        /// </summary>
        /// <param name="MonitorHistoryDto"></param>
        /// <returns></returns>
        internal IEnumerable<MonitorDetailDto> GetEvaluationResultDetail(MonitorHistoryDto item)
        {
            object[] para = { item.MonitorID };
            string query = @"EXEC sp_GetEvaluationResultDetail {0}";
            return MonitorDetailRepo.ExecuteQueryOBS(query, para);
        }

        /// <summary>
        /// Search Monitor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<MonitorHistoryDto> SearchMonitorHistory(MonitorHistoryDto dto)
        {
            object[] para = { dto.LeaderExtension, dto.AgentExtension, Utils.StringDateFormat(dto.MonitorDateFrom), Utils.StringDateFormat(dto.MonitorDateTo) };
            string query = @"EXEC sp_SearchMonitorHistory {0},{1},{2},{3}";
            return MonitorHistoryRepo.ExecuteQueryOBS(query, para);
        }

        /// <summary>
        /// Search Monitor History Total
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<MonitorHistoryDto> SearchMonitorHistoryTotal(MonitorHistoryDto dto)
        {
            object[] para = { Utils.StringDateFormat(dto.MonitorDateFrom), Utils.StringDateFormat(dto.MonitorDateTo) };
            string query = @"EXEC sp_SearchMonitorHistoryTotal {0},{1}";
            return MonitorHistoryRepo.ExecuteQueryOBS(query, para);
        }


        /// <summary>
        /// Search Monitor History Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AgentByLeaderDto> SearchMonitorHistoryDetail(MonitorHistoryDto dto)
        {
            object[] para = { Utils.StringDateFormat(dto.MonitorDateFrom), Utils.StringDateFormat(dto.MonitorDateTo) };
            string query = @"EXEC sp_SearchMonitorHistoryDetail {0},{1}";
            return AgentByLeaderRepo.ExecuteQueryOBS(query, para);
        }

        /// <summary>
        /// update monitor average
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateMonitorAverage(MonitorHistoryDto dto)
        {
            object[] para = { dto.MonitorID };
            string query = @"EXEC sp_UpdateMonitorAverage {0}";
            return MonitorHistoryRepo.ExecuteCommandOBS(query, para);
        }

        /// <summary>
        /// delete monitor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteMonitor(MonitorHistoryDto dto)
        {
            object[] param = { dto.MonitorID };
            string query = "EXEC sp_DeleteMonitor {0}";
            return MonitorHistoryRepo.ExecuteCommandOBS(query, param);
        }
    }
}