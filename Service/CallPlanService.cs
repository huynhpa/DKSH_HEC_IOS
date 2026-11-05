using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface CallPlanService
    {
        ResultMessageDto UploadCallPlan(HttpPostedFileBase aFile);
        IEnumerable<CallPlanDto> SearchCallPlan(CallPlanDto dto);
        ResultMessageDto ImportCampaignByCustomer(HttpPostedFileBase aFile, string WindowUser, CallPlanDto dto);
        IWorkbook CampaignByCustomerTemplate(CampaignDto dto);
        IEnumerable<ServiceTypeDto> AllCampaignActive(CampaignDto dto);
        IEnumerable<CallPlanByCampaignDto> SearchCallPlanByCampaign(CallPlanByCampaignDto dto);
        ResultMessageDto ProcessCallPlanByCampaign(CallPlanByCampaignDto dto);
        ResultMessageDto GetCallPlan(CallPlanDto dto);
        ResultMessageDto RouteToPhoneNumber(CallPlanDto dto);
        IWorkbook ExportCampaignFocus(CallPlanDto dto);
        ResultMessageDto ImportDailyInTimeSummary(HttpPostedFileBase aFile, string windowUser, CallPlanDto dto);
        IEnumerable<DailyInTimeSummaryDto> SearchDailyInTimeSummary(CallPlanDto dto);
        ResultMessageDto ImportLimitedCustomer(HttpPostedFileBase aFile, LimitedCustomerDto dto);
        IEnumerable<LimitedCustomerDto> SearchLimitedCustomer(LimitedCustomerDto dto);
        ResultMessageDto CheckLimitedCustomer(LimitedCustomerDto dto);
    }
}
