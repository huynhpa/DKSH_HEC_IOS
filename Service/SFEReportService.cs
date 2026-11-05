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
    public interface SFEReportService
    {
        ResultMessageDto SaveSFEReportConst(SFEReportConstDto json);
        IEnumerable<SFEReportConstDto> SFEReportConst(SFEReportConstDto json);
        IEnumerable<SFEOrgchartDto> SFEOrgchart(SFEOrgchartDto dto);
        IEnumerable<SFEReportDto> SFEPerformanceReport(SFEOrgchartDto dto);
        ResultMessageDto SaveTargetCallTeleSale(HttpPostedFileBase aFile, TargetCallTeleSaleDto json);
        IWorkbook ExportSFEPerformance(SFEOrgchartDto dto);
    }
}
