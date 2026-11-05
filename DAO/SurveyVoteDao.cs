using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class SurveyVoteDao
    {
        private GenericRepository<SurveyVoteDto> surveyvoteRepo;
        private OBSEntity obsEntity = null;
        public SurveyVoteDao()
        {
            surveyvoteRepo = new GenericRepository<SurveyVoteDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Get Survey Vote
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal SurveyVoteDto GetSurveyVote(OBSurveyParam dto)
        {
            object[] param = { dto.SurveyId };
            string spQuery = @"EXEC sp_GetSurveyVote {0}";
            return surveyvoteRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Get By Phone Number
        /// </summary>
        /// <param name="obsDto"></param>
        /// <returns></returns>
        internal SurveyVoteDto GetByPhoneNumber(OBSurveyParam obsDto)
        {
            object[] param = { obsDto.PhoneNumber };
            string spQuery = @"EXEC sp_GetByPhoneNumber {0}";
            return surveyvoteRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Get By Phone Number NotCode
        /// </summary>
        /// <param name="obsDto"></param>
        /// <returns></returns>
        internal SurveyVoteDto GetByPhoneNumberNotCode(OBSurveyParam obsDto)
        {
            object[] param = { obsDto.PhoneNumber };
            string spQuery = @"EXEC sp_GetByPhoneNumberNotCode {0}";
            return surveyvoteRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Get By PhoneNumber Service
        /// </summary>
        /// <param name="obsDto"></param>
        /// <returns></returns>
        internal SurveyVoteDto GetByPhoneNumberService(OBSurveyParam obsDto)
        {
            object[] param = { obsDto.PhoneNumber, obsDto.ServiceCode };
            string spQuery = @"EXEC sp_GetByPhoneNumberService {0}, {1}";
            return surveyvoteRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Insert Survey Vote
        /// </summary>
        /// <param name="svObj"></param>
        /// <returns></returns>
        internal int InsertSurveyVote(SurveyVoteDto svObj)
        {
            object[] param = { svObj.ID,svObj.ProvinceID,svObj.CenterID,svObj.MSBCH,svObj.EmployeeID,svObj.SurveyDate,svObj.CustomerName,svObj.CustomerAddress,svObj.CustomerPhone,svObj.ServiceTypeID,svObj.NumberOfContact,svObj.SuperviseID,
                                svObj.StartTime ,svObj.EndTime,svObj.SurveyTime,svObj.Cost,svObj.IsCallBack,svObj.CustomerComment,svObj.SurveyStatus,svObj.CustomField1,svObj.CustomField2,svObj.CustomField3,
                                svObj.CustomField4,svObj.CustomField5,svObj.CustomField6,svObj.CustomField7,svObj.CustomField8,svObj.CustomField9,svObj.CustomField10,svObj.CreatedBy,svObj.UpdatedBy };
            string spQuery = @"EXEC sp_InsertSurveyVote {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},
                                {13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},
                                {24},{25},{26},{27},{28},{29},{30}";
            return surveyvoteRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// Update SurveyVote
        /// </summary>
        /// <param name="sv"></param>
        /// <returns></returns>
        internal int UpdateSurveyVote(SurveyVoteDto sv)
        {
            object[] param = { sv.ID,sv.ProvinceID ,sv.CenterID ,sv.MSBCH ,sv.EmployeeID ,sv.SurveyDate ,sv.CustomerName ,sv.CustomerAddress ,sv.CustomerPhone ,
                            sv.CustomerAge ,sv.ServiceTypeID ,sv.NumberOfContact ,sv.SuperviseID ,sv.StartTime ,sv.EndTime ,sv.SurveyTime ,
                            sv.Cost ,sv.IsCallBack ,sv.CustomerComment ,sv.SurveyStatus ,sv.CustomField1 ,sv.CustomField2 ,sv.CustomField3 ,sv.CustomField4 ,
                            sv.CustomField5 ,sv.CustomField6 ,sv.CustomField7 ,sv.CustomField8 ,sv.CustomField9 ,sv.CustomField10 ,sv.UpdatedBy };
            string spQuery = @"EXEC sp_UpdateSurveyVote {0}, {1}, {2}, {3}, {4}, 
                            {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, 
                            {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}";
            return surveyvoteRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// sp_CanDeleteSurveyVote
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal SurveyVoteDto CanDeleteSurveyVote(SurveyVoteDto dto)
        {
            object[] param = { dto.ServiceTypeID };
            string spQuery = @"EXEC sp_CanDeleteSurveyVote {0}";
            return surveyvoteRepo.ExecuteQuerySingleOBS(spQuery, param);
        }
    }
}