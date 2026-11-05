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
    public class SurveyResultDao
    {
        private OBSEntity obsEntity = null;
        private GenericRepository<SurveyResultDto> surveyResultRepo;

        /// <summary>
        /// INIT
        /// </summary>
        public SurveyResultDao()
        {
            surveyResultRepo = new GenericRepository<SurveyResultDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// DeleteBySurveyVoteId
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteBySurveyVoteId(SurveyResultDto dto)
        {
            object[] param = { dto.SurveyVoteID };
            string spQuery = @" EXEC sp_DeleteBySurveyVoteId {0}";
            return surveyResultRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Get Survey Result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SurveyResultDto> GetSurveyResult(SurveyResultDto dto)
        {
            object[] param = { dto.SurveyVoteID };
            string spQuery = @" EXEC sp_GetSurveyResult {0}";
            return surveyResultRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Insert Survey Result
        /// </summary>
        /// <param name="surveyResult"></param>
        /// <returns></returns>
        internal int InsertSurveyResult(SurveyResultDto surveyResult)
        {
            object[] param = { surveyResult.ID, surveyResult.SurveyVoteID, surveyResult.QuestionID, surveyResult.AnswerID,
                               surveyResult.AnswerContent, surveyResult.UserName, surveyResult.IsDeleted };
            string spQuery = @" EXEC sp_InsertSurveyResult {0},{1},{2},{3},{4},{5},{6}";
            return surveyResultRepo.ExecuteCommandOBS(spQuery, param);
        }
    }
}