using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.Common;
using DKSHApplication.DAOBusiness;
using DKSHApplication.DB;
using DKSHApplication.DTO;

namespace DKSHApplication.DAO
{
    public class HrEmployeeDao
    {
        private GenericRepository<hrEmployeeDto> hrEmployeeRepo;
        private GenericRepository<AgentGroupDto> AgentGroupRepo;
        OBSEntity obsEntity = null;
        public HrEmployeeDao()
        {
            hrEmployeeRepo = new GenericRepository<hrEmployeeDto>(new OBSEntity());
            AgentGroupRepo = new GenericRepository<AgentGroupDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Get Employee Detail
        /// </summary>
        /// <returns></returns>
        internal hrEmployeeDto GetEmployeeDetail(hrEmployeeDto dto)
        {
            object[] param = { dto.ID };
            string query = @"EXEC sp_GetEmployeeDetail {0}";
            return hrEmployeeRepo.ExecuteQuerySingleOBS(query, param);
        }

        /// <summary>
        /// Delete Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteAgent(hrEmployeeDto dto)
        {
            object[] param = { dto.ID };
            string query = @"EXEC sp_DeleteAgent {0}";
            return hrEmployeeRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// Get Employee By User Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal hrEmployeeDto GetEmployeeByUserName(hrEmployeeDto dto)
        {
            object[] param = { dto.WindowsUser };
            string query = @"EXEC sp_GetEmployeeByUserName {0}";
            return hrEmployeeRepo.ExecuteQuerySingleOBS(query, param);
        }

        /// <summary>
        /// Insert Hr Employee
        /// </summary>
        /// <param name="hrEmpDto"></param>
        /// <returns></returns>
        internal int InsertHrEmployee(hrEmployeeDto hrEmpDto)
        {
            object[] param = { hrEmpDto.ID, hrEmpDto.EmployeeID, hrEmpDto.WindowsUser,
                            hrEmpDto.AgentNumber, hrEmpDto.FullName, hrEmpDto.LastName, 
                            hrEmpDto.GroupID, hrEmpDto.GroupCode, hrEmpDto.contracts, hrEmpDto.SFECode };
            string query = @"EXEC sp_InsertHrEmployee {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
            return hrEmployeeRepo.ExecuteCommandOBS(query, param); 
        }

        /// <summary>
        /// UpdateAgent to table hrEmployee
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateAgent(hrEmployeeDto dto)
        {
            object[] param = { dto.ID, dto.EmployeeID, dto.WindowsUser, dto.AgentNumber,
                dto.FullName, dto.LastName, dto.GroupID, dto.GroupCode, dto.contracts, dto.IsApproved, dto.IsLockedOut, dto.SFECode };
            string query = @"EXEC sp_UpdateUser {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}";
            return hrEmployeeRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// agent group
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AgentGroupDto> AgentGroup(AgentGroupDto dto)
        {
            var query = from u in obsEntity.AgentGroup
                        where u.IsDelete == dto.IsDelete
                        select new AgentGroupDto()
                        {
                            GroupCode = u.GroupCode,
                            GroupName = u.GroupName,
                            IsDelete = u.IsDelete
                        };
            return query.OrderBy(x => x.GroupCode).ToList();
        }

    }
}