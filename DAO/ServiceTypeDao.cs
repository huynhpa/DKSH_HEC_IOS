using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class ServiceTypeDao
    {
        private GenericRepository<ServiceTypeDto> serviceTypeRepo;
        private GenericRepository<AssignmentDto> assignmentRepo;
        private OBSEntity obsEntity = null;
        public ServiceTypeDao()
        {
            serviceTypeRepo = new GenericRepository<ServiceTypeDto>(new OBSEntity());
            assignmentRepo = new GenericRepository<AssignmentDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Get Service Type By ID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ServiceTypeDto GetServiceTypeByID(OBSurveyParam dto)
        {
            object[] param = { dto.ServiceTypeId };
            string spQuery = @"EXEC sp_GetServiceTypeByID {0}";
            return serviceTypeRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ServiceTypeDto GetServiceTypeByID(ServiceTypeDto dto)
        {
            object[] param = { dto.ID };
            string spQuery = @"EXEC sp_GetServiceTypeByID {0}";
            return serviceTypeRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// ViewAssignment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AssignmentDto> ViewAssignment(AssignmentDto dto)
        {
            object[] param = { };
            string spQuery = @"EXEC sp_ViewAssignment";
            return assignmentRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// GetNextServiceID
        /// </summary>
        /// <param name="serviceObj"></param>
        /// <returns></returns>
        internal string GetNextServiceID(ServiceTypeDto serviceObj)
        {
            string service = string.Empty;
            object[] param = { serviceObj.ServiceCode };
            string spQuery = "EXEC sp_GetNextServiceID {0}";
            ServiceTypeDto serviceType = serviceTypeRepo.ExecuteQuerySingleOBS(spQuery, param);
            if (serviceType != null)
            {
                service = serviceType.ServiceCode;
            }            
            return service;
        }

        /// <summary>
        /// Get Active Service Type
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ServiceTypeDto> GetActiveServiceType(ServiceTypeDto dto)
        {
            object[] param = { dto.ProjectID };
            string spQuery = @"EXEC sp_GetActiveServiceType {0}";
            return serviceTypeRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ServiceTypeDto> GetAllActiveServiceType(ServiceTypeDto dto)
        {
            object[] param = { };
            string spQuery = @"EXEC sp_GetAllActiveServiceType {0}";
            return serviceTypeRepo.ExecuteQueryOBS(spQuery, param);
        }

    }
}