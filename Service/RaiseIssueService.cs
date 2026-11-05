using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface RaiseIssueService
    {
        /// <summary>
        /// Search Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        IEnumerable<RaiseIssueDto> SearchIssue(RaiseIssueParam jsonParam);

        /// <summary>
        /// Insert Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto InsertIssue(RaiseIssueParam jsonParam);

        /// <summary>
        /// Get Issue Detail
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        RaiseIssueDto GetIssueDetail(RaiseIssueParam jsonParam);

        /// <summary>
        /// Update Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto UpdateIssue(RaiseIssueParam jsonParam);

        /// <summary>
        /// Delete Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto DeleteIssue(RaiseIssueParam jsonParam);

        /// <summary>
        /// Get IssueNo List
        /// </summary>
        /// <returns></returns>
        IEnumerable<RaiseIssueDto> GetIssueNoList();

        /// <summary>
        /// Get IssueNo By Ticket
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IEnumerable<RaiseIssueDto> GetIssueNoByTicket(TicketNoDto dto);

    }
}
