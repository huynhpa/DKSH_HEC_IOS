using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.DB;
using DKSHApplication.Common;

namespace DKSHApplication.DAO
{
    public class AgentDao
    {
        private GenericRepository<UsersDto> UserRepo;
        OBSEntity obsEntity = null;
        public AgentDao()
        {
            UserRepo = new GenericRepository<UsersDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertAgent(UsersDto dto)
        {
            object[] param = { dto.UserId, dto.UserName };
            string spQuery = @"EXEC sp_InsertUser {0}, {1}";
            return UserRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Get Agent Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal UsersDto GetAgentDetail(UsersDto dto)
        {
            object[] param = { dto.UserId };
            string spQuery = @"EXEC [sp_GetAgentDetail] {0}";
            return UserRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Check UserName Existed
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<UsersDto> CheckUserExisted(UsersDto dto)
        {
            object[] param = { dto.UserName, dto.AgentNumber };
            string spQuery = @"EXEC [sp_CheckUserExisted] {0}, {1}";
            return UserRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// AssignAgentProject
        /// </summary>
        /// <param name="agentDto"></param>
        /// <returns></returns>
        internal int AssignAgentProject(AgentDto agentDto)
        {
            object[] param = { agentDto.Extension, agentDto.ProjectCode, agentDto.UpdatedBy };
            string spQuery = @"EXEC sp_AssignAgentProject {0}, {1}, {2}";
            return UserRepo.ExecuteCommandOBS(spQuery, param);
        }
    }
}