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
    public class RecordsServiceImpl : RecordsService
    {
        RecordsDao recordsDao = new RecordsDao();
        MonitorDao monitorDao = new MonitorDao();
        MySqlConnectionRecords recordIssabel = new MySqlConnectionRecords();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto SaveRecord(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
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

                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = 0;
                    Records46Dto record = null;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        record = new Records46Dto();
                        IRow row = sheet.GetRow(i);
                        record.calldate = Utils.GetFormattedCellValue(row.GetCell(0));
                        record.destination = Utils.GetFormattedCellValue(row.GetCell(3));
                        record.duration = Utils.GetFormattedCellValue(row.GetCell(9));
                        record.disposition = Utils.GetFormattedCellValue(row.GetCell(11));
                        record.uniqueid = Utils.GetFormattedCellValue(row.GetCell(14));
                        record.recordingfile = Utils.GetFormattedCellValue(row.GetCell(17));
                        record.cnum = Utils.GetFormattedCellValue(row.GetCell(18));
                        int j = recordsDao.SaveRecordFromServer46(record);
                        if (j >= 1)
                        {
                            rowCount += 1;
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
        /// Search Record
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<Records46Dto> SearchRecord(Records46Dto dto)
        {
            try
            {
                IEnumerable<Records46Dto> result = recordsDao.SearchRecord(dto);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IWorkbook RecordExport(Records46Dto dto)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "RecordsReport.xlsx";
            IEnumerable<Records46Dto> result = recordsDao.RecordExport(dto);
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
                int rowIdx = 2;
                int totalSuccessCall = 0;
                foreach (Records46Dto item in result)
                {
                    totalSuccessCall = item.PositiveCall;
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.cnum); // Agent number
                    sheet.GetRow(rowIdx).GetCell(0).CellStyle = style;
                    row.CreateCell(1).SetCellValue(item.PositiveCall); // PositiveCall
                    sheet.GetRow(rowIdx).GetCell(1).CellStyle = style;
                    row.CreateCell(2).SetCellValue(item.NoAnswer); // NoAnswer
                    sheet.GetRow(rowIdx).GetCell(2).CellStyle = style;
                    row.CreateCell(3).SetCellValue(item.Busy); // Busy
                    sheet.GetRow(rowIdx).GetCell(3).CellStyle = style;
                    row.CreateCell(4).SetCellValue(item.Answered); // Answered
                    sheet.GetRow(rowIdx).GetCell(4).CellStyle = style;
                    row.CreateCell(5).SetCellValue(item.SpecialCall); // SpecialCall
                    sheet.GetRow(rowIdx).GetCell(5).CellStyle = style;
                    row.CreateCell(6).SetCellValue(item.TotalCall); // TotalCall
                    sheet.GetRow(rowIdx).GetCell(6).CellStyle = style;
                    row.CreateCell(7).SetCellValue(totalSuccessCall); // Total Successfull Call 15s
                    sheet.GetRow(rowIdx).GetCell(7).CellStyle = style;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto FetchRecordIssabel(Records46Dto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int del = recordsDao.DeleteRecord46(dto);
                DataTable dt = recordIssabel.GetRecordingIssabel(dto);
                Records46Dto record = null;
                int rowCount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    record = new Records46Dto();
                    record.calldate = dt.Rows[i]["calldate"].ToString();
                    record.destination = dt.Rows[i]["dst"].ToString();
                    record.duration = dt.Rows[i]["duration"].ToString();
                    record.disposition = dt.Rows[i]["disposition"].ToString();
                    record.uniqueid = dt.Rows[i]["uniqueid"].ToString();
                    record.recordingfile = dt.Rows[i]["recordingfile"].ToString();
                    record.cnum = dt.Rows[i]["cnum"].ToString();
                    record.contracts = "DKSH";
                    rowCount += recordsDao.SaveRecordFromServer46(record);
                }

                DataTable dt_gigamed = recordIssabel.GetRecordingIssabelGigamed(dto);
                for (int j = 0; j < dt_gigamed.Rows.Count; j++)
                {
                    record = new Records46Dto();
                    record.calldate = dt_gigamed.Rows[j]["calldate"].ToString();
                    record.destination = dt_gigamed.Rows[j]["dst"].ToString();
                    record.duration = dt_gigamed.Rows[j]["duration"].ToString();
                    record.disposition = dt_gigamed.Rows[j]["disposition"].ToString();
                    record.uniqueid = dt_gigamed.Rows[j]["uniqueid"].ToString();
                    record.recordingfile = dt_gigamed.Rows[j]["recordingfile"].ToString();
                    record.cnum = dt_gigamed.Rows[j]["cnum"].ToString();
                    record.contracts = "GIGAMED";
                    rowCount += recordsDao.SaveRecordFromServer46(record);
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

        /// <summary>
        /// delete monitor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteMonitor(MonitorHistoryDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int del = monitorDao.DeleteMonitor(dto);
                if (del > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE, del);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.MESSAGE_NO_ROW_DELELE;
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// SearchTeleOrgchart
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<TeleSaleOrgChartDto> SearchTeleOrgchart(TeleSaleOrgChartDto param)
        {
            try
            {
                IEnumerable<TeleSaleOrgChartDto> result = recordsDao.SearchTeleOrgchart(param);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetTeleOrgChartDetail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public TeleSaleOrgChartDto GetTeleOrgChartDetail(TeleSaleOrgChartDto param)
        {
            TeleSaleOrgChartDto result = recordsDao.GetTeleOrgChartDetail(param);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SaveTeleSaleOrgChart(TeleSaleOrgChartDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IEnumerable<TeleSaleOrgChartDto> check_ext = recordsDao.CheckExtension(dto);
                if (dto.IsUpdate == false && check_ext.Count() > 0)
                {
                    string[] para = new string[2];
                    para[0] = dto.AgentExtension;
                    para[1] = dto.AgentContract;
                    result.code = Constant.ERROR;
                    result.message = string.Format(Constant.EXTENSION_EXISTED, para);
                }
                else
                {
                    int ins = recordsDao.SaveTeleSaleOrgChart(dto);
                    if (ins > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.NO_ROW_SAVE;
                    }
                }
                
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        public ResultMessageDto DeleteTeleSaleOrgChart(TeleSaleOrgChartDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int ins = recordsDao.DeleteTeleSaleOrgChart(dto);
                if (ins > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
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

        /// <summary>
        /// Talk Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook TalkTime(Records46Dto dto)
        {
            recordsDao.SaveTalkTimeDaily(dto);
            dto.calldate = Utils.StringDateFormat(dto.calldate);
            dto.calldate_to = Utils.StringDateFormat(dto.calldate_to);
            IEnumerable<Records46Dto> _talktime = recordsDao.TalkTime(dto);
            IWorkbook workbook = null;
            string fileNm = "TalkTime.xlsx";
            string filePath = @"D:\OSS\Dev_Publish\ExcelTemplate\";
            //string filePath = @"C:\Users\huynh.anh.pham\Documents\DKSH Operation\OBS\DKSH_HEC_IOS\DKSHApplication\ExcelTemplate\";
            using (FileStream file = new FileStream(filePath + fileNm, FileMode.Open, FileAccess.ReadWrite))
            {
                if (Constant.EXCEL_XLSX.Equals("xlsx"))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }
                ICellStyle title_cell = workbook.CreateCellStyle();
                title_cell.VerticalAlignment = VerticalAlignment.Center;
                var color_fail = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_success = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_callback = new XSSFColor(new byte[] { 51, 153, 255 });
                var font_fail = (XSSFFont)workbook.CreateFont();
                var font_success = (XSSFFont)workbook.CreateFont();
                var font_callback = (XSSFFont)workbook.CreateFont();
                font_fail.SetColor(color_fail);
                font_success.SetColor(color_success);
                font_callback.SetColor(color_callback);
                //font_fail.IsBold = true;
                font_success.IsBold = true;
                font_callback.IsBold = true;
                // Fail
                var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
                font_fail.FontName = "Calibri";
                font_fail.FontHeightInPoints = 10;
                font_fail.SetColor(color_fail);
                style_fail.SetFont(font_fail);
                // Success
                var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                font_success.FontName = "Calibri";
                font_success.FontHeightInPoints = 10;
                style_success.SetFont(font_success);
                // Call back
                var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
                font_callback.FontName = "Calibri";
                font_callback.FontHeightInPoints = 10;
                style_callback.SetFont(font_callback);
                var title_style = new XSSFColor(new byte[] { 12, 124, 243 });
                var title_font = (XSSFFont)workbook.CreateFont();
                title_font.SetColor(title_style);
                title_font.IsBold = true;
                title_font.FontHeightInPoints = 10;
                title_font.FontName = "Calibri";
                var style_title = (XSSFCellStyle)workbook.CreateCellStyle();
                style_title.VerticalAlignment = VerticalAlignment.Center;
                style_title.SetFont(title_font);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                ISheet sheet = workbook.GetSheetAt(0);
                int idxRow = 1;
                foreach (Records46Dto item in _talktime)
                {
                    IRow row = sheet.CreateRow(idxRow);
                    row.CreateCell(0).SetCellValue(item.calldate);
                    row.CreateCell(1).SetCellValue(item.duration);
                    row.CreateCell(2).SetCellValue(item.cnum);
                    row.CreateCell(3).SetCellValue(item.AgentName);
                    row.CreateCell(4).SetCellValue(item.AgentContract);
                    row.CreateCell(5).SetCellValue(item.LeaderName);
                    row.CreateCell(6).SetCellValue(item.GroupName);
                    row.CreateCell(7).SetCellValue(item.login);
                    row.CreateCell(8).SetCellValue(item.logoff);
                    row.CreateCell(9).SetCellValue(item.total_call);
                    //row.CreateCell(10).SetCellValue(item.zalo_call);
                    row.CreateCell(11).SetCellValue(item.actual_target + " %");
                    if (item.actual_target != null && decimal.Parse(item.actual_target) < 90)
                    {
                        row.GetCell(11).CellStyle = style_fail;
                    }
                    if (item.duration != null && item.duration.Substring(0, 2).Equals("00"))
                    {
                        row.GetCell(1).CellStyle = style_fail;
                    }
                    row.CreateCell(12).SetCellValue(item.active_customer);
                    idxRow += 1;
                }
                
            }
            return workbook;
        }
    }
}