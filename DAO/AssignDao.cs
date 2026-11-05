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
    public class AssignDao
    {
        private GenericRepository<AssignDto> assignRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// initialize
        /// </summary>
        public AssignDao()
        {
            assignRepo = new GenericRepository<AssignDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Insert Phone Number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateAssign(AssignDto dto)
        {
            object[] param = { dto.ID, dto.AgentID, dto.Status };
            string procQuery = @"EXEC sp_UpdateAssign {0}, {1}, {2}";
            return assignRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// Get Assign Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal AssignDto GetAssignDetail(PhoneNumberDto dto)
        {
            object[] param = { dto.ID };
            string procQuery = @"EXEC sp_GetAssignDetail {0}";
            return assignRepo.ExecuteQuerySingleOBS(procQuery, param);
        }

        /// <summary>
        /// Insert Assign
        /// </summary>
        /// <param name="assignDto"></param>
        /// <returns></returns>
        internal int InsertAssign(AssignDto assignDto)
        {
            object[] param = { assignDto.ID, assignDto.PhoneNumberID, assignDto.AgentID, assignDto.Status, assignDto.CreatedBy };
            string procQuery = @"EXEC sp_InsertAssign {0}, {1}, {2}, {3}, {4}";
            return assignRepo.ExecuteCommandOBS(procQuery, param);
        }

        /// <summary>
        /// Save Status
        /// </summary>
        /// <param name="assignDto"></param>
        /// <returns></returns>
        internal int SaveStatus(AssignDto assignDto)
        {
            object[] param = { assignDto.ID, assignDto.AgentID, assignDto.Status,
                               Utils.StringDateFormatHHMMSS(assignDto.AppointmentDate) };
            string procQuery = @"EXEC sp_UpdateAssignStatus {0}, {1}, {2}, {3}";
            return assignRepo.ExecuteCommandOBS(procQuery, param);
        }
    }
}