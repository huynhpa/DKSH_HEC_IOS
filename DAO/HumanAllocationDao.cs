using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class HumanAllocationDao
    {
        private GenericRepository<AgentsDto> agentsRepo;

        private GenericRepository<HumanAllocationDto> humanAllocationRepo;

        private GenericRepository<LogAgentJobDto> logAgentJobRepo;

        private GenericRepository<AgentJobsDto> agentJobRepo;

        private GenericRepository<ServiceTypeDto> serviceTypeRepo;

        public CCCToolsEntity cccToolsEntity = null;
        public HumanAllocationDao()
        {
            agentsRepo = new GenericRepository<AgentsDto>(new CCCToolsEntity());
            humanAllocationRepo = new GenericRepository<HumanAllocationDto>(new CCCToolsEntity());
            logAgentJobRepo = new GenericRepository<LogAgentJobDto>(new CCCToolsEntity());
            agentJobRepo = new GenericRepository<AgentJobsDto>(new CCCToolsEntity());
            serviceTypeRepo = new GenericRepository<ServiceTypeDto>(new OBSEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Get log agent job detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public LogAgentJobDto GetAgentJobDetail(LogAgentJobDto param)
        {
            object[] pr = { param.Id, param.AgentId };
            string spQuery = "EXEC [sp_GetAgentJobDetail] {0}, {1}";
            return logAgentJobRepo.ExecuteQuerySingle(spQuery, pr);
        }

        /// <summary>
        /// Get Campaign List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HumanAllocationDto> GetCampaignList()
        {
            object[] param = { };
            string spQuery = "EXEC [sp_GetCampaignList]";
            return humanAllocationRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AgentsDto> GetAgentList()
        {
            object[] param = { };
            string spQuery = "EXEC [sp_GetAgentList]";
            return agentsRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Human Allocation Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public HumanAllocationDto GetHumanAllocationDetail(HumanAllocationDto dto)
        {
            object[] param = { dto.Id, dto.AgentId, dto.CampaignId };
            string spQuery = "EXEC [sp_GetHumanAllocationDetail] {0}, {1}, {2}";
            return humanAllocationRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get Human Allocation List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<HumanAllocationDto> SearchHumanAllocation(HumanAllocationDto dto)
        {
            object[] param = { dto.LoginDate, dto.AgentId, dto.CampaignId };
            string spQuery = "EXEC [sp_GetHumanAllocation] {0}, {1}, {2}";
            return humanAllocationRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// LogIn Call Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int LogInCallTime(HumanAllocationDto dto)
        {
            object[] param = { dto.Id, Utils.ConvertTime(dto.SignInTime), Utils.StringDateFormat(dto.LoginDate),
                               dto.AgentId, dto.CampaignId, dto.CampaignName,
                               dto.CreateBy, dto.UpdateBy, dto.LoginStatus };
            string spQuery = "EXEC [sp_LogInCallTime] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return humanAllocationRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// LogIn Call Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int LogOutCallTime(HumanAllocationDto dto)
        {
            object[] param = { dto.Id, dto.AgentId, dto.CampaignId, Utils.ConvertTime(dto.SignInTime), Utils.ConvertTime(dto.SignOutTime),
                              Utils.StringDateFormat(dto.LoginDate) ,Utils.StringDateFormat(dto.LogoutDate),
                               dto.UpdateBy, dto.LogoutStatus };
            string spQuery = "EXEC [sp_LogOutCallTime] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return humanAllocationRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Login And Log Out
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateLoginCallTime(HumanAllocationDto dto)
        {
            object[] param = { dto.Id, Utils.ConvertTime(dto.SignInTime), Utils.StringDateFormat(dto.LoginDate),
                               dto.CallInHours, dto.CallInMinutes, dto.UpdateBy };
            string spQuery = "EXEC [sp_UpdateLoginCallTime] {0}, {1}, {2}, {3}, {4}, {5}";
            return humanAllocationRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Search Log Agent Job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<LogAgentJobDto> SearchLogAgentJob(LogAgentJobDto param)
        {
            object[] pr = { param.SignInDate, param.AgentId };
            string spQuery = "EXEC [sp_SearchLogAgentJob] {0}, {1}";
            return logAgentJobRepo.ExecuteQuery(spQuery, pr);
        }

        /// <summary>
        /// Update Login And Log Out
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateLogoutCallTime(HumanAllocationDto dto)
        {
            object[] param = { dto.Id, Utils.ConvertTime(dto.SignOutTime), Utils.StringDateFormat(dto.LogoutDate),
                               dto.CallInHours, dto.CallInMinutes, dto.UpdateBy };
            string spQuery = "EXEC [sp_UpdateLogoutCallTime] {0}, {1}, {2}, {3}, {4}, {5}";
            return humanAllocationRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Get Login Logout By Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public HumanAllocationDto GetLoginOrLogoutByAgent(HumanAllocationDto dto)
        {
            object[] param = { dto.AgentId, dto.CampaignId, dto.CreateDate };
            string spQuery = "EXEC [sp_GetLoginAndLogoutByAgent] {0}, {1}, {2}";
            return humanAllocationRepo.ExecuteQuerySingle(spQuery, param);
        }

        /*====================================================================*/

        /// <summary>
        /// Get Log Agent Job
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<LogAgentJobDto> GetLogAgentJob(LogAgentJobDto dto)
        {
            object[] param = { dto.CreateDate, dto.JobCode, dto.AgentId };
            string spQuery = "EXEC [sp_GetLogAgentJob] {0}, {1}, {2}";
            return logAgentJobRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AgentJobsDto> GetAgentJob()
        {
            object[] param = { };
            string spQuery = "EXEC [sp_GetAgentJob]";
            return agentJobRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Insert Log Agent Job
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertLogAgentJob(LogAgentJobDto dto)
        {
            object[] param = {  dto.Id, dto.JobCode, Utils.ConvertTime(dto.SignInTime), Utils.StringDateFormat(dto.SignInDate),
                                dto.CreateBy, dto.UpdateBy, dto.Remark, dto.AgentId };
            string spQuery = "EXEC [sp_InsertLogAgentJob] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return logAgentJobRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Log Agent Job
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>integer</returns>
        public int UpdateLogAgentJob(LogAgentJobDto dto)
        {
            object[] param = { dto.Id, dto.JobCode, Utils.ConvertTime(dto.SignInTime), Utils.ConvertTime(dto.SignOutTime),
                                Utils.StringDateFormat(dto.SignInDate), Utils.StringDateFormat(dto.SignOutDate),
                                dto.UpdateBy, dto.Remark, dto.AgentId };
            string spQuery = "EXEC [sp_UpdateLogAgentJob] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return logAgentJobRepo.ExecuteCommand(spQuery, param);
        }
    }
}