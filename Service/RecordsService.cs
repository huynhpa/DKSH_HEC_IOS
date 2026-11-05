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
    public interface RecordsService
    {
        IEnumerable<Records46Dto> SearchRecord(Records46Dto dto);
        ResultMessageDto SaveRecord(HttpPostedFileBase aFile);
        IWorkbook RecordExport(Records46Dto dto);
        ResultMessageDto FetchRecordIssabel(Records46Dto dto);
        ResultMessageDto DeleteMonitor(MonitorHistoryDto dto);
        IEnumerable<TeleSaleOrgChartDto> SearchTeleOrgchart(TeleSaleOrgChartDto param);
        TeleSaleOrgChartDto GetTeleOrgChartDetail(TeleSaleOrgChartDto param);
        ResultMessageDto SaveTeleSaleOrgChart(TeleSaleOrgChartDto dto);
        ResultMessageDto DeleteTeleSaleOrgChart(TeleSaleOrgChartDto dto);
        IWorkbook TalkTime(Records46Dto dto);
    }
}
