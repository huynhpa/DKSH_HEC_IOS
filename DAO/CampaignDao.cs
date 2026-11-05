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
    public class CampaignDao
    {
        public GenericRepository<ProjectDto> projectRepo;
        public GenericRepository<ServiceTypeDto> ServiceTypeRepo;
        private GenericRepository<SearchCallDto> searchcallRepo;
        public OBSEntity obsEntity;
        public CampaignDao()
        {
            obsEntity = new OBSEntity();
            projectRepo = new GenericRepository<ProjectDto>(new OBSEntity());
            ServiceTypeRepo = new GenericRepository<ServiceTypeDto>(new OBSEntity());
            searchcallRepo = new GenericRepository<SearchCallDto>(new OBSEntity());
        }

        /// <summary>
        /// Campaign Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ServiceTypeDto CampaignDetail(ServiceTypeDto dto)
        {
            object[] param = { dto.ServiceCode, dto.ProjectID };
            string spQuery = @"EXEC sp_CampaignDetail {0},{1}";
            return ServiceTypeRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Delete Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteCampaign(ServiceTypeDto dto)
        {
            object[] param = { dto.ID, dto.UpdatedBy };
            string spQuery = @"EXEC sp_DeleteCampaign {0}, {1}";
            return ServiceTypeRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// ProjectActive
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ProjectDto> ProjectActive()
        {
            object[] param = { };
            string spQuery = @"EXEC sp_ProjectActive";
            return projectRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// SearchCampaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ServiceTypeDto> SearchCampaign(CampaignDto dto)
        {
            object[] param = { dto.ProjectID, dto.ServiceName, dto.Status };
            string spQuery = @"EXEC sp_SearchCampaign {0}, {1}, {2}";
            return ServiceTypeRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Update Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateCampaign(ServiceTypeDto dto)
        {
            object[] param = { dto.ID, dto.ServiceName, dto.StartAlert, dto.EndAlert,
                                dto.IsActive, dto.CreatedBy, dto.UpdatedBy, dto.ProjectID,
                                Utils.StringDateFormat(dto.StartDate), Utils.StringDateFormat(dto.EndDate),
                                dto.ServiceCode };
            string spQuery = @"EXEC sp_UpdateCampaign {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}";
            return ServiceTypeRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Insert Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertCampaign(ServiceTypeDto dto)
        {
            object[] param = { dto.ID, dto.ServiceName, dto.StartAlert, dto.EndAlert,
                                dto.IsActive, dto.CreatedBy, dto.UpdatedBy, dto.ProjectID,
                                Utils.StringDateFormat(dto.StartDate), Utils.StringDateFormat(dto.EndDate) };
            string spQuery = @"EXEC sp_InsertCampaign {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
            return ServiceTypeRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// view agent in campaign
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> ViewAgentInCampaign(SearchCallDto json)
        {
            object[] parameter = { json.ServiceId };
            string spQuery = @"EXEC [sp_ViewAgentInCampaign] {0}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }
    }
}