using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DKSHApplication.Service
{
    public interface CDIDashboardService
    {
        /// <summary>
        /// CustCode Registered By DAY MONTH YEAR
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CdiDashboardDto GetCDIDashboard(CDIDashboardParam dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IWorkbook ExportLicensesSummary(CdiDashboardDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hour"></param>
        void ScheduleTimerAutoLarmExpired(int hour);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hour"></param>
        void ScheduleTimerAutoLarmComingExpire(int hour);

        /// <summary>
        /// Auto Alarm CDI Report Daily
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ResultMessageDto AutoAlarmCDIReportDaily();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResultMessageDto SendMailToSuperVisor();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IWorkbook ChannelHistoryReport(CdiDashboardDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<SummaryChannelHistoryDto> SummaryChannelHistory(CDIDashboardParam dto);

        /// <summary>
        /// View Detail Channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IWorkbook ViewDetailChannel(CdiDashboardDto dto);

        /// <summary>
        /// View Total Every Month
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IWorkbook ViewTotalEveryMonth(CdiDashboardDto dto);

        /// <summary>
        /// Compare Any Month
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IWorkbook CompareAnyMonth(CdiDashboardDto dto);
    }
}
