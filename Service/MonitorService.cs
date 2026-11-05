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
    public interface MonitorService
    {
        IEnumerable<MonitorHistoryDto> SearchMonitorHistory(MonitorHistoryDto dto);
        MonitorHistoryDto GetRecordInfoToMonitor(MonitorHistoryDto dto);
        IEnumerable<CriterialEvaluationCallDto> CriterialEvaluationCall();
        IEnumerable<EvaluationMarkDto> EvaluationMark();
        ResultMessageDto SaveEvaluationCall(EvaluationDataParam data, MonitorHistoryDto dto);
        IEnumerable<AgentByLeaderDto> GetAgentByLeader(MonitorHistoryDto dto);
        IEnumerable<AgentByLeaderDto> GetLeaderList();
        MonitorHistoryDto GetMonitorDetail(MonitorHistoryDto dto);
        IWorkbook MonitorHistoryExport(MonitorHistoryDto dto);
    }
}
