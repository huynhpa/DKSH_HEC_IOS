using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class HomeServiceImpl : HomeService
    {
        OBSDashboardDao dashboardDao = new OBSDashboardDao();

        /// <summary>
        /// OBS Dashboard Summary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public OBSDashboardDto OBSDashboardSummary(OBSDashboardDto dto)
        {
            OBSDashboardDto result = new OBSDashboardDto();
            try
            {
                result.GauChart = dashboardDao.GauChart(dto);
                if (string.IsNullOrEmpty(dto.AgentExtension))
                {
                    result.ComboChart = dashboardDao.ComboChartSum(dto);
                }
                else
                {
                    result.ComboChart = dashboardDao.ComboChart(dto);
                }
            }
            catch(SqlException ex)
            {
                result.CODE = Constant.ERROR;
                result.MESSAGE = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SummaryByCampaignDto> SummaryByCampaign(OBSDashboardDto dto)
        {
            if (string.IsNullOrEmpty(dto.Month))
            {
                dto.Month = DateTime.Now.ToString("MM-yyyy");
            }
            IEnumerable<SummaryByCampaignDto> result = dashboardDao.SummaryByCampaign(dto);
            return result;
        }

        /// <summary>
        /// Export Summary By Campaign
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public IWorkbook ExportSummaryByCampaign(IEnumerable<SummaryByCampaignDto> result)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "SummaryByCampaign.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(0);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                var font = (XSSFFont)workbook.CreateFont();
                var style = (XSSFCellStyle)workbook.CreateCellStyle();
                font.FontName = "Segoe UI";
                font.FontHeightInPoints = 9;
                style.SetFont(font);
                int rowIdx = 1;
                foreach (SummaryByCampaignDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.ServiceName); // Campaign
                    sheet.GetRow(rowIdx).GetCell(0).CellStyle = style;
                    row.CreateCell(1).SetCellValue(item.Success.ToString()); // Success
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    row.CreateCell(2).SetCellValue(item.Unsuccess.ToString()); // Unsuccess
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// Summary By Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SummaryByAgentDto> SummaryByAgent(OBSDashboardDto dto)
        {
            if (string.IsNullOrEmpty(dto.Month))
            {
                dto.Month = DateTime.Now.ToString("MM-yyyy");
            }
            IEnumerable<SummaryByAgentDto> result = dashboardDao.SummaryByAgent(dto);
            return result;
        }

        /// <summary>
        /// Export Summary By Agent
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public IWorkbook ExportSummaryByAgent(IEnumerable<SummaryByAgentDto> result)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "SummaryByAgent.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(0);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                var font = (XSSFFont)workbook.CreateFont();
                var style = (XSSFCellStyle)workbook.CreateCellStyle();
                font.FontName = "Segoe UI";
                font.FontHeightInPoints = 9;
                style.SetFont(font);
                int rowIdx = 1;
                foreach (SummaryByAgentDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.AgentNumber); // Agent number
                    sheet.GetRow(rowIdx).GetCell(0).CellStyle = style;
                    row.CreateCell(1).SetCellValue(item.FullName); // Fullname
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    row.CreateCell(2).SetCellValue(item.EmployeeID); // Saleman code
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    row.CreateCell(3).SetCellValue(item.ServiceName); // Campaign
                    sheet.GetRow(rowIdx).GetCell(3).CellStyle = style;
                    row.CreateCell(4).SetCellValue(item.Success.ToString()); // Success
                    sheet.GetRow(rowIdx).GetCell(4).CellStyle = style;
                    row.CreateCell(5).SetCellValue(item.Unsuccess.ToString()); // Unsuccess
                    sheet.GetRow(rowIdx).GetCell(5).CellStyle = style;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// SupsList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AgentByLeaderDto> SupsList()
        {
            IEnumerable<AgentByLeaderDto> result = dashboardDao.SupsList();
            return result;
        }

        /// <summary>
        /// SaleRepBySup
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AgentByLeaderDto> SaleRepBySup(AgentByLeaderDto dto)
        {
            IEnumerable<AgentByLeaderDto> result = dashboardDao.SaleRepBySup(dto);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto RefreshPerformanceData(OBSDashboardDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (!string.IsNullOrEmpty(dto.AgentExtension))
                {
                    result.number = dashboardDao.SaveDataPerformanceByAgent(dto);
                    result.number = dashboardDao.RefreshPerformanceData(dto);
                }
                else
                {
                    result.code = Constant.INFO;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
               
            return result;
        }
    }
}