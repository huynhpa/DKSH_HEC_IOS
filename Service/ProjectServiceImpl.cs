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
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class ProjectServiceImpl : ProjectService
    {
        ProjectDao projectDao = new ProjectDao();

        public ResultMessageDto DeleteProject(ProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = projectDao.DeleteProject(dto);
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
        /// GetProjectDetail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ProjectDto GetProjectDetail(ProjectDto dto)
        {
            ProjectDto project = projectDao.GetProjectDetail(dto);
            return project;
        }

        /// <summary>
        /// insert project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertProject(ProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                dto.ID = Guid.NewGuid();
                result.number = projectDao.InsertProject(dto);
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
        /// Search Project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ProjectDto> SearchProject(ProjectDto dto)
        {
            IEnumerable<ProjectDto> result = projectDao.SearchProject(dto);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateProject(ProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = projectDao.UpdateProject(dto);
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
    }
}