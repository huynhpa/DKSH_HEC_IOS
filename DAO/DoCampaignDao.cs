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
    public class DoCampaignDao
    {
        private GenericRepository<ServiceTypeDto> serviceTypeRepo;
        private GenericRepository<hrEmployeeDto> hrEmpRepo;
        private GenericRepository<StatusDto> statusRepo;
        private GenericRepository<AgeRankDto> ageRankRepo;
        private GenericRepository<DoCampaignResultDto> docampaignRepo;
        private GenericRepository<SearchCallDto> searchCallRepo;
        private GenericRepository<Click2CallDto> Click2CallRepo;
        private GenericRepository<EffectiveCampaignHistoryDto> EffectiveCampaignHistoryRepo;
        private GenericRepository<CallPlanByCampaignDto> CallPlanByCampaignRepo;
        private GenericRepository<OSSCallPlanDto> OSSCallPlanRepo;
        private GenericRepository<DoctorInformationDto> doctorInfoRepo;
        private GenericRepository<iMindContactInfoDto> contactRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// initialize
        /// </summary>
        public DoCampaignDao()
        {
            serviceTypeRepo = new GenericRepository<ServiceTypeDto>(new OBSEntity());
            hrEmpRepo = new GenericRepository<hrEmployeeDto>(new OBSEntity());
            statusRepo = new GenericRepository<StatusDto>(new OBSEntity());
            ageRankRepo = new GenericRepository<AgeRankDto>(new OBSEntity());
            docampaignRepo = new GenericRepository<DoCampaignResultDto>(new OBSEntity());
            searchCallRepo = new GenericRepository<SearchCallDto>(new OBSEntity());
            Click2CallRepo = new GenericRepository<Click2CallDto>(new OBSEntity());
            EffectiveCampaignHistoryRepo = new GenericRepository<EffectiveCampaignHistoryDto>(new OBSEntity());
            CallPlanByCampaignRepo = new GenericRepository<CallPlanByCampaignDto>(new OBSEntity());
            OSSCallPlanRepo = new GenericRepository<OSSCallPlanDto>(new OBSEntity());
            doctorInfoRepo = new GenericRepository<DoctorInformationDto>(new OBSEntity());
            contactRepo = new GenericRepository<iMindContactInfoDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Load Age Rank
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<AgeRankDto> LoadAgeRank()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_LoadAgeRank]";
            return ageRankRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Active Service Type
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ServiceTypeDto> GetActiveServiceType(OBSurveyParam dto)
        {
            object[] parameter = { dto.ProjectID };
            string spQuery = @"EXEC [sp_GetActiveServiceType] {0}";
            return serviceTypeRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Group ID By Windows User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<hrEmployeeDto> GetGroupIDByWindowsUser(OBSurveyParam dto)
        {
            object[] parameter = { dto.WindowsUser };
            string spQuery = @"EXEC [sp_GetGroupIDByWindowsUser] {0}";
            return hrEmpRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Load Status
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatusDto> LoadStatus()
        {
            object[] parameter = { };
            string spQuery = @"EXEC [sp_LoadStatus]";
            return statusRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Outbound History
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DoCampaignResultDto> OutboundHistory(ContactsDto dto)
        {
            object[] parameter = { dto.code };
            string spQuery = @"EXEC [sp_OutboundHistory] {0}";
            return docampaignRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// DeleteSurvey
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal int DeleteSurvey(SurveyVoteDto param)
        {
            object[] par = { param.ID };
            string spQuery = "EXEC sp_DeleteSurvey {0}";
            return serviceTypeRepo.ExecuteCommandOBS(spQuery, par);
        }

        /// <summary>
        /// Save Survey Result Export
        /// </summary>
        /// <param name="surveyExport"></param>
        /// <returns></returns>
        internal int SaveSurveyResultExport(SurveyResultExportDto surveyExport)
        {
            object[] par = {  surveyExport.SurveyVoteId, surveyExport.AnswerField01, surveyExport.AnswerField02
                            , surveyExport.AnswerField03, surveyExport.AnswerField04, surveyExport.AnswerField05
                            , surveyExport.AnswerField06, surveyExport.AnswerField07, surveyExport.AnswerField08
                            , surveyExport.AnswerField09, surveyExport.AnswerField10, surveyExport.AnswerField11
                            , surveyExport.AnswerField12, surveyExport.AnswerField13, surveyExport.AnswerField14
                            , surveyExport.AnswerField15 };
            string spQuery = @"EXEC sp_SaveSurveyExportResult {0},{1},{2},{3},{4},{5},{6},{7},{8}
                                                             ,{9},{10},{11},{12},{13},{14},{15}";
            return serviceTypeRepo.ExecuteCommandOBS(spQuery, par);
        }

        /// <summary>
        /// Check Survey Duplicate
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> CheckSurveyDuplicate(OBSurveyParam param)
        {
            object[] par = { param.CustCode };
            string spQuery = @"EXEC sp_CheckSurveyDuplicate {0}";
            return searchCallRepo.ExecuteQueryOBS(spQuery, par);
        }

        /// <summary>
        /// click 2 call
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal int Click2Call(Click2CallDto param)
        {
            object[] par = { param.click2callid, param.extension, param.phone_number, param.action_autocall, param.stringcode, param.CustCode };
            string spQuery = "EXEC sp_Click2Call {0},{1},{2},{3},{4},{5}";
            return Click2CallRepo.ExecuteCommandOBS(spQuery, par);
        }

        /// <summary>
        /// Save effective campaign history
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveEffectiveCampaignHistory(EffectiveCampaignHistoryDto dto)
        {
            object[] par = { dto.ServiceTypeID, dto.SurveyVoteID, dto.CreateBy };
            string spQuery = "EXEC sp_SaveEffectiveCampaignHistory {0},{1},{2}";
            return EffectiveCampaignHistoryRepo.ExecuteCommandOBS(spQuery, par);
        }

        /// <summary>
        /// get effective campaign history
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<EffectiveCampaignHistoryDto> GetEffectiveCampaignHistory(EffectiveCampaignHistoryDto dto)
        {
            object[] par = { dto.SurveyVoteID };
            string spQuery = "EXEC sp_GetEffectiveCampaignHistoryBySurveyVote {0}";
            return EffectiveCampaignHistoryRepo.ExecuteQueryOBS(spQuery, par);
        }

        /// <summary>
        /// customer focus campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallPlanByCampaignDto> CustomerFocusCampaign(CallPlanByCampaignDto dto)
        {
            object[] par = { dto.CustCode, dto.GroupCode };
            string spQuery = "EXEC sp_CustomerFocusCampaign {0},{1}";
            return CallPlanByCampaignRepo.ExecuteQueryOBS(spQuery, par);
        }

        /// <summary>
        /// Focus Campaign Send Customer History
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<OSSCallPlanDto> FocusCampaignSendCustomerHistory(OSSCallPlanDto dto)
        {
            object[] par = { dto.cust_code, dto.group_code, dto.user_ext };
            string spQuery = "EXEC sp_FocusCampaignSendCustomerHistory {0},{1},{2}";
            return OSSCallPlanRepo.ExecuteQueryOBS(spQuery, par);
        }

        /// <summary>
        /// Get Doctor Information
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal DoctorInformationDto GetDoctorInformation(OBSurveyParam param)
        {
            object[] par = { param.CustCode };
            string spQuery = "EXEC sp_GetDoctorInfoById {0}";
            return doctorInfoRepo.ExecuteQuerySingleOBS(spQuery, par);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        internal IEnumerable<DoctorInformationDto> GetDoctorCallplan(OBSurveyParam para)
        {
            object[] par = { para.UserExt };
            string spQuery = "EXEC sp_GetDoctorCallplan {0}";
            return doctorInfoRepo.ExecuteQueryOBS(spQuery, par);
        }

        internal int AddContactList(iMindContactInfoDto json)
        {
            object[] par = { json.code, json.phone_number, json.PersonName, json.update_by };
            string spQuery = @"EXEC sp_AddContactList {0},{1},{2},{3}";
            return contactRepo.ExecuteCommandOBS(spQuery, par);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal int SaveSpecialNote(iMindContactInfoDto info)
        {
            object[] par = { info.code, info.phone_number, info.note };
            string spQuery = @"EXEC sp_SaveSpecialNote {0},{1},{2}";
            return contactRepo.ExecuteCommandOBS(spQuery, par);
        }
    }
}