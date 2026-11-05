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
    public class QuestionDao
    {
        private GenericRepository<QuestionDto> questionRepo;
        private GenericRepository<AnswerTypeDto> answerTypeRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// INIT
        /// </summary>
        public QuestionDao()
        {
            questionRepo = new GenericRepository<QuestionDto>(new OBSEntity());
            answerTypeRepo = new GenericRepository<AnswerTypeDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Load Service Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal IEnumerable<QuestionDto> LoadServiceDetail(OBSurveyParam param)
        {
            object[] parameter = { param.ServiceId };
            string spQuery = @"EXEC [sp_LoadServiceDetail] {0}";
            return questionRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Insert Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertQuestion(QuestionDto dto)
        {
            object[] parameter = { dto.ID, dto.QuestionText, dto.QuestionCode, dto.QuestionTextNonHTML,
                                    dto.QuestionTypeCode, dto.CreatedBy, dto.UpdatedBy, dto.IsCampaignDetail };
            string spQuery = @"EXEC [sp_InsertQuestion] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return questionRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// update question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateQuestion(QuestionDto dto)
        {
            object[] parameter = { dto.ID, dto.QuestionText,dto.QuestionCode,dto.QuestionTextNonHTML,
                dto.QuestionTypeCode, dto.UpdatedBy, dto.IsDeleted };
            string spQuery = @"EXEC [sp_UpdateQuestion] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return questionRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<QuestionDto> GetQuestion(QuestionDto dto)
        {
            object[] parameter = { dto.ServiceTypeID };
            string spQuery = @"EXEC [sp_GetQuestion] {0}";
            return questionRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal QuestionDto GetQuestionByID(QuestionDto dto)
        {
            object[] parameter = { dto.ID };
            string spQuery = @"EXEC [sp_GetQuestionByID] {0}";
            return questionRepo.ExecuteQuerySingleOBS(spQuery, parameter);
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteQuestion(QuestionDto dto)
        {
            object[] parameter = { dto.ID };
            string spQuery = @"EXEC [sp_DeleteQuestion] {0}";
            return questionRepo.ExecuteCommandOBS(spQuery, parameter);
        }


        /// <summary>
        /// Check Exist
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<QuestionDto> CheckExist(QuestionDto dto)
        {
            object[] parameter = { dto.ID, dto.QuestionCode, dto.ServiceTypeID };
            string spQuery = @"EXEC [sp_LoadServiceDetail] {0}, {1}, {2}";
            return questionRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Answer Type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal IEnumerable<AnswerTypeDto> AnswerType()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_AnswerType]";
            return answerTypeRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Check Question Code Exist
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<QuestionDto> CheckQuestionCodeExist(QuestionDto dto)
        {
            object[] parameter = { dto.ID, dto.QuestionCode, dto.ServiceTypeID };
            string spQuery = @"EXEC [sp_CheckQuestionCodeExist] {0}, {1}, {2}";
            return questionRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// GetByServiceType
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<QuestionDto> GetByServiceType(QuestionDto dto)
        {
            object[] parameter = { dto.ServiceTypeID };
            string spQuery = @"EXEC sp_GetByServiceType {0}";
            return questionRepo.ExecuteQueryOBS(spQuery, parameter);
        }
        

        
    }
}