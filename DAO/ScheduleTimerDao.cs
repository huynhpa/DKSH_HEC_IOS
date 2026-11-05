using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class ScheduleTimerDao
    {
        private GenericRepository<ScheduleTimerDto> ScheduleTimerRepo;
        private GenericRepository<ShiftAgentsDto> ShiftAgentsRepo;
        private CCCToolsEntity cccToolsEntity = null;

        /// <summary>
        /// INITIALIZE
        /// </summary>
        public ScheduleTimerDao()
        {
            ScheduleTimerRepo = new GenericRepository<ScheduleTimerDto>(new CCCToolsEntity());
            ShiftAgentsRepo = new GenericRepository<ShiftAgentsDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        internal ShiftAgentsDto GetAgentInfo(ShiftAgentsDto agent)
        {
            object[] parameter = { agent.AgentId };
            string spQuery = @"EXEC [sp_GetAgentInfo] {0}";
            return ShiftAgentsRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// New Agents
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int NewAgents(ShiftAgentsDto dto)
        {
            object[] parameter = { dto.AgentId, dto.AgentName, dto.Groups, dto.AgentEmail, dto.IsActive, dto.IsBackUp,
                dto.IsUsed, Utils.StringDateFormat(dto.JoinDate), dto.Note, dto.Line, dto.CreateBy };
            string spQuery = @"EXEC [sp_NewAgents] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}";
            return ShiftAgentsRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Update Agents
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateAgents(ShiftAgentsDto dto)
        {
            object[] parameter = { dto.AgentId, dto.AgentName, dto.Groups, dto.AgentEmail, dto.IsActive, dto.IsBackUp,
                dto.IsUsed, Utils.StringDateFormat(dto.JoinDate), dto.Note, dto.Line, dto.Updateby };
            string spQuery = @"EXEC [sp_UpdateAgents] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}";
            return ShiftAgentsRepo.ExecuteCommand(spQuery, parameter);
        }

        /// <summary>
        /// Search Agents
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ShiftAgentsDto> SearchAgents(ShiftAgentsDto dto)
        {
            object[] parameter = { dto.AgentName, dto.Groups, dto.IsBackUp, dto.Line, dto.IsUsed };
            string spQuery = @"EXEC [sp_SearchAgents] {0}, {1}, {2}, {3}, {4}";
            return ShiftAgentsRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Check Agent Existed
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        internal ShiftAgentsDto CheckAgentExisted(ShiftAgentsDto agent)
        {
            object[] parameter = { agent.AgentName };
            string spQuery = @"EXEC [sp_CheckAgentExisted] {0}";
            return ShiftAgentsRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Generate Date
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int GenerateDate(ScheduleTimerDto dto)
        {
            object[] parameter = { dto.ShiftId, dto.AgentId };
            string spQuery = @"EXEC [sp_GenerateDate] {0}, {1}";
            return ShiftAgentsRepo.ExecuteCommand(spQuery, parameter);
        }
    }
}