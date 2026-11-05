using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using MP.Core.Library;
using NPOI.HSSF.UserModel;

namespace DKSHApplication.Service
{
    public class SFEReportServiceImpl : SFEReportService
    {
        SFEReportDao SFEDao = new SFEReportDao();

        /// <summary>
        /// SaveSFEReportConst
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto SaveSFEReportConst(SFEReportConstDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = SFEDao.SaveSFEReportConst(json);
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// SFEReportConst
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<SFEReportConstDto> SFEReportConst(SFEReportConstDto json)
        {
            IEnumerable<SFEReportConstDto> result = SFEDao.SFEReportConst(json);
            return result;
        }


        /// <summary>
        /// SFE Orgchart
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<SFEOrgchartDto> SFEOrgchart(SFEOrgchartDto json)
        {
            IEnumerable<SFEOrgchartDto> result = SFEDao.SFEOrgchart(json);
            return result;
        }

        /// <summary>
        /// SFE Performance Report
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SFEReportDto> SFEPerformanceReport(SFEOrgchartDto dto)
        {
            SFEReportDto json = new SFEReportDto() {
                create_by = dto.user_id
            };
            int udt = SFEDao.UpdateTeleSalePerformance(json);
            IEnumerable<SFEReportDto> result = SFEDao.SFEPerformanceReport(dto);
            return result;
        }

        /// <summary>
        /// TargetCall TeleSale
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto SaveTargetCallTeleSale(HttpPostedFileBase aFile, TargetCallTeleSaleDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            CallPlanByCampaignDto n = new CallPlanByCampaignDto();
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    IWorkbook workbook;
                    mem.SetLength(aFile.ContentLength);
                    aFile.InputStream.Read(mem.GetBuffer(), 0, aFile.ContentLength);
                    string extension = Path.GetExtension(aFile.FileName).Replace(".", "");
                    if (Constant.EXCEL_XLSX.Equals(extension))
                    {
                        workbook = new XSSFWorkbook(mem);
                    }
                    else if (Constant.EXCEL_XLS.Equals(extension))
                    {
                        workbook = new HSSFWorkbook(mem);
                    }
                    else
                    {
                        throw new Exception("This format is not supported");
                    }
                    SFEDao.DeleteTargetCallTeleSale(json);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = 0;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            TargetCallTeleSaleDto tc = new TargetCallTeleSaleDto()
                            {
                                update_by = json.update_by,
                                working_month = json.working_month,
                                sap_code = Utils.GetFormattedCellValue(row.GetCell(2)),
                                working_day = Utils.GetFormattedCellValue(row.GetCell(3)),
                                call_day = Utils.GetFormattedCellValue(row.GetCell(4))
                            };
                            rowCount += SFEDao.SaveTargetCallTeleSale(tc);
                        }
                    }
                    if (rowCount > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.number = rowCount;
                        result.message = string.Format(Constant.PROCESS_MESSAGE, rowCount);
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.MESSAGE_ERROR;
                    }
                    mem.Close();
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook ExportSFEPerformance(SFEOrgchartDto dto)
        {
            string fileExcel = "SFEPerformanceReport.xlsx";
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileExcel;
            IWorkbook workbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                string extentsion = Path.GetExtension(filePath).Replace(".", string.Empty);
                if (Constant.EXCEL_XLSX.Equals(extentsion))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }
                var color_fail = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_success = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_callback = new XSSFColor(new byte[] { 51, 153, 255 });
                var font_fail = (XSSFFont)workbook.CreateFont();
                var font_success = (XSSFFont)workbook.CreateFont();
                var font_callback = (XSSFFont)workbook.CreateFont();
                var style_normal = (XSSFCellStyle)workbook.CreateCellStyle();
                var font_normal = (XSSFFont)workbook.CreateFont();
                // font fail
                font_fail.FontName = "Arial";
                font_fail.FontHeightInPoints = 10;
                // font success
                font_success.FontName = "Arial";
                font_success.FontHeightInPoints = 10;
                // font call back
                font_callback.FontName = "Arial";
                font_callback.FontHeightInPoints = 10;
                // normal
                font_normal.FontName = "Arial";
                font_normal.FontHeightInPoints = 10;
                style_normal.SetFont(font_normal);
                font_fail.SetColor(color_fail);
                font_success.SetColor(color_success);
                font_callback.SetColor(color_callback);
                font_fail.IsBold = true;
                font_success.IsBold = true;
                font_callback.IsBold = true;
                // Fail
                var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
                style_fail.SetFont(font_fail);
                // Success
                var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                style_success.SetFont(font_success);
                // Call back
                var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
                style_callback.SetFont(font_callback);
                ISheet sheet = workbook.GetSheetAt(0); // Call campaign history
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.MMDDYYYY);
                cellStyle.SetFont(font_normal);
                int rowIdx = 3;
                IEnumerable<SFEReportDto> result = SFEDao.SFEPerformanceReport(dto);
                foreach (SFEReportDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.sfa_code); // sfa_code
                    row.GetCell(0).CellStyle = style_normal;
                    row.CreateCell(1).SetCellValue(item.pda_code); // pda_code
                    row.GetCell(1).CellStyle = style_normal;
                    row.CreateCell(2).SetCellValue(item.salesrep_name); // salesrep_name
                    row.GetCell(2).CellStyle = style_normal;
                    row.CreateCell(3).SetCellValue(item.position); // position
                    row.GetCell(3).CellStyle = style_normal;                    
                    row.CreateCell(4).SetCellValue(item.salesrep_active); // salesrep_active
                    row.GetCell(4).CellStyle = style_normal;
                    row.CreateCell(5).SetCellValue(item.salesrep_nonactive); // salesrep_nonactive
                    row.GetCell(5).CellStyle = style_normal;
                    row.CreateCell(6).SetCellValue(item.dif_per_rep); // dif_per_rep
                    row.GetCell(6).CellStyle = style_normal;
                    row.CreateCell(7).SetCellValue(item.call_month_actual); // call_month_actual
                    row.GetCell(7).CellStyle = style_normal;
                    row.CreateCell(8).SetCellValue(item.call_month_target); // call_month_target
                    row.GetCell(8).CellStyle = style_normal;
                    row.CreateCell(9).SetCellValue(item.call_month_percent); // call_month_percent
                    row.GetCell(9).CellStyle = style_normal;
                    row.CreateCell(10).SetCellValue(item.call_per_day); // call_per_day
                    row.GetCell(10).CellStyle = style_normal;
                    row.CreateCell(13).SetCellValue(item.call_effective_actual); // call_effective_actual
                    row.GetCell(13).CellStyle = style_normal;
                    row.CreateCell(14).SetCellValue(item.call_effective_target); // call_effective_target
                    row.GetCell(14).CellStyle = style_normal;
                    row.CreateCell(15).SetCellValue(item.call_effective_percent); // call_effective_percent
                    row.GetCell(15).CellStyle = style_normal;
                    row.CreateCell(17).SetCellValue(item.coverage_actual); // coverage_actual
                    row.GetCell(17).CellStyle = style_normal;
                    row.CreateCell(18).SetCellValue(item.coverage_target); // coverage_target
                    row.GetCell(18).CellStyle = style_normal;
                    row.CreateCell(19).SetCellValue(item.coverage_percent); // coverage_percent
                    row.GetCell(19).CellStyle = style_normal;
                    row.CreateCell(20).SetCellValue(item.active_ol_actual); // active_ol_actual
                    row.GetCell(20).CellStyle = style_normal;
                    row.CreateCell(21).SetCellValue(item.active_ol_target); // active_ol_target
                    row.GetCell(21).CellStyle = style_normal;
                    row.CreateCell(22).SetCellValue(item.active_ol_percent); // active_ol_percent
                    row.GetCell(22).CellStyle = style_normal;
                    row.CreateCell(25).SetCellValue(item.selling_brand); // selling_brand
                    row.GetCell(25).CellStyle = style_normal;
                    row.CreateCell(26).SetCellValue(item.sku_per_rep); // sku_per_rep
                    row.GetCell(26).CellStyle = style_normal;
                    row.CreateCell(27).SetCellValue(item.sku_per_order); // sku_per_order
                    row.GetCell(27).CellStyle = style_normal;
                    row.CreateCell(28).SetCellValue(item.value_per_order); // value_per_order
                    row.GetCell(28).CellStyle = style_normal;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }
    }
}