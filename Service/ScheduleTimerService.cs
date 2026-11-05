using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;

namespace DKSHApplication.Service
{
    public interface ScheduleTimerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleTimerDto"></param>
        /// <returns></returns>
        IEnumerable<ScheduleTimerDto> SearchScheduleTimer(ScheduleTimerDto scheduleTimerDto);
        IWorkbook ExportSchedule(ScheduleTimerDto scheduleTimerDto);
        ResultMessageDto NewAgents(ShiftAgentsDto agent);
        ResultMessageDto UpdateAgents(ShiftAgentsDto agent);
        ShiftAgentsDto GetAgentInfo(ShiftAgentsDto agent);
        IEnumerable<ShiftAgentsDto> SearchAgents(ShiftAgentsDto dto);
        ShiftAgentsDto CheckAgentExisted(ShiftAgentsDto dto);
    }
}
