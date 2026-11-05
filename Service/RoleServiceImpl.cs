using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using MP.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class RoleServiceImpl : RoleService
    {
        RoleDao roleDao = new RoleDao();

        /// <summary>
        /// DelRole
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DelRole(RoleDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (CheckDeleteRole(dto) == 0)
                {
                    result.number = roleDao.DelRole(dto);
                    if (result.number > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.NO_ROW_SAVE;
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.ROLE_IN_USE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public RoleDto GetRoleDetail(RoleDto dto)
        {
            RoleDto rs = roleDao.GetRoleDetail(dto);
            return rs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDto> GetRoleList()
        {
            IEnumerable<RoleDto> rs = roleDao.GetAllRole();
            return rs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<RoleDto> GetUsersInRoles(RoleDto dto)
        {
            IEnumerable<RoleDto> result = roleDao.GetRoleByUser(dto);
            return result;
        }

        /// <summary>
        /// InsertRole
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertRole(RoleDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if(!CheckRoleExist(dto))
                {
                    dto.RoleId = Guid.NewGuid();
                    result.number = roleDao.InsertRole(dto);
                    if (result.number > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.NO_ROW_SAVE;
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.ROLE_NAME_EXISTED;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Search Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<RoleDto> SearchRole(RoleDto dto)
        {
            IEnumerable<RoleDto> result = roleDao.SearchRole(dto);
            return result;
        }

        /// <summary>
        /// UpdateRole
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateRole(RoleDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = roleDao.UpdateRole(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Check Delete Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private int CheckDeleteRole(RoleDto dto)
        {
            IEnumerable<RoleDto> flag = roleDao.CheckDeleteRole(dto);
            return flag.Count();
        }

        /// <summary>
        /// Check Role Exist
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private bool CheckRoleExist(RoleDto dto)
        {
            bool flag = false;
            IEnumerable<RoleDto> role = roleDao.CheckRoleExist(dto);
            if (role.Count() > 0)
            {
                flag = true;
            }
            return flag;
        }
    }
}