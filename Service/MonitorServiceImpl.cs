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
    public class MonitorServiceImpl : MonitorService
    {
        MonitorDao monitorDao = new MonitorDao();
        //private static readonly string WELCOME = "LOI_CHAO";
        //private static readonly string CUSTOMER_RELATIONSHIP = "QUAN_HE_KHACH_HANG";
        //private static readonly string CONTENT = "NOI_DUNG";
        //private static readonly string FEELING = "CAM_XUC";
        //private static readonly string FINISH = "KET_THUC";
        public IEnumerable<CriterialEvaluationCallDto> CriterialEvaluationCall()
        {
            //CriterialData result = new CriterialData();
            //result.WELCOME = monitorDao.CriterialEvaluationCall(WELCOME);
            //result.CUSTOMER_RELATIONSHIP = monitorDao.CriterialEvaluationCall(CUSTOMER_RELATIONSHIP);
            //result.CONTENT = monitorDao.CriterialEvaluationCall(CONTENT);
            //result.FEELING = monitorDao.CriterialEvaluationCall(FEELING);
            //result.FINISH = monitorDao.CriterialEvaluationCall(FINISH);
            //return result;
            IEnumerable<CriterialEvaluationCallDto> result = monitorDao.CriterialEvaluationCall();
            return result;
        }

        /// <summary>
        /// Evaluation Mark
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationMarkDto> EvaluationMark()
        {
            IEnumerable<EvaluationMarkDto> result = monitorDao.EvaluationMark();
            return result;
        }

        /// <summary>
        /// Get Agent By Leader
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AgentByLeaderDto> GetAgentByLeader(MonitorHistoryDto dto)
        {
            IEnumerable<AgentByLeaderDto> result = monitorDao.GetAgentByLeader(dto);
            return result;
        }

        /// <summary>
        /// Leaders
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AgentByLeaderDto> GetLeaderList()
        {
            IEnumerable<AgentByLeaderDto> result = monitorDao.GetLeaderList().DistinctBy(x => x.LeaderExtension);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MonitorHistoryDto GetMonitorDetail(MonitorHistoryDto dto)
        {
            MonitorHistoryDto result = monitorDao.GetMonitorDetail(dto);
            result.EvaluationResult = monitorDao.GetEvaluationResultDetail(dto);
            return result;
        }

        /// <summary>
        /// Get Record Info
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MonitorHistoryDto GetRecordInfoToMonitor(MonitorHistoryDto dto)
        {
            MonitorHistoryDto result = new MonitorHistoryDto();
            Records46Dto record = monitorDao.GetRecordInfoToMonitor(dto);
            if (record != null)
            {
                MonitorLogDto log = new MonitorLogDto();
                log.phone_number = record.destination;
                log.agent_extension = record.cnum;
                log.call_date = record.calldate;
                log.duration = record.duration;
                log.monitor_extension = dto.MonitorBy;
                monitorDao.SaveMonitorLog(log);
                result.RecordID = record.RecordID;
                result.calldate = record.calldate;
                result.destination = record.destination;
                result.disposition = record.disposition;
                result.cnum = record.cnum;
                result.contracts = record.contracts;
                result.LeaderEmail = record.LeaderEmail;
                result.uniqueid = record.uniqueid;
                result.duration = record.duration;
                result.FullName = record.FullName;
                result.RecordLink = record.RecordLink;
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.RECORD_NOT_FOUND;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IWorkbook MonitorHistoryExport(MonitorHistoryDto dto)
        {            
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "MonitorReport.xlsx";
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
                ISheet sheetReport = workbook.GetSheetAt(0); // Report
                ISheet sheetTotal = workbook.GetSheetAt(1); // Total
                ISheet sheetDetail = workbook.GetSheetAt(2); // Detail
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                int rowIdx = 1;
                int cell = 9;
                // monitor history sheet Report
                IEnumerable<MonitorHistoryDto> monitorReport = monitorDao.SearchMonitorHistory(dto);
                int i = 0;
                foreach (MonitorHistoryDto item in monitorReport)
                {
                    monitorReport.ElementAt(i).EvaluationResult = monitorDao.GetTotalItemEvaluationResultByMonitorID(item);
                    i += 1;
                }
                foreach (MonitorHistoryDto item in monitorReport)
                {
                    IRow row = sheetReport.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.MonitorAgent); // Agent
                    row.CreateCell(1).SetCellValue(item.FullName); // FullName
                    row.CreateCell(2).SetCellValue(item.LeaderExtension + " - " + item.LeaderName); // LeaderExtension
                    row.CreateCell(3).SetCellValue(item.MonitorDateTime); // LeaderName
                    row.CreateCell(4).SetCellValue(item.MonitorBy + " - " + item.MonitorName); // MonitorName
                    row.CreateCell(5).SetCellValue(item.destination); // PhoneNumber
                    row.CreateCell(6).SetCellValue(item.calldate); // Calldate
                    row.CreateCell(7).SetCellValue(item.TotalMark); // total           
                    row.CreateCell(8).SetCellValue(item.TotalMark/ (int)item.Average); // average
                    foreach (MonitorDetailDto detail in item.EvaluationResult)
                    {
                        row.CreateCell(cell).SetCellValue(detail.Mark); // Mark
                        cell += 1;
                    }
                    
                    cell = 9;
                    rowIdx += 1;
                }
                // monitor history sheet Total
                rowIdx = 1;
                IEnumerable<MonitorHistoryDto> monitorTotal = monitorDao.SearchMonitorHistoryTotal(dto);
                foreach (MonitorHistoryDto item in monitorTotal)
                {
                    IRow row = sheetTotal.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.MonitorBy + " - " + item.LeaderName); // Leader
                    row.CreateCell(1).SetCellValue(item.TotalMonitorByAgent); // Tổng số Agent đã monitor
                    row.CreateCell(2).SetCellValue(item.TotalMonitor); // Tổng số call đã monitor
                    row.CreateCell(3).SetCellValue(item.MonitorDateTime); // Month
                    rowIdx += 1;
                }
                // monitor history sheet Detail
                rowIdx = 1;
                IEnumerable<AgentByLeaderDto> monitorDetail = monitorDao.SearchMonitorHistoryDetail(dto);
                foreach (AgentByLeaderDto item in monitorDetail)
                {
                    IRow row = sheetDetail.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.AgentExtension + " - " + item.AgentName); // Agent
                    row.CreateCell(1).SetCellValue(item.LeaderExtension + " - " + item.LeaderName); // Leader
                    row.CreateCell(2).SetCellValue(item.Campaign); // Campaign
                    row.CreateCell(3).SetCellValue(item.TotalMonitor); // Tổng số call đã monitor
                    row.CreateCell(4).SetCellValue(item.TotalMonitor > 0 ? (item.TotalMark / item.TotalMonitor) : item.TotalMark); // Tong diem trung binh
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }


        /// <summary>
        /// save evaluation call & evaluation detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto SaveEvaluationCall(EvaluationDataParam data, MonitorHistoryDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (dto.IsUpdate == null || dto.IsUpdate == false)
                {
                    dto.MonitorID = Guid.NewGuid();
                }               
                result.number = monitorDao.SaveEvaluationCall(dto);
                MonitorHistoryDto detail = null;
                int idx = 0;
                foreach(EvalMark item in data.EvalMark)
                {
                    detail = new MonitorHistoryDto();
                    detail.MonitorID = dto.MonitorID;
                    detail.RecordID = dto.RecordID;
                    detail.uniqueid = dto.uniqueid;                    
                    detail.CriterialID = FGuid.ToGuid(item.CriterialID);
                    detail.CreateBy = dto.CreateBy;
                    detail.UpdateBy = dto.UpdateBy;
                    detail.EvalID = item.Mark;
                    detail.IsUpdate = dto.IsUpdate;
                    detail.Note = item.Note;
                    idx += monitorDao.SaveEvaluationDetail(detail);
                }
                int udt = monitorDao.UpdateMonitorAverage(dto);
                // send mail to leader
                if (!string.IsNullOrEmpty(dto.LeaderEmail))
                {
                    string[] para = new string[4];
                    para[0] = dto.cnum + "-" + dto.FullName;
                    para[1] = dto.TotalMark.ToString();
                    para[2] = dto.destination;
                    para[3] = dto.calldate;
                    SendMailDto sm = new SendMailDto()
                    {
                        recipient = dto.LeaderEmail,
                        title = string.Format(Constant.EMAIL_TITLE_MONITOR, dto.cnum + "-" + dto.FullName),
                        content = string.Format(Constant.EMAIL_CONTENT_MONITOR, para)
                    };
                    Utils.SendMailMultipleAttachment(sm);
                }
                
                if (result.number > 0)
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
        /// Search Monitor History
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<MonitorHistoryDto> SearchMonitorHistory(MonitorHistoryDto dto)
        {
            IEnumerable<MonitorHistoryDto> result = monitorDao.SearchMonitorHistory(dto);
            int i = 0;
            foreach (MonitorHistoryDto item in result)
            {
                result.ElementAt(i).EvaluationResult = monitorDao.GetTotalItemEvaluationResultByMonitorID(item);
                i += 1;
            }
            return result;
        }
    }
}