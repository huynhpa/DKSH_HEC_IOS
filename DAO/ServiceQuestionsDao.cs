using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;

namespace DKSHApplication.DAO
{
    public class ServiceQuestionsDao
    {
        private GenericRepository<ServiceQuestionsDto> serviceQuestionRepo;
        OBSEntity obsEntity = null;
        public ServiceQuestionsDao()
        {
            obsEntity = new OBSEntity();
            serviceQuestionRepo = new GenericRepository<ServiceQuestionsDto>(new OBSEntity());
        }

        /// <summary>
        /// GetServiceQuestionsByServiceType
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ServiceQuestionsDto> GetServiceQuestionsByServiceType(OBSurveyParam dto)
        {
            object[] param = { dto.ServiceTypeId };
            string spQuery = @"EXEC sp_GetServiceQuestionsByServiceType {0}";
            return serviceQuestionRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// GetByID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ServiceQuestionsDto GetByID(ServiceQuestionsDto dto)
        {
            object[] param = { dto.ID };
            string spQuery = @"EXEC sp_GetServiceQuestionsByID {0}";
            return serviceQuestionRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// GetListQuestion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ServiceQuestionsDto> GetListQuestion(OBSurveyParam dto)
        {
            object[] param = { dto.ServiceTypeId };
            string spQuery = @"EXEC GetListQuestion {0}";
            return serviceQuestionRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Insert ServiceQuestions
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertServiceQuestions(ServiceQuestionsDto dto)
        {
            object[] param = { dto.ID, dto.ServiceTypeID, dto.QuestionID, dto.OrderIndex, dto.CreatedBy, dto.UpdatedBy, dto.IsDeleted };
            string spQuery = @"EXEC sp_InsertServiceQuestions {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return serviceQuestionRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Update ServiceQuestions
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateServiceQuestions(ServiceQuestionsDto dto)
        {
            object[] param = { dto.ID, dto.ServiceTypeID, dto.QuestionID, dto.OrderIndex, dto.UpdatedBy, dto.IsDeleted };
            string spQuery = @"EXEC sp_UpdateServiceQuestions {0}, {1}, {2}, {3}, {4}, {5}";
            return serviceQuestionRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Get ServiceQuestions By QuestionID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal ServiceQuestionsDto GetByQuestionID(ServiceQuestionsDto dto)
        {
            object[] param = { dto.QuestionID };
            string spQuery = @"EXEC sp_GetByQuestionID {0}";
            return serviceQuestionRepo.ExecuteQuerySingleOBS(spQuery, param);
        }
    }
}