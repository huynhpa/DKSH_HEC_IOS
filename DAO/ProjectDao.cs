using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class ProjectDao
    {
        private GenericRepository<ProjectDto> projectRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// init
        /// </summary>
        public ProjectDao()
        {
            projectRepo = new GenericRepository<ProjectDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Search Project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ProjectDto> SearchProject(ProjectDto dto)
        {
            object[] parameter = { dto.ProjectName, dto.IsActive };
            string spQuery = @"EXEC [sp_SearchProject] {0}, {1}";
            return projectRepo.ExecuteQueryOBS(spQuery, parameter);
        }
        

        /// <summary>
        /// Insert Call Plan Result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertProject(ProjectDto dto)
        {
            object[] parameter = { dto.ID, dto.ProjectName, dto.CreatedBy, dto.UpdatedBy, dto.IsActive, dto.Header, dto.Footer };
            string spQuery = @"EXEC [sp_InsertProject] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return projectRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// GetProjectDetail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ProjectDto GetProjectDetail(ProjectDto dto)
        {
            object[] parameter = { dto.ID };
            string spQuery = @"EXEC [sp_GetProjectDetail] {0}";
            return projectRepo.ExecuteQuerySingleOBS(spQuery, parameter);
        }

        /// <summary>
        /// DeleteProject
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteProject(ProjectDto dto)
        {
            object[] parameter = { dto.ID, dto.UpdatedBy };
            string spQuery = @"EXEC [sp_DeleteProject] {0}, {1}";
            return projectRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Update survey status call plan result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateProject(ProjectDto dto)
        {
            object[] parameter = { dto.ID, dto.ProjectName, dto.IsActive,
                                    dto.UpdatedBy, dto.Header, dto.Footer };
            string spQuery = @"EXEC [sp_UpdateProject] {0}, {1}, {2}, {3}, {4}, {5}";
            return projectRepo.ExecuteCommandOBS(spQuery, parameter);
        }
    }
}