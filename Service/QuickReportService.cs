using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface QuickReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        IEnumerable<QuickReportDto> SearchQuickReport(QuickReportDto json);

        /// <summary>
        /// GetQuickReportDetail
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        QuickReportDto GetQuickReportDetail(QuickReportDto json);

        /// <summary>
        /// InsertQuickReport
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        QuickReportResult InsertQuickReport(QuickReportDto json);

        /// <summary>
        /// UpdateQuickReport
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        QuickReportResult UpdateQuickReport(QuickReportDto json);

        /// <summary>
        /// DeleteQuickReport
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        QuickReportResult DeleteQuickReport(QuickReportDto json);
    }
}
