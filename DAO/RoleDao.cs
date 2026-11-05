using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using MP.Core.Library;
using System.Data;

namespace DKSHApplication.DAO
{
    public class RoleDao
    {
        private GenericRepository<RoleDto> RoleRepository;
        private OBSEntity obEntity = null;

        public RoleDao()
        {
            this.RoleRepository = new GenericRepository<RoleDto>(new CCCToolsEntity());
            obEntity = new OBSEntity();
        }

        internal int DelRole(RoleDto dto)
        {
            object[] param = { dto.RoleId };
            string spQuery = @"exec sp_DeleteRole {0}";
            return RoleRepository.ExecuteCommandOBS(spQuery, param);
        }

        // BEGIN ROLE OBS
        internal IEnumerable<RoleDto> GetRoleByUser(RoleDto dto)
        {
            object[] param = { dto.UserId };
            string spQuery = @"exec sp_GetRoleByUser {0}";
            return RoleRepository.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Search Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<RoleDto> SearchRole(RoleDto dto)
        {
            object[] param = { dto.RoleName };
            string spQuery = @"exec sp_SearchRole {0}";
            return RoleRepository.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Get Role Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal RoleDto GetRoleDetail(RoleDto dto)
        {
            object[] param = { dto.RoleId };
            string spQuery = @"exec sp_GetRoleDetail {0}";
            return RoleRepository.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<RoleDto> GetAllRole()
        {
            object[] param = { };
            string spQuery = @"exec sp_Roles";
            return RoleRepository.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// sp_InsertRole
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertRole(RoleDto dto)
        {
            object[] param = { dto.RoleId, dto.RoleName, dto.Description };
            string spQuery = @"exec sp_InsertRole {0},{1},{2}";
            return RoleRepository.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        internal int SetUserInRoles(RoleDto roleDto)
        {
            object[] param = { roleDto.UserId, roleDto.RoleId };
            string spQuery = @"exec sp_SetUserInRoles {0},{1}";
            return RoleRepository.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// DeleteUserInRole
        /// </summary>
        /// <param name="delRole"></param>
        /// <returns></returns>
        internal int DeleteUserInRole(RoleDto delRole)
        {
            object[] param = { delRole.UserId };
            string spQuery = @"exec sp_DeleteUserInRoles {0}";
            return RoleRepository.ExecuteCommandOBS(spQuery, param);            
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateRole(RoleDto dto)
        {
            object[] param = { dto.RoleId, dto.RoleName, dto.Description };
            string spQuery = @"exec sp_UpdateRole {0},{1},{2}";
            return RoleRepository.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Check Delete Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<RoleDto> CheckDeleteRole(RoleDto dto)
        {
            object[] param = { dto.RoleId };
            string spQuery = "EXEC sp_CheckDeleteRole {0}";
            return RoleRepository.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Check Role Exist
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<RoleDto> CheckRoleExist(RoleDto dto)
        {
            object[] param = { dto.RoleId, dto.RoleName };
            string spQuery = @"exec sp_RoleExists {0},{1}";
            return RoleRepository.ExecuteQueryOBS(spQuery, param);
        }
        // END ROLE OBS

    }
}