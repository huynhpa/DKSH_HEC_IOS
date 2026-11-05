using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface HomeService
    {
        OBSDashboardDto OBSDashboardSummary(OBSDashboardDto dto);
        IEnumerable<SummaryByCampaignDto> SummaryByCampaign(OBSDashboardDto dto);
        IWorkbook ExportSummaryByCampaign(IEnumerable<SummaryByCampaignDto> result);
        IEnumerable<SummaryByAgentDto> SummaryByAgent(OBSDashboardDto dto);
        IWorkbook ExportSummaryByAgent(IEnumerable<SummaryByAgentDto> result);
        IEnumerable<AgentByLeaderDto> SupsList();
        IEnumerable<AgentByLeaderDto> SaleRepBySup(AgentByLeaderDto dto);
        ResultMessageDto RefreshPerformanceData(OBSDashboardDto dto);
    }
}
