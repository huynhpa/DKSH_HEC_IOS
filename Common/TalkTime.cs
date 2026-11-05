using DKSHApplication.DAO;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DKSHApplication.Common
{
    public class TalkTime : IJob
    {
        private static List<string> email = new List<string>()
        {
            "thai.huu.pham@gigamed.com.vn",
            "tuan.huu.huynh@gigamed.com.vn",
            "van.thibich.ngo@dksh.com",
            "diem.kieu.tran@dksh.com",
            "kim.thithien.tran@gigamed.com.vn",
            "trang.kieu.nguyen@gigamed.com.vn",
            "telesales.hcm.team.3@gigamed.com.vn",
            "telesales.hcm.team.4@gigamed.com.vn",
            "quynh.thianh.dinh@vimedimexbinhduong.com",
            "telesales.hcm.team.1@vimedimexbinhduong.com",
            "tele.sale4@vimedimexbinhduong.com",
            "telesales.hcm.team.8@gigamed.com.vn",
            "telesales.hcm.team.9@gigamed.com.vn",
            "telesales.hcm.team.5@gigamed.com.vn",
            "telesales.hcm.team.10@gigamed.com.vn",
            "telesales.hcm.team.2@gigamed.com.vn",
            "toan.van3.nguyen@gigamed.com.vn",
            "telesales.hcm.team.6@gigamed.com.vn",
            "telesales.hcm.team.7@gigamed.com.vn",
            "phuong.thilinh.pham@vimedimexbinhduong.com",
            "telesales.hcm.team.2@vimedimexbinhduong.com",
            "telesales.hcm.team.3@vimedimexbinhduong.com",
            "tele.sales.heca@dksh.com",
            "telesales.hcm.team.1@gigamed.com.vn",
            "telesales.team2.vn@dksh.com",
            "ccc.telemarketing.team@dksh.com",
            "ccc.telemarketing2.team@dksh.com",
            "telesales.team.vn@dksh.com",
            "telesales.team1.vn@dksh.com",
            "ccc.telemarketing3.team@dksh.com"
        };
        public void Execute(IJobExecutionContext context)
        {
            RecordsDao dao = new RecordsDao();
            string filePath = @"D:\OSS\Dev_Publish\ExcelTemplate\";
            try
            {
                string _datetime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                Records46Dto records = new Records46Dto()
                {
                    calldate = _datetime,
                    calldate_to = _datetime
                };

                ResultMessageDto rs = FetchRecordIssabel(records);
                dao.SaveTalkTimeDaily(records);
                IEnumerable<Records46Dto> _talktime = dao.TalkTime(records);
                IWorkbook workbook = null;
                string fileNm = "TalkTime.xlsx";                
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
                        row.CreateCell(10).SetCellValue(item.zalo_call);
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
                    string _excel = filePath + @"TalkTimeReport\TalkTime" + DateTime.Now.AddDays(-1).ToString("dd MMM yyyy") + ".xlsx";
                    if (!File.Exists(_excel))
                    {
                        FileStream fs = new FileStream(_excel, FileMode.CreateNew);
                        workbook.Write(fs);
                        fs.Close();
                    }
                    workbook.Close();
                    file.Close();
                    List<string> attachment = new List<string>();
                    string title = string.Format("Talk time {0}", DateTime.Now.AddDays(-1).ToString("dd MMM yyyy"));
                    string content = @"<p>Dear TeleSale Team</p><p>Please check the file in the attachment to get talk time by the agent on {0}</p>";
                    attachment.Add(_excel);
                    Utils.SendMailMultipleAttacment(title, string.Format(content, _datetime), "van.thibich.ngo@dksh.com", email, attachment);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("************** SAVE TALK TIME *****************\n");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt\n"));
                sb.Append(ex.Message);
                sb.Append("*************** SAVE TALK TIME ****************\n");
                File.AppendAllText(filePath + "log.txt", sb.ToString());
                sb.Clear();
            }
        }

        public ResultMessageDto FetchRecordIssabel(Records46Dto dto)
        {
            MySqlConnectionRecords recordIssabel = new MySqlConnectionRecords();
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                RecordsDao dao = new RecordsDao();
                int del = dao.DeleteRecord46(dto);
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
                    rowCount += dao.SaveRecordFromServer46(record);
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
                    rowCount += dao.SaveRecordFromServer46(record);
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
    }
}