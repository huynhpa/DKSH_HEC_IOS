using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public interface HumanAllocationService
    {
        IEnumerable<HumanAllocationDto> GetCampaignList();
        IEnumerable<AgentsDto> GetAgentList();
        ResultMessageDto LogInCallTime(HumanAllocationDto dto);
        ResultMessageDto LogOutCallTime(HumanAllocationDto dto);
        HumanAllocationDto GetLoginOrLogoutByAgent(HumanAllocationDto dto);
        IEnumerable<HumanAllocationDto> SearchHumanAllocation(HumanAllocationDto dto);
        IEnumerable<AgentJobsDto> GetAgentJob();
        LogAgentJobDto GetAgentJobDetail(LogAgentJobDto param);
        IEnumerable<LogAgentJobDto> SearchLogAgentJob(LogAgentJobDto param);
        ResultMessageDto InsertLogAgentJob(LogAgentJobDto param);
        ResultMessageDto UpdateLogAgentJob(LogAgentJobDto param);
    }
}