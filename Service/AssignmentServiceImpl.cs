using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using MP.Core.Library;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class AssignmentServiceImpl : AssignmentService
    {
        ServiceTypeDao serviceTypeDao = new ServiceTypeDao();
        HrEmployeeDao hrEmployeeDao = new HrEmployeeDao();
        PhoneNumberDao phoneNumberDao = new PhoneNumberDao();
        AssignDao assignDao = new AssignDao();

        public List<ProjectDto> GetProjectByWindowUser(ProjectDto project)
        {
            List<ProjectDto> result = new List<ProjectDto>();
            result.Add(project);
            return result;
        }

        /// <summary>
        /// Remove Assignment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto RemoveAssignment(AssignmentDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = phoneNumberDao.RemoveAssignment(dto);
                if (result.number > 0)
                {
                    result.code = Constant.DELETE;
                    result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch(SqlException sql)
            {
                result.code = Constant.ERROR;
                result.message = sql.Message;
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto SaveStatus(AssignDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (param.ID != Guid.Empty)
                {
                    if (param.CallBack == true)
                    {
                        param.Status = "99";
                    }
                    result.number = assignDao.SaveStatus(param);
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
                    result.message = Constant.PHONE_NUMBER_NOT_EXISTED;
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
        /// View Assignment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AssignmentDto> ViewAssignment(AssignmentDto dto)
        {
            IEnumerable<AssignmentDto> result = serviceTypeDao.ViewAssignment(dto);
            return result;
        }


        
    }
}