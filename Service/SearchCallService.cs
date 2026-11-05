using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface SearchCallService
    {
        IEnumerable<SearchCallDto> Search(SearchCallDto param);
        IWorkbook ReportCall(IEnumerable<SearchCallDto> searchcallList, bool? duplicateCall);
        IEnumerable<ServiceTypeDto> GetCampaignList();
        IWorkbook SurveyResult(SearchCallDto json);
        IEnumerable<ProjectDto> GetProjectList();
        IWorkbook DuplicateCall(SearchCallDto json);
        IWorkbook ActiveOutletDaily(SearchCallDto json);
        ResultMessageDto UploadCustomer(HttpPostedFileBase aFile, SearchCallParam dto);
        ResultMessageDto ProcessActiveOutletDaily(SearchCallParam dto);
        IEnumerable<SearchCallDto> SearchEffectiveCampaignHistory(SearchCallDto json);
        IWorkbook ExportEffectiveCampaign(SearchCallDto json);
        IWorkbook CallCampaignHistory(SearchCallDto json);
        IWorkbook SFEReport(SearchCallDto json);
    }
}
