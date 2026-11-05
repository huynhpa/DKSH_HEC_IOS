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
    public class SearchCallDao
    {
        private GenericRepository<SearchCallDto> searchcallRepo;
        private GenericRepository<ProjectDto> ProjectRepo;
        private GenericRepository<SurveyResultReportDto> SurveyResultReportRepo;
        private GenericRepository<ActiveOutletDailyDto> activeOutletRepo;
        private GenericRepository<CustomerClassDto> customerClassRepo;
        private GenericRepository<CampaignResultTitleDto> CampaignResultTitleRepo;
        private GenericRepository<CallPlanDto> CallPlanRepo;
        private GenericRepository<SurveyResultDto> SurveyResultRepo;
        private GenericRepository<SFEReportDto> SFEReportRepo;

        private OBSEntity obsEntity = null;

        public SearchCallDao()
        {
            searchcallRepo = new GenericRepository<SearchCallDto>(new OBSEntity());
            ProjectRepo = new GenericRepository<ProjectDto>(new OBSEntity());
            SurveyResultReportRepo = new GenericRepository<SurveyResultReportDto>(new OBSEntity());
            activeOutletRepo = new GenericRepository<ActiveOutletDailyDto>(new OBSEntity());
            customerClassRepo = new GenericRepository<CustomerClassDto>(new OBSEntity());
            CampaignResultTitleRepo = new GenericRepository<CampaignResultTitleDto>(new OBSEntity());
            CallPlanRepo = new GenericRepository<CallPlanDto>(new OBSEntity());
            SurveyResultRepo = new GenericRepository<SurveyResultDto>(new OBSEntity());
            SFEReportRepo = new GenericRepository<SFEReportDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Search Call
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> SearchCall(SearchCallDto dto)
        {
            object[] parameter = { dto.CustomerCode, dto.Status, Utils.StringToDateDDMMYYYYHHMM(dto.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(dto.ToDate, Constant.YYYYMMDDHHMM), dto.Agent,
                                   dto.ServiceIdList, dto.ProjectID, dto.contracts, dto.Extension };
            string spQuery = @"EXEC [sp_SearchCall] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// SearchCall Multi Rows
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> SearchCall_MultiRows(SearchCallDto dto)
        {
            object[] parameter = { dto.CustomerCode, dto.Status, Utils.StringToDateDDMMYYYYHHMM(dto.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(dto.ToDate, Constant.YYYYMMDDHHMM), dto.Agent,
                                   dto.ServiceIdList, dto.ProjectID };
            string spQuery = @"EXEC [sp_SearchCall_MultiRows] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }


        /// <summary>
        /// query file name sp_SearchCallReport
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> SearchCallEffective(SearchCallDto dto)
        {
            object[] parameter = { dto.CustomerCode, dto.Status, Utils.StringToDateDDMMYYYYHHMM(dto.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(dto.ToDate, Constant.YYYYMMDDHHMM), dto.Agent,
                                   dto.ServiceIdList, dto.ProjectID };
            string spQuery = @"EXEC [sp_SearchCallEffective] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// GetCallSurveyBySaleman
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> CallOnPlan(OBSurveyParam dto)
        {
            object[] parameter = { dto.SalesmanCode, dto.SurveyDate };
            string spQuery = @"EXEC [sp_CallOnPlan] {0}, {1}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// call un plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> CallUnPlan(OBSurveyParam dto)
        {
            object[] parameter = { dto.SalesmanCode, dto.SurveyDate };
            string spQuery = @"EXEC [sp_CallUnPlan] {0}, {1}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Campaign List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceTypeDto> GetCampaignList()
        {
            try
            {
                var query = (from s in obsEntity.ServiceTypes
                             where s.IsDeleted == false && s.IsActive == true
                             select new ServiceTypeDto {
                                 ID = s.ID,
                                 ServiceCode = s.ServiceCode,
                                 ServiceName = s.ServiceName,
                                 CreatedDate = s.CreatedDate.ToString()
                             }).OrderBy(x=>x.ServiceName).ToList();
                return query;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> GetCallByAgent(hrEmployeeDto dto)
        {
            object[] parameter = { dto.AgentNumber, dto.contracts };
            string spQuery = @"EXEC [sp_GetCallByAgent] {0},{1}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Project List
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ProjectDto> GetProjectList()
        {
            object[] parameter = { };
            string spQuery = @"EXEC sp_GetProject";
            return ProjectRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// SearchSurveyByAgent
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> SearchSurveyByAgent(SearchCallParam json)
        {
            object[] parameter = { json.SurveyCustCode, json.ServiceId, json.SurveyDate, json.AgentNumber, json.SurveyStatus };
            string spQuery = @"EXEC [sp_SearchSurveyByAgent] {0},{1},{2},{3},{4}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Survey Result By Survey
        /// </summary>
        /// <param name="sv"></param>
        /// <returns></returns>
        internal IEnumerable<SurveyResultReportDto> GetSurveyResultBySurvey(SurveyVoteDto sv)
        {
            object[] parameter = { sv.ID };
            string spQuery = @"EXEC [sp_GetSurveyResultBySurvey] {0}";
            return SurveyResultReportRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Answer Result
        /// </summary>
        /// <param name="sv"></param>
        /// <returns></returns>
        internal IEnumerable<SurveyResultReportDto> GetAnswerResult(SurveyVoteDto sv)
        {
            object[] parameter = { sv.ID };
            string spQuery = @"EXEC [sp_GetAnswerResult] {0}";
            return SurveyResultReportRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sv"></param>
        /// <returns></returns>
        internal IEnumerable<SurveyResultReportDto> GetSurveyAnswerResultByAnswer(SurveyVoteDto sv)
        {
            object[] parameter = { sv.ID, sv.AnswerID };
            string spQuery = @"EXEC [sp_GetSurveyAnswerResultByAnswer] {0},{1}";
            return SurveyResultReportRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Distinct Survey From Search Call
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> GetDistinctSurveyFromSearchCall(SearchCallDto dto)
        {
            object[] parameter = { dto.ServiceIdList, dto.ProjectID };
            string spQuery = @"EXEC sp_GetDistinctSurveyFromSearchCall {0},{1}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Summary Duplicate Call
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> SummaryDuplicateCall(SearchCallDto json)
        {
            object[] parameter = { json.Status, Utils.StringToDateDDMMYYYYHHMM(json.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(json.ToDate, Constant.YYYYMMDDHHMM),
                                   json.ServiceId, json.ProjectID };
            string spQuery = @"EXEC [sp_SummaryDuplicateCall] {0}, {1}, {2}, {3}, {4}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Active Outlet Daily
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<ActiveOutletDailyDto> ActiveOutletDaily(SearchCallDto json)
        {
            object[] parameter = { json.Months };
            string spQuery = @"EXEC [sp_ActiveOutletDaily] {0}";
            return activeOutletRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        internal int DeleteCustomerClass(SearchCallParam dto)
        {
            object[] parameter = { dto.MonthOfYear };
            string spQuery = @"EXEC [sp_DeleteCustomerClass] {0}";
            return customerClassRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// save customer class
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        internal int SaveCustomerClass(CustomerClassDto customer)
        {
            object[] parameter = { customer.CustCode, customer.CustName, customer.Channel, customer.Province, customer.Region,
                                customer.SaleRep, customer.Extension, customer.Groups, customer.UpdateBy, customer.Classic, customer.MonthOfYear };
            string spQuery = @"EXEC [sp_SaveCustomerClass] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}";
            return customerClassRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aco"></param>
        /// <returns></returns>
        internal int ProcessActiveOutletDaily(SearchCallParam aco)
        {
            object[] parameter = { aco.MonthOfYear, aco.UserName };
            string spQuery = @"EXEC [sp_ProcessActiveOutletDaily] {0}, {1}";
            return customerClassRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Count Active Outlet Daily
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<ActiveOutletDailyDto> CountActiveOutletDaily(SearchCallParam json)
        {
            object[] parameter = { json.MonthOfYear, json.Classic, json.Group , json.UserName };
            string spQuery = @"EXEC [sp_CountActiveOutletDaily] {0}, {1}, {2}, {3}";
            return activeOutletRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Update Active Outlet Daily
        /// </summary>
        /// <param name="aco"></param>
        /// <returns></returns>
        internal int UpdateActiveOutletDaily(ActiveOutletDailyDto aco)
        {
            object[] parameter = { aco.Extension, aco.Months, aco.CalledShareClassA, aco.CalledShareClassB,
                                    aco.CalledShareClassC, aco.CalledShareClassD, aco.CalledTLClassA,
                                    aco.CalledTLClassB, aco.CalledTLClassC, aco.CalledTLClassD };
            string spQuery = @"EXEC [sp_UpdateActiveOutletDaily] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
            return customerClassRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aco"></param>
        /// <returns></returns>
        internal int UpdateCalledActiveOutlet(ActiveOutletDailyDto aco)
        {
            object[] parameter = { aco.Classic, aco.Groups, aco.Months, aco.Extension, aco.CalledClass };
            string spQuery = @"EXEC [sp_UpdateCalledActiveOutlet] {0}, {1}, {2}, {3}, {4}";
            return customerClassRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        internal int SaveCampaignResultTitle(CampaignResultTitleDto title)
        {
            object[] parameter = { title.ServiceTypeID, title.ServiceQuestionsID, title.AnswerID, title.AnswerText };
            string spQuery = @"EXEC sp_SaveCampaignResultTitle {0}, {1}, {2}, {3}";
            return CampaignResultTitleRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        internal int DeleteCampaignResultTitle(CampaignResultTitleDto title)
        {
            object[] parameter = { title.ServiceTypeID };
            string spQuery = @"EXEC sp_DeleteCampaignResultTitle {0}, {1}, {2}, {3}";
            return CampaignResultTitleRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// search effective campaign history
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> SearchEffectiveCampaignHistory(SearchCallDto json)
        {
            object[] parameter = { json.CustomerCode, json.Status, Utils.StringToDateDDMMYYYYHHMM(json.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(json.ToDate, Constant.YYYYMMDDHHMM), json.Agent,
                                   json.ServiceIdList, json.contracts, json.Extension };
            string spQuery = @"EXEC sp_SearchEffectiveCampaignHistory {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SFEReportDto> SFEReport(SearchCallDto json)
        {
            object[] parameter = { json.CustomerCode, json.Status, Utils.StringToDateDDMMYYYYHHMM(json.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(json.ToDate, Constant.YYYYMMDDHHMM), json.Agent,
                                   json.ServiceIdList };
            string spQuery = @"EXEC sp_SFEReport {0}, {1}, {2}, {3}, {4}, {5}";
            return SFEReportRepo.ExecuteQueryOBS(spQuery, parameter);
        }


        /// <summary>
        /// Search Effective Campaign History By ServiceType
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> SearchEffectiveCampaignHistoryByServiceType(SearchCallDto json)
        {
            object[] parameter = { json.CustomerCode, json.Status, Utils.StringToDateDDMMYYYYHHMM(json.FromDate, Constant.YYYYMMDDHHMM),
                                   Utils.StringToDateDDMMYYYYHHMM(json.ToDate, Constant.YYYYMMDDHHMM), json.Agent,
                                   json.ProjectID, json.ServiceTypeID, json.contracts };
            string spQuery = @"EXEC sp_SearchEffectiveCampaignHistoryByServiceType {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// search effective campaign history by customer
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal IEnumerable<SearchCallDto> SearchEffectiveCampaignHistoryByCustomer(SearchCallDto json)
        {
            object[] parameter = { json.CustomerCode, json.contracts };
            string spQuery = @"EXEC sp_SearchEffectiveCampaignHistoryByCustomer {0},{1}";
            return searchcallRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Check ServiceType Whether SurveyResult
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal IEnumerable<SurveyResultDto> CheckServiceTypeWhetherSurveyResult(SearchCallDto item)
        {
            object[] parameter = { item.SurveyVoteID, item.ServiceTypeID };
            string spQuery = @"EXEC sp_CheckServiceTypeWhetherSurveyResult {0},{1}";
            return SurveyResultRepo.ExecuteQueryOBS(spQuery, parameter);
        }
    }
}