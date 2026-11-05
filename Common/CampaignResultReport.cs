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
using System.Web;

namespace DKSHApplication.Common
{
    public class CampaignResultReport : IJob
    {
        SearchCallDao searchCallDao = new SearchCallDao();
        QuestionDao questionDao = new QuestionDao();
        AnswerDao answerDao = new AnswerDao();
        private static List<string> email = new List<string>()
        {
            "thai.huu.pham@gigamed.com.vn",
            "kim.thithien.tran@dksh.com",
            "diem.kieu.tran@dksh.com",
            "tuan.huu.huynh@gigamed.com.vn",
            "van.thibich.ngo@dksh.com",
            "ngoc.hong.luong@gigamed.com.vn"
        };
        public void Execute(IJobExecutionContext context)
        {
            ResultMessageDto result = new ResultMessageDto();
            string fileExport = @"D:\OBS\DKSH_HEC_IOS\CampaignResultReport\" + DateTime.Now.ToString("dd-MM-yyyy"); 
            // HttpContext.Current.Server.MapPath("~/ExcelTemplate/CampaignResultReport/") + DateTime.Now.ToString("dd-MM-yyyy");
            // @"E:\CampaignResultReport\" + DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(fileExport))
            {
                Directory.CreateDirectory(fileExport);
            }
            
            // get campaigns is active
            StringBuilder sb = new StringBuilder();
            IEnumerable<ServiceTypeDto> cpl = searchCallDao.GetCampaignList().OrderByDescending(z => z.ServiceName);
            foreach (ServiceTypeDto item in cpl)
            {
                sb.Append(item.ID);
                sb.Append(",");
            }
            SearchCallDto dto = new SearchCallDto()
            {
                ServiceIdList = sb.ToString(),
                FromDate = DateTime.Now.ToString("dd-MM-yyyy") + " 01:00 AM",
                ToDate = DateTime.Now.ToString("dd-MM-yyyy") + " 11:59 PM"
                //FromDate = "28-07-2021 01:00 AM",
                //ToDate = "28-07-2021 11:59 PM"
            };
            List<string> listPath = new List<string>();
            foreach (ServiceTypeDto service in cpl)
            {
                try
                {
                    IWorkbook workbook = null;
                    string filePath = @"D:\OBS\DKSH_HEC_IOS\CampaignResultReport\SurveyExportAutoDaily.xlsx";                    
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
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
                        font_fail.IsBold = true;
                        font_success.IsBold = true;
                        font_callback.IsBold = true;
                        // Fail
                        var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
                        font_fail.FontName = "Segoe UI";
                        font_fail.FontHeightInPoints = 9;
                        style_fail.SetFont(font_fail);
                        // Success
                        var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                        font_success.FontName = "Segoe UI";
                        font_success.FontHeightInPoints = 9;
                        style_success.SetFont(font_success);
                        // Call back
                        var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
                        font_callback.FontName = "Segoe UI";
                        font_callback.FontHeightInPoints = 9;
                        style_callback.SetFont(font_callback);
                        var title_style = new XSSFColor(new byte[] { 12, 124, 243 });
                        var title_font = (XSSFFont)workbook.CreateFont();
                        title_font.SetColor(title_style);
                        title_font.IsBold = true;
                        title_font.FontHeightInPoints = 9;
                        title_font.FontName = "Segoe UI";
                        var style_title = (XSSFCellStyle)workbook.CreateCellStyle();
                        style_title.VerticalAlignment = VerticalAlignment.Center;
                        style_title.SetFont(title_font);
                        ICellStyle cellStyle = workbook.CreateCellStyle();
                        cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);                        
                        QuestionDto questionDto = null;
                        int idxService = 0;
                        
                        questionDto = new QuestionDto();
                        questionDto.ServiceTypeID = service.ID;
                        ISheet sheet = workbook.CloneSheet(0);
                        workbook.SetSheetName(idxService + 1, service.ServiceName.Replace("/",""));
                        // Generate questions title
                        IEnumerable<QuestionDto> questions = questionDao.GetByServiceType(questionDto);
                        int idxques = 0;
                        int cell = 9;
                        AnswerDto answerDto = null;
                        List<Guid> AnswerID = new List<Guid>();
                        foreach (QuestionDto question in questions)
                        {
                            IRow row_Question = sheet.GetRow(0);
                            answerDto = new AnswerDto();
                            answerDto.QuestionID = question.ID;
                            int idxAnswer = 0;
                            IEnumerable<AnswerDto> answers = answerDao.GetByQuestion(answerDto);
                            foreach (AnswerDto answer in answers)
                            {
                                AnswerID.Add(answer.ID);
                                string answerText = "(" + answer.AnswerCode + " " + (string.IsNullOrEmpty(answer.ContentAnswer) ? answer.OrderIndex.ToString() : answer.ContentAnswer) + ") ";
                                row_Question.CreateCell(cell).SetCellValue(answerText);
                                row_Question.GetCell(cell).CellStyle = style_title;
                                idxAnswer += 1;
                                cell += 1;
                            }
                            idxques += 1;
                        }
                        // Generate calls & answers
                        int idxRow = 1;
                        SurveyVoteDto sv = null;
                        int cellIdx = 9;
                        dto.ServiceIdList = service.ID.ToString(); // select call by service type Id
                        IEnumerable<SearchCallDto> callByServiceType = searchCallDao.SearchCallEffective(dto);
                        if (callByServiceType != null && callByServiceType.Count() > 0)
                        {
                            foreach (SearchCallDto item in callByServiceType)
                            {
                                IRow row = sheet.CreateRow(idxRow);
                                row.CreateCell(0).SetCellValue(item.TalkTime); // Survey time
                                row.CreateCell(1).SetCellValue(item.Agent + "-" + item.EmployeeName);
                                // calling date
                                if (!string.IsNullOrEmpty(item.CallingDate))
                                {
                                    row.CreateCell(2).SetCellValue(new DateTime(int.Parse(item.CallingDate.Substring(6, 4)),
                                        int.Parse(item.CallingDate.Substring(3, 2)), int.Parse(item.CallingDate.Substring(0, 2)),
                                        int.Parse(item.CallingDate.Substring(11, 2)), int.Parse(item.CallingDate.Substring(14, 2)),
                                        int.Parse(item.CallingDate.Substring(17, 2))));
                                    row.GetCell(2).CellStyle = cellStyle;
                                }
                                row.CreateCell(3).SetCellValue(item.CustomerPhone); // Customer phone
                                row.CreateCell(4).SetCellValue(item.CustomerName); // Customer name
                                row.CreateCell(5).SetCellValue(item.CustomerCode); // Customer code
                                row.CreateCell(6).SetCellValue(item.Campaign); // Campaign
                                row.CreateCell(7).SetCellValue(item.ContactPerson); // Contact person
                                if (!string.IsNullOrEmpty(item.Status))
                                {
                                    if (item.Status.Equals("0"))
                                    {
                                        row.CreateCell(8).SetCellValue(Constant.UNSUCCESS); // unsuccess
                                        row.GetCell(8).CellStyle = style_fail;
                                    }
                                    else if (item.Status.Equals("1"))
                                    {
                                        row.CreateCell(8).SetCellValue(Constant.SUCCESS); // success
                                        row.GetCell(8).CellStyle = style_success;
                                    }
                                    else if (item.Status.Equals("2"))
                                    {
                                        row.CreateCell(8).SetCellValue(Constant.CALLBACK); // call back
                                        row.GetCell(8).CellStyle = style_callback;
                                    }
                                }
                                sv = new SurveyVoteDto();
                                sv.ID = (Guid)item.Id; // assign survey vote id
                                                        // get answert by survey vote
                                IEnumerable<SurveyResultReportDto> surveyResult = searchCallDao.GetAnswerResult(sv);
                                foreach (SurveyResultReportDto svrs in surveyResult)
                                {
                                    IRow rowSV = sheet.GetRow(idxRow);
                                    for (int i = 0; i < AnswerID.Count; i++)
                                    {
                                        if (svrs.AnswerID == AnswerID[i])
                                        {
                                            rowSV.CreateCell(cellIdx).SetCellValue(svrs.AnswerContent);
                                            break;
                                        }
                                        cellIdx += 1;
                                    }
                                    cellIdx = 9;
                                }
                                idxRow += 1;
                            }
                        }
                        idxService += 1;
                        dto.ServiceId = null;
                        workbook.RemoveSheetAt(0);
                        string excelFile = fileExport + @"\" + service.ServiceCode + "." + Constant.EXCEL_XLSX;
                        if (!File.Exists(excelFile))
                        {
                            FileStream fs = new FileStream(excelFile, FileMode.CreateNew);
                            workbook.Write(fs);
                            fs.Close();                            
                            listPath.Add(excelFile);
                        }                     
                        // add file to zip    
                        workbook.Close();
                        file.Close();
                    }
                }
                catch (Exception ex)
                {
                    Utils.WriteFileLog(fileExport, ex.Message);
                    result.code = Constant.ERROR;
                    result.message = ex.Message;
                    continue;
                }
            } // END FOREACH   
            // ZIP
            result.filePath = Utils.CreateZipFile(listPath.Count(), fileExport, listPath);
            List<string> attachment = new List<string>();
            List<string> ccMail = new List<string>();
            string title = string.Format("Campaign Result {0}", DateTime.Now.ToString("dd MMM yyyy"));
            //string title = string.Format("Campaign Result {0}", "28 Jul 2021");
            attachment.Add(result.filePath);
            Utils.SendMailMultipleAttacment(title, title, "huynh.anh.pham@dksh.com", email, attachment);
        }

    }
}