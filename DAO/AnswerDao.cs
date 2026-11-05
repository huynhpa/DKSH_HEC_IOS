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
    public class AnswerDao
    {
        private GenericRepository<AnswerDto> answerRepo;
        private GenericRepository<CampaignDetailDto> CampaignDetailRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// initialize
        /// </summary>
        public AnswerDao()
        {
            answerRepo = new GenericRepository<AnswerDto>(new OBSEntity());
            CampaignDetailRepo = new GenericRepository<CampaignDetailDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// GetAnswerByQuestionID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AnswerDto> GetAnswerByQuestionID(AnswerDto dto)
        {
            object[] param = { dto.QuestionID, dto.SurveyVoteID };
            string spQuery = @"exec GetAnswerByQuestionID {0}, {1}";
            return answerRepo.ExecuteQueryOBS(spQuery, param);
        }
        /// <summary>
        /// GetByQuestionIDNew
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AnswerDto> GetByQuestionIDNew(OBSurveyParam dto)
        {
            object[] param = { dto.QuestionID };
            string spQuery = @"exec sp_GetByQuestionIDNew {0}";
            return answerRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// get answer by question id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AnswerDto> GetByQuestion(AnswerDto dto)
        {
            object[] param = { dto.QuestionID };
            string spQuery = @"exec sp_GetByQuestion {0}";
            return answerRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// get answer detail by ID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal AnswerDto GetAnswerByID(AnswerDto dto)
        {
            object[] param = { dto.ID };
            string spQuery = @"exec sp_GetAnswerByID {0}";
            return answerRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Update Answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateAnswer(AnswerDto dto)
        {
            object[] param = { dto.ID ,dto.AnswerCode ,dto.QuestionID ,dto.ContentAnswer
                               ,dto.GroupCode ,dto.AnswerTypeCode ,dto.Mark ,dto.OrderIndex ,dto.AnswerValue ,dto.MinValue
                               ,dto.MaxValue , dto.UpdatedBy , dto.IsActive, dto.CampaignDetailId, dto.CampaignDescription
                               ,dto.ServiceTypeID };
            string spQuery = @"exec sp_UpdateAnswer {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}";
            return answerRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Insert Answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertAnswer(AnswerDto dto)
        {
            object[] param = { dto.ID ,dto.AnswerCode ,dto.QuestionID ,(string.IsNullOrEmpty(dto.ContentAnswer) ? string.Empty : dto.ContentAnswer)
                               ,dto.GroupCode ,dto.AnswerTypeCode ,dto.Mark ,dto.OrderIndex ,dto.AnswerValue ,dto.MinValue
                               ,dto.MaxValue ,dto.CreatedBy ,dto.UpdatedBy ,dto.IsActive, dto.CampaignDetailId, dto.CampaignDescription
                               ,dto.ServiceTypeID };
            string spQuery = @"exec sp_InsertAnswer {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}";
            return answerRepo.ExecuteCommandOBS(spQuery, param);
        }


        /// <summary>
        /// Delete Answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteAnswer(AnswerDto dto)
        {
            object[] param = { dto.ID };
            string spQuery = @"exec sp_DeleteAnswer {0}";
            return answerRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyVote"></param>
        /// <returns></returns>
        internal IEnumerable<AnswerDto> GetAnswerByServiceType(SurveyVoteDto surveyVote)
        {
            object[] param = { surveyVote.ServiceTypeID };
            string spQuery = @"exec sp_GetAnswerByServiceType {0}";
            return answerRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// get detail campaign by id
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal CampaignDetailDto GetCampaignDetailById(CampaignDetailDto json)
        {
            object[] parameter = { json.CampaignDetailId };
            string spQuery = @"EXEC sp_GetCampaignDetailById {0}";
            return CampaignDetailRepo.ExecuteQuerySingleOBS(spQuery, parameter);
        }

        /// <summary>
        /// get questions and answers by servicetype
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        internal IEnumerable<AnswerDto> GetQuestionAnswerByServiceType(AnswerDto a)
        {
            object[] param = { a.ServiceTypeID };
            string spQuery = @"exec sp_GetQuestionAnswerByServiceType {0}";
            return answerRepo.ExecuteQueryOBS(spQuery, param);
        }
    }
}