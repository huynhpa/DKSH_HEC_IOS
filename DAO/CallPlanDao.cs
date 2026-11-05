using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CallPlanDao
    {
        private GenericRepository<CallPlanDto> callplanRepo;
        //private GenericRepository<CallPlanResultDto> callplanresultRepo;
        private GenericRepository<CallPlanByCampaignDto> CallPlanByCampaignRepo;
        private GenericRepository<OSSCallPlanDto> OSSCallPlanRepo;
        private GenericRepository<DailyInTimeSummaryDto> dailySummaryRepo;
        private GenericRepository<LimitedCustomerDto> limitedCustomerRepo;
        private OBSEntity obsEntity = null;

        /// <summary>
        /// init
        /// </summary>
        public CallPlanDao()
        {
            callplanRepo = new GenericRepository<CallPlanDto>(new OBSEntity());
            OSSCallPlanRepo = new GenericRepository<OSSCallPlanDto>(new OBSEntity());
            CallPlanByCampaignRepo = new GenericRepository<CallPlanByCampaignDto>(new OBSEntity());
            dailySummaryRepo = new GenericRepository<DailyInTimeSummaryDto>(new OBSEntity());
            limitedCustomerRepo = new GenericRepository<LimitedCustomerDto>(new OBSEntity());
            obsEntity = new OBSEntity();
        }

        /// <summary>
        /// Search CallPlan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallPlanDto> SearchCallPlan(CallPlanDto dto)
        {
            object[] parameter = { dto.salesman_code, dto.cust_code, Utils.StringDateFormat(dto.route_date),
                                Utils.StringDateFormat(dto.route_date_end) };
            string spQuery = @"EXEC sp_CallPlan {0}, {1}, {2}, {3}";
            return callplanRepo.ExecuteQueryOBS(spQuery, parameter);
        }

        /// <summary>
        /// Get Top 1 Call Plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal CallPlanDto Top1CallPlan(CallPlanDto dto)
        {
            object[] parameter = { dto.salesman_code };
            string spQuery = @"EXEC [sp_Top1CallPlan] {0}";
            return callplanRepo.ExecuteQuerySingleOBS(spQuery, parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateIsAssignCallPlan(CallPlanDto dto)
        {
            object[] parameter = { dto.callplan_id };
            string spQuery = @"EXEC [sp_UpdateIsAssignCallPlan] {0}";
            return callplanRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Insert Call Plan Result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int InsertCallPlanResult(CallPlanResultDto dto)
        {
            object[] parameter = { dto.callresult_id, dto.callplan_id, dto.salesman_code, dto.cust_code };
            string spQuery = @"EXEC [sp_InsertCallPlanResult] {0}, {1}, {2}, {3}";
            return callplanRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Insert CallPlan
        /// </summary>
        /// <param name="callPlan"></param>
        /// <returns></returns>
        internal int InsertCallPlan(CallPlanDto callPlan)
        {
            object[] parameter = { callPlan.callplan_id, callPlan.salesman_code, callPlan.cust_code
                    , callPlan.route_date, callPlan.txn_timestamp };
            string spQuery = @"EXEC [sp_SaveCallPlan] {0}, {1}, {2}, {3}, {4}";
            return callplanRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// SaveCallPlanFromBlueOcean
        /// </summary>
        /// <param name="callPlan"></param>
        /// <returns></returns>
        internal int SaveCallPlanFromBlueOcean(CallPlanDto callPlan)
        {
            object[] parameter = { callPlan.callplan_id, callPlan.salesman_code, callPlan.cust_code
                    , callPlan.route_date, callPlan.txn_timestamp };
            string spQuery = @"EXEC [sp_SaveCallPlanFromBlueOcean] {0}, {1}, {2}, {3}, {4}";
            return callplanRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Update survey status call plan result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateSurveyStatus(CallPlanResultDto dto)
        {
            object[] parameter = { dto.SurveyVoteID, dto.visitedResult, dto.callplan_id };
            string spQuery = @"EXEC [sp_UpdateSurveyStatus] {0}, {1}, {2}";
            return callplanRepo.ExecuteCommandOBS(spQuery, parameter);
        }

        /// <summary>
        /// Import call plan assgin customer to campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveCallplanByCampaign(CallPlanByCampaignDto dto)
        {
            object[] paras = { dto.CustCode, dto.ServiceCode, dto.create_by, dto.indicator,
                                dto.GroupCode, dto.user_ext, dto.update_by, dto.salesman_code, dto.Remark };
            string spQuery = @"EXEC sp_SaveCallplanByCampaign {0},{1},{2},{3},{4},{5},{6},{7},{8}";
            return CallPlanByCampaignRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// Update OSS Call Plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdateOSSCallPlan(OSSCallPlanDto dto)
        {
            object[] paras = { dto.service_type_code,dto.project_id, dto.cust_code, dto.indicator,
                                dto.saleman_code, dto.user_ext, dto.update_by, dto.call_date, dto.call_status };
            string spQuery = @"EXEC sp_UpdateOSSCallPlan {0},{1},{2},{3},{4},{5},{6},{7},{8}";
            return OSSCallPlanRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// Campaign By Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallPlanByCampaignDto> SearchCallPlanByCampaign(CallPlanByCampaignDto dto)
        {
            object[] paras = { dto.ServiceTypeId, dto.CustCode, dto.salesman_code, dto.call_status, Utils.StringMonthFormat(dto.FocusMonth) };
            string spQuery = @"EXEC sp_CampaignByCustomer {0},{1},{2},{3},{4}";
            return CallPlanByCampaignRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// get customer by campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallPlanByCampaignDto> GetCustomerByCampaign(CallPlanByCampaignDto dto)
        {
            object[] paras = { dto.CustCode, dto.GroupCode };
            string spQuery = @"EXEC sp_GetCustomerByCampaign {0},{1}";
            return CallPlanByCampaignRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// combine call plan to campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallPlanByCampaignDto> CombineCallPlanToCampaign(CallPlanByCampaignDto dto)
        {
            object[] paras = { Utils.StringDateFormat(dto.CallDate), Utils.StringDateFormat(dto.CallDateTo) };
            string spQuery = @"EXEC sp_CombineCallPlanToCampaign {0},{1},{2}";
            return CallPlanByCampaignRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// CallPlanBySaleman
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<CallPlanDto> CallPlanBySaleman(CallPlanDto dto)
        {
            object[] paras = { dto.salesman_code, dto.route_date };
            string spQuery = @"EXEC sp_CallPlanBySaleman {0},{1}";
            return callplanRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// delete call plan
        /// </summary>
        /// <returns></returns>
        internal int DeleteCallPlan()
        {
            object[] paras = { };
            string spQuery = @"EXEC sp_DeleteCallPlan";
            return CallPlanByCampaignRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// get call plan daily
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<CallPlanDto> GetCallPlanDaily(CallPlanDto dto)
        {
            object[] paras = { };
            string spQuery = @"EXEC sp_GetCallPlanDaily";
            return callplanRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// check customer whether focus in campaign
        /// </summary>
        /// <param name="oss"></param>
        /// <returns></returns>
        internal IEnumerable<OSSCallPlanDto> CheckCustomerWhetherFocusCampaign(OSSCallPlanDto oss)
        {
            object[] paras = { oss.cust_code, oss.user_ext, oss.group_code };
            string spQuery = @"EXEC sp_CheckCustomerWhetherFocusCampaign {0},{1},{2}";
            return OSSCallPlanRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// Delete Focus Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteFocusCampaign(CallPlanDto dto)
        {
            object[] paras = { dto.ServiceTypeId, dto.FocusMonth, dto.remove_type };
            string spQuery = @"EXEC sp_DeleteFocusCampaign {0},{1},{2}";
            return OSSCallPlanRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// SearchDailyInTimeSummary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<DailyInTimeSummaryDto> SearchDailyInTimeSummary(CallPlanDto dto)
        {
            object[] paras = { dto.salesman_code, dto.summary_month };
            string spQuery = @"EXEC sp_SearchDailyInTimeSummary {0},{1}";
            return dailySummaryRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// DailyInTimeSummary
        /// </summary>
        /// <param name="dits"></param>
        /// <returns></returns>
        internal int DailyInTimeSummary(DailyInTimeSummaryDto dits)
        {
            object[] paras = { dits.sales_area_code
                            ,dits.sales_area_name
                            ,dits.salesrep_code
                            ,dits.salesrep_name
                            ,dits.visit_id
                            ,dits.cust_code
                            ,dits.cust_name
                            ,dits.cont_code
                            ,dits.cont_name
                            ,dits.clas
                            ,dits.call_date
                            ,dits.time_in
                            ,dits.time_out
                            ,dits.time_spend
                            ,dits.reason_code
                            ,dits.reason_name
                            ,dits.remarks
                            ,dits.start_date
                            ,dits.end_date
                            ,dits.dummy_reason_code
                            ,dits.txn_timestamp
                            ,dits.sfms_ind
                            ,dits.call_ind
                            ,dits.sales_ord_ind
                            ,dits.tra_ind
                            ,dits.coll_ind
                            ,dits.det_ind
                            ,dits.visit_ind
                            ,dits.drc_ind
                            ,dits.mss_ind
                            ,dits.in_latitude
                            ,dits.in_longitude
                            ,dits.in_gpstime
                            ,dits.out_latitude
                            ,dits.out_longitude
                            ,dits.out_gpstime
                            ,dits.summary_month
                        };
            string spQuery = @"EXEC sp_SaveDailyInTimeSumary {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}
                                                        ,{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}
                                                        ,{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36}";
            return dailySummaryRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteDailyInTimeSummary(CallPlanDto dto)
        {
            object[] paras = { dto.summary_month };
            string spQuery = @"delete from [dbo].[DailyInTimeSummary] where summary_month = {0}";
            return dailySummaryRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<LimitedCustomerDto> SearchLimitedCustomer(LimitedCustomerDto dto)
        {
            object[] paras = { Utils.StringDateFormat(dto.limit_from), Utils.StringDateFormat(dto.limit_to), dto.leader_ext };
            string spQuery = @"EXEC sp_SearchLimitedCustomer {0},{1},{2}";
            return limitedCustomerRepo.ExecuteQueryOBS(spQuery, paras);
        }

        /// <summary>
        /// Save Limited Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveLimitedCustomer(LimitedCustomerDto dto)
        {
            object[] paras = { dto.cust_code,Utils.StringDateFormat(dto.limit_from),Utils.StringDateFormat(dto.limit_to), dto.leader_ext };
            string spQuery = @"exec sp_SaveLimitedCustomer {0},{1},{2},{3}";
            return limitedCustomerRepo.ExecuteCommandOBS(spQuery, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<LimitedCustomerDto> CheckLimitedCustomer(LimitedCustomerDto dto)
        {
            object[] paras = { dto.cust_code, dto.call_date, dto.extension };
            string spQuery = @"EXEC sp_CheckLimitedCustomer {0},{1},{2}";
            return limitedCustomerRepo.ExecuteQueryOBS(spQuery, paras);
        }
    }
}