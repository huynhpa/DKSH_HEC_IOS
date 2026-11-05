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
    public class SearchCallServiceImpl : SearchCallService
    {
        SearchCallDao searchCallDao = new SearchCallDao();
        QuestionDao questionDao = new QuestionDao();
        AnswerDao answerDao = new AnswerDao();
        ServiceTypeDao serviceTypeDao = new ServiceTypeDao();
        CampaignDao campaignDao = new CampaignDao();
        public List<SurveyCollection> SurveyItems = new List<SurveyCollection>();
        private static List<string> Classic = new List<string> { "A", "B", "C", "D" };
        private static List<string> Group = new List<string> { "Share", "TL" };
        private string pathexport = string.Empty;
        /// <summary>
        /// Search Call
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> Search(SearchCallDto param)
        {
            IEnumerable<SearchCallDto> lst = searchCallDao.SearchCall(param);
            //IEnumerable<SearchCallDto> lst = searchCallDao.SearchCall_MultiRows(param);
            return lst;
        }

        /// <summary>
        /// Report Call
        /// </summary>
        /// <param name="searchcallList"></param>
        /// <returns></returns>
        public IWorkbook ReportCall(IEnumerable<SearchCallDto> searchcallList, bool? duplicateCall)
        {
            string fileExcel = "Report_Call.xlsx";
            if (duplicateCall != null && duplicateCall == true)
            {
                fileExcel = "DuplicateCall.xlsx";
            }
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileExcel;
            IWorkbook workbook;            
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                string extentsion = Path.GetExtension(filePath).Replace(".",string.Empty);
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
                var normal = (XSSFFont)workbook.CreateFont();
                normal.FontName = "Arial";
                normal.FontHeightInPoints = 9;

                font_fail.SetColor(color_fail);
                font_fail.FontName = "Arial";
                font_fail.FontHeightInPoints = 9;

                font_success.SetColor(color_success);
                font_success.FontName = "Arial";
                font_success.FontHeightInPoints = 9;

                font_callback.SetColor(color_callback);
                font_callback.FontName = "Arial";
                font_callback.FontHeightInPoints = 9;

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
                // normal
                var style_normal = (XSSFCellStyle)workbook.CreateCellStyle();
                style_normal.SetFont(normal);

                var date = (XSSFFont)workbook.CreateFont();
                date.FontName = "Arial";
                date.FontHeightInPoints = 9;
                ISheet sheet = workbook.GetSheet("Report Call"); // Report Call
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.SetFont(date);
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYYHHMMSS_slash);
                int rowIdx = 1;
                foreach (SearchCallDto item in searchcallList)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustomerCode); // Cust code
                    row.GetCell(0).CellStyle = style_normal;
                    row.CreateCell(1).SetCellValue(item.VNZ0Code); // VNZ0Code
                    row.GetCell(1).CellStyle = style_normal;
                    row.CreateCell(2).SetCellValue(item.CustomerName); // Customer Name
                    row.GetCell(2).CellStyle = style_normal;
                    //row.CreateCell(3).SetCellValue(item.CustomerPhone); // Phone Number
                    //row.GetCell(3).CellStyle = style_normal;
                    row.CreateCell(3).SetCellValue(item.EmployeeName); // Employee Name
                    row.GetCell(3).CellStyle = style_normal;
                    row.CreateCell(4).SetCellValue(item.SalemanCode); // Saleman Code
                    row.GetCell(4).CellStyle = style_normal;
                    row.CreateCell(5).SetCellValue(item.Agent); // Agent
                    row.GetCell(5).CellStyle = style_normal;
                    // Calling Date
                    if (!string.IsNullOrEmpty(item.CallingDate))
                    {
                        row.CreateCell(6).SetCellValue(new DateTime(int.Parse(item.CallingDate.Substring(6, 4)),
                            int.Parse(item.CallingDate.Substring(3, 2)), int.Parse(item.CallingDate.Substring(0, 2)),
                            int.Parse(item.CallingDate.Substring(11, 2)), int.Parse(item.CallingDate.Substring(14, 2)),
                            int.Parse(item.CallingDate.Substring(17, 2))));
                        row.GetCell(6).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(6).SetCellValue(item.CallingDate);
                    }
                    // Start time
                    if (!string.IsNullOrEmpty(item.StartTime))
                    {
                        row.CreateCell(7).SetCellValue(new DateTime(int.Parse(item.StartTime.Substring(6, 4)),
                            int.Parse(item.StartTime.Substring(3, 2)), int.Parse(item.StartTime.Substring(0, 2)),
                            int.Parse(item.StartTime.Substring(11, 2)), int.Parse(item.StartTime.Substring(14, 2)),
                            int.Parse(item.StartTime.Substring(17, 2))));
                        row.GetCell(7).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(item.StartTime);
                        row.GetCell(7).CellStyle = cellStyle;
                    }
                    // End time
                    if (!string.IsNullOrEmpty(item.EndTime))
                    {
                        row.CreateCell(8).SetCellValue(new DateTime(int.Parse(item.EndTime.Substring(6, 4)),
                            int.Parse(item.EndTime.Substring(3, 2)), int.Parse(item.EndTime.Substring(0, 2)),
                            int.Parse(item.EndTime.Substring(11, 2)), int.Parse(item.EndTime.Substring(14, 2)),
                            int.Parse(item.EndTime.Substring(17, 2))));
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(8).SetCellValue(item.EndTime);
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    row.CreateCell(9).SetCellValue(item.TalkTime); // Talk time
                    row.GetCell(9).CellStyle = style_normal;
                    // Call Back Date
                    if (!string.IsNullOrEmpty(item.CallBackDate))
                    {
                        row.CreateCell(10).SetCellValue(new DateTime(int.Parse(item.CallBackDate.Substring(6, 4)),
                            int.Parse(item.CallBackDate.Substring(3, 2)), int.Parse(item.CallBackDate.Substring(0, 2))));
                        row.GetCell(10).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(10).SetCellValue(item.CallBackDate);
                        row.GetCell(10).CellStyle = cellStyle;
                    }
                    row.CreateCell(11).SetCellValue(item.ProjectName); // Project name
                    row.GetCell(11).CellStyle = style_normal;
                    row.CreateCell(12).SetCellValue(item.UpdateBy); // Update By
                    row.GetCell(12).CellStyle = style_normal;
                    // Update Date
                    if (!string.IsNullOrEmpty(item.UpdateDate))
                    {
                        row.CreateCell(13).SetCellValue(new DateTime(int.Parse(item.UpdateDate.Substring(6, 4)),
                            int.Parse(item.UpdateDate.Substring(3, 2)), int.Parse(item.UpdateDate.Substring(0, 2))));
                        row.GetCell(13).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(13).SetCellValue(item.UpdateDate);
                        row.GetCell(13).CellStyle = cellStyle;
                    }
                    if (!string.IsNullOrEmpty(item.Status))
                    {
                        if (item.Status.Equals("0"))
                        {
                            row.CreateCell(14).SetCellValue(Constant.UNSUCCESS); // unsuccess
                            row.GetCell(14).CellStyle = style_fail;                            
                        }
                        else if (item.Status.Equals("1"))
                        {
                            row.CreateCell(14).SetCellValue(Constant.SUCCESS); // success
                            row.GetCell(14).CellStyle = style_success;                            
                        }
                        else if (item.Status.Equals("2"))
                        {
                            row.CreateCell(14).SetCellValue(Constant.CALLBACK); // call back
                            row.GetCell(14).CellStyle = style_callback;                           
                        }
                    }
                    if (!string.IsNullOrEmpty(item.CallingDate))
                    {
                        row.CreateCell(15).SetCellValue(item.CallingDate.Substring(3, 7));
                        row.GetCell(15).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(15).SetCellValue(item.CallingDate);
                        row.GetCell(15).CellStyle = cellStyle;
                    }
                    row.CreateCell(16).SetCellValue(item.ProjectName); // Project name
                    row.GetCell(16).CellStyle = style_normal;
                    row.CreateCell(17).SetCellValue(item.SO); // SO
                    row.GetCell(17).CellStyle = style_normal;
                    row.CreateCell(18).SetCellValue(item.contracts); // contract
                    row.GetCell(18).CellStyle = style_normal;
                    row.CreateCell(19).SetCellValue(item.VisitID); // visit id
                    row.GetCell(19).CellStyle = style_normal;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// Get Campaign List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceTypeDto> GetCampaignList()
        {
            IEnumerable<ServiceTypeDto> result = searchCallDao.GetCampaignList();
            return result;
        }

        /// <summary>
        /// Export Campaign
        /// </summary>
        /// <param name="searchCall"></param>
        /// <returns></returns>
        public IWorkbook SurveyResult(SearchCallDto dto)
        {
            IWorkbook workbook = null;
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "SurveyExport.xlsx";
            try
            {
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
                    font_fail.FontName = "Arial";
                    font_fail.FontHeightInPoints = 9;
                    style_fail.SetFont(font_fail);
                    // Success
                    var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                    font_success.FontName = "Arial";
                    font_success.FontHeightInPoints = 9;
                    style_success.SetFont(font_success);
                    // Call back
                    var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
                    font_callback.FontName = "Arial";
                    font_callback.FontHeightInPoints = 9;
                    style_callback.SetFont(font_callback);
                    var title_style = new XSSFColor(new byte[] { 12, 124, 243 });
                    var title_font = (XSSFFont)workbook.CreateFont();
                    title_font.SetColor(title_style);
                    title_font.IsBold = true;
                    title_font.FontHeightInPoints = 9;
                    title_font.FontName = "Arial";
                    var style_title = (XSSFCellStyle)workbook.CreateCellStyle();
                    style_title.VerticalAlignment = VerticalAlignment.Center;
                    style_title.SetFont(title_font);
                    ICellStyle cellStyle = workbook.CreateCellStyle();
                    cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);

                    IEnumerable<SearchCallDto> services = searchCallDao.GetDistinctSurveyFromSearchCall(dto);
                    QuestionDto questionDto = null;
                    int idxService = 0;
                    foreach (SearchCallDto service in services)
                    {
                        questionDto = new QuestionDto();
                        questionDto.ServiceTypeID = service.Id;
                        ISheet sheet = workbook.CloneSheet(0);
                        workbook.SetSheetName(idxService + 1, string.IsNullOrEmpty(service.ServiceName) ? service.Campaign : service.ServiceName);                     
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
                        dto.ServiceIdList = service.Id.ToString(); // select call by service type Id   
                        IEnumerable<SearchCallDto> callByServiceType = searchCallDao.SearchCallEffective(dto);
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
                        idxService += 1;
                        dto.ServiceId = null;
                    }
                    workbook.RemoveSheetAt(0);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {                
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return workbook;
        }


        //public IWorkbook ExportEffectiveCampaign(SearchCallDto dto)
        //{
        //    IWorkbook workbook = null;
        //    var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "SurveyExport.xlsx";
        //    try
        //    {
        //        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
        //        {
        //            if (Constant.EXCEL_XLSX.Equals("xlsx"))
        //            {
        //                workbook = new XSSFWorkbook(file);
        //            }
        //            else
        //            {
        //                throw new Exception("This format is not supported");
        //            }
        //            ICellStyle title_cell = workbook.CreateCellStyle();
        //            title_cell.VerticalAlignment = VerticalAlignment.Center;
        //            var color_fail = new XSSFColor(new byte[] { 255, 0, 0 });
        //            var color_success = new XSSFColor(new byte[] { 51, 204, 51 });
        //            var color_callback = new XSSFColor(new byte[] { 51, 153, 255 });
        //            var font_fail = (XSSFFont)workbook.CreateFont();
        //            var font_success = (XSSFFont)workbook.CreateFont();
        //            var font_callback = (XSSFFont)workbook.CreateFont();
        //            font_fail.SetColor(color_fail);
        //            font_success.SetColor(color_success);
        //            font_callback.SetColor(color_callback);
        //            font_fail.IsBold = true;
        //            font_success.IsBold = true;
        //            font_callback.IsBold = true;
        //            // Fail
        //            var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
        //            font_fail.FontName = "Arial";
        //            font_fail.FontHeightInPoints = 9;
        //            style_fail.SetFont(font_fail);
        //            // Success
        //            var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
        //            font_success.FontName = "Arial";
        //            font_success.FontHeightInPoints = 9;
        //            style_success.SetFont(font_success);
        //            // Call back
        //            var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
        //            font_callback.FontName = "Arial";
        //            font_callback.FontHeightInPoints = 9;
        //            style_callback.SetFont(font_callback);
        //            var title_style = new XSSFColor(new byte[] { 12, 124, 243 });
        //            var title_font = (XSSFFont)workbook.CreateFont();
        //            title_font.SetColor(title_style);
        //            title_font.IsBold = true;
        //            title_font.FontHeightInPoints = 9;
        //            title_font.FontName = "Arial";
        //            var style_title = (XSSFCellStyle)workbook.CreateCellStyle();
        //            style_title.VerticalAlignment = VerticalAlignment.Center;
        //            style_title.SetFont(title_font);
        //            ICellStyle cellStyle = workbook.CreateCellStyle();
        //            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
        //            IEnumerable<SearchCallDto> resultCampaign = searchCallDao.SearchEffectiveCampaignHistory(dto);
        //            QuestionDto questionDto = null;
        //            int idxService = 0;
        //            foreach (SearchCallDto service in resultCampaign)
        //            {
        //                questionDto = new QuestionDto();
        //                questionDto.ServiceTypeID = service.Id;
        //                ISheet sheet = workbook.CloneSheet(0);
        //                workbook.SetSheetName(idxService + 1, string.IsNullOrEmpty(service.ServiceName) ? service.Campaign : service.ServiceName);
        //                // Generate questions title
        //                IEnumerable<QuestionDto> questions = questionDao.GetByServiceType(questionDto);
        //                int idxques = 0;
        //                int cell = 9;
        //                AnswerDto answerDto = null;
        //                List<Guid> AnswerID = new List<Guid>();
        //                foreach (QuestionDto question in questions)
        //                {
        //                    IRow row_Question = sheet.GetRow(0);
        //                    answerDto = new AnswerDto();
        //                    answerDto.QuestionID = question.ID;
        //                    int idxAnswer = 0;
        //                    IEnumerable<AnswerDto> answers = answerDao.GetByQuestion(answerDto);
        //                    foreach (AnswerDto answer in answers)
        //                    {
        //                        AnswerID.Add(answer.ID);
        //                        string answerText = "(" + answer.AnswerCode + " " + (string.IsNullOrEmpty(answer.ContentAnswer) ? answer.OrderIndex.ToString() : answer.ContentAnswer) + ") ";
        //                        row_Question.CreateCell(cell).SetCellValue(answerText);
        //                        row_Question.GetCell(cell).CellStyle = style_title;
        //                        idxAnswer += 1;
        //                        cell += 1;
        //                    }
        //                    idxques += 1;
        //                }
        //                // Generate calls & answers
        //                int idxRow = 1;
        //                SurveyVoteDto sv = null;
        //                int cellIdx = 9;
        //                dto.ServiceIdList = service.Id.ToString(); // select call by service type Id   
        //                IEnumerable<SearchCallDto> callByServiceType = searchCallDao.SearchCall(dto);
        //                foreach (SearchCallDto item in callByServiceType)
        //                {
        //                    IRow row = sheet.CreateRow(idxRow);
        //                    row.CreateCell(0).SetCellValue(item.TalkTime); // Survey time
        //                    row.CreateCell(1).SetCellValue(item.Agent + "-" + item.EmployeeName);
        //                    // calling date
        //                    if (!string.IsNullOrEmpty(item.CallingDate))
        //                    {
        //                        row.CreateCell(2).SetCellValue(new DateTime(int.Parse(item.CallingDate.Substring(6, 4)),
        //                            int.Parse(item.CallingDate.Substring(3, 2)), int.Parse(item.CallingDate.Substring(0, 2)),
        //                            int.Parse(item.CallingDate.Substring(11, 2)), int.Parse(item.CallingDate.Substring(14, 2)),
        //                            int.Parse(item.CallingDate.Substring(17, 2))));
        //                        row.GetCell(2).CellStyle = cellStyle;
        //                    }
        //                    row.CreateCell(3).SetCellValue(item.CustomerPhone); // Customer phone
        //                    row.CreateCell(4).SetCellValue(item.CustomerName); // Customer name
        //                    row.CreateCell(5).SetCellValue(item.CustomerCode); // Customer code
        //                    row.CreateCell(6).SetCellValue(item.Campaign); // Campaign
        //                    row.CreateCell(7).SetCellValue(item.ContactPerson); // Contact person
        //                    if (!string.IsNullOrEmpty(item.Status))
        //                    {
        //                        if (item.Status.Equals("0"))
        //                        {
        //                            row.CreateCell(8).SetCellValue(Constant.UNSUCCESS); // unsuccess
        //                            row.GetCell(8).CellStyle = style_fail;
        //                        }
        //                        else if (item.Status.Equals("1"))
        //                        {
        //                            row.CreateCell(8).SetCellValue(Constant.SUCCESS); // success
        //                            row.GetCell(8).CellStyle = style_success;
        //                        }
        //                        else if (item.Status.Equals("2"))
        //                        {
        //                            row.CreateCell(8).SetCellValue(Constant.CALLBACK); // call back
        //                            row.GetCell(8).CellStyle = style_callback;
        //                        }
        //                    }
        //                    sv = new SurveyVoteDto();
        //                    sv.ID = (Guid)item.Id; // assign survey vote id
        //                    // get answert by survey vote
        //                    IEnumerable<SurveyResultReportDto> surveyResult = searchCallDao.GetAnswerResult(sv);
        //                    foreach (SurveyResultReportDto svrs in surveyResult)
        //                    {
        //                        IRow rowSV = sheet.GetRow(idxRow);
        //                        for (int i = 0; i < AnswerID.Count; i++)
        //                        {
        //                            if (svrs.AnswerID == AnswerID[i])
        //                            {
        //                                rowSV.CreateCell(cellIdx).SetCellValue(svrs.AnswerContent);
        //                                break;
        //                            }
        //                            cellIdx += 1;
        //                        }
        //                        cellIdx = 9;
        //                    }
        //                    idxRow += 1;
        //                }
        //                idxService += 1;
        //                dto.ServiceId = null;
        //            }
        //            workbook.RemoveSheetAt(0);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    return workbook;
        //}
        /// <summary>
        /// EffectiveCampaignReport
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public IWorkbook ExportEffectiveCampaign(SearchCallDto json)
        {
            string fileExcel = "EffectiveCampaignReport.xlsx";
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileExcel;
            IWorkbook workbook;
            try
            {
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
                    font_fail.FontName = "Arial";
                    font_fail.FontHeightInPoints = 9;
                    style_fail.SetFont(font_fail);
                    // Success
                    var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                    font_success.FontName = "Arial";
                    font_success.FontHeightInPoints = 9;
                    style_success.SetFont(font_success);
                    // Call back
                    var style_callback = (XSSFCellStyle)workbook.CreateCellStyle();
                    font_callback.FontName = "Arial";
                    font_callback.FontHeightInPoints = 9;
                    style_callback.SetFont(font_callback);
                    var title_style = new XSSFColor(new byte[] { 12, 124, 243 });
                    var title_font = (XSSFFont)workbook.CreateFont();
                    title_font.SetColor(title_style);
                    title_font.IsBold = true;
                    title_font.FontHeightInPoints = 9;
                    title_font.FontName = "Arial";
                    var style_title = (XSSFCellStyle)workbook.CreateCellStyle();
                    style_title.VerticalAlignment = VerticalAlignment.Center;
                    style_title.SetFont(title_font);
                    ICellStyle cell_date = workbook.CreateCellStyle();
                    cell_date.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int idxItem = 0;
                    int idxRow = 1;
                    int idxAnsw = 1;
                    IEnumerable<SearchCallDto> resultCampaign = searchCallDao.SearchEffectiveCampaignHistory(json);
                    foreach (SearchCallDto item in resultCampaign)
                    {
                        //IEnumerable<SurveyResultDto> exists = searchCallDao.CheckServiceTypeWhetherSurveyResult(item);
                        //if (exists.Count() > 0)
                        //{
                        AnswerDto a = new AnswerDto()
                        {
                            ServiceTypeID = item.ServiceTypeID
                        };
                        IEnumerable<AnswerDto> answers = answerDao.GetQuestionAnswerByServiceType(a);
                        foreach (AnswerDto ans in answers)
                        {
                            IRow row = sheet.CreateRow(idxRow);
                            if (item.Status.Equals("0"))
                            {
                                row.CreateCell(0).SetCellValue(Constant.UNSUCCESS); // unsuccess
                                row.GetCell(0).CellStyle = style_fail;
                            }
                            else if (item.Status.Equals("1"))
                            {
                                row.CreateCell(0).SetCellValue(Constant.SUCCESS); // success
                                row.GetCell(0).CellStyle = style_success;
                            }
                            else if (item.Status.Equals("2"))
                            {
                                row.CreateCell(0).SetCellValue(Constant.CALLBACK); // call back
                                row.GetCell(0).CellStyle = style_callback;
                            }
                            row.CreateCell(1).SetCellValue(item.SalemanCode);
                            row.CreateCell(2).SetCellValue(item.EmployeeName);
                            if (!string.IsNullOrEmpty(item.CallingDate))
                            {
                                row.CreateCell(3).SetCellValue(new DateTime(int.Parse(item.CallingDate.Substring(6, 4)),
                                    int.Parse(item.CallingDate.Substring(3, 2)), int.Parse(item.CallingDate.Substring(0, 2)),
                                    int.Parse(item.CallingDate.Substring(11, 2)), int.Parse(item.CallingDate.Substring(14, 2)),
                                    int.Parse(item.CallingDate.Substring(17, 2))));
                                row.GetCell(3).CellStyle = cell_date;
                            }
                            row.CreateCell(4).SetCellValue(item.TalkTime);
                            row.CreateCell(5).SetCellValue(item.CustomerCode);
                            row.CreateCell(6).SetCellValue(item.CustomerName);
                            row.CreateCell(7).SetCellValue(item.ProjectName);
                            row.CreateCell(8).SetCellValue(item.ServiceCode);
                            row.CreateCell(9).SetCellValue(item.Campaign);
                            row.CreateCell(10).SetCellValue(ans.QuestionCode);
                            row.CreateCell(11).SetCellValue(ans.QuestionTextNonHTML);
                            row.CreateCell(12).SetCellValue(ans.AnswerCode);
                            row.CreateCell(13).SetCellValue(ans.AnswerTypeCode + " - " + ans.AnswerTypeName);
                            row.CreateCell(14).SetCellValue(ans.ContentAnswer);
                            SurveyVoteDto sv = new SurveyVoteDto()
                            {
                                ID = (Guid)item.SurveyVoteID,
                                AnswerID = ans.ID
                            };
                            IEnumerable<SurveyResultReportDto> surveyResult = searchCallDao.GetSurveyAnswerResultByAnswer(sv);
                            if (surveyResult != null && surveyResult.Count() > 0)
                            {
                                if (ans.AnswerTypeCode.Equals("3") || ans.AnswerTypeCode.Equals("5"))
                                {
                                    row.CreateCell(16).SetCellValue(surveyResult.ElementAt(0).AnswerContent);
                                }
                                else
                                {
                                    row.CreateCell(15).SetCellValue(surveyResult.ElementAt(0).AnswerContent);
                                }
                            }
                            idxAnsw += 1;
                            idxRow += 1;
                        }
                        idxAnsw = 1;
                        idxItem += 1;
                    }
                } // end foreach
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return workbook;
        }


        /// <summary>
        /// Get Project List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectDto> GetProjectList()
        {
            IEnumerable<ProjectDto> result = searchCallDao.GetProjectList();
            return result;
        }

        /// <summary>
        /// Duplicate Call
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IWorkbook DuplicateCall(SearchCallDto json)
        {
            IWorkbook workbook;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "DuplicateCall.xlsx";
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
                var font_fail = (XSSFFont)workbook.CreateFont();
                var font_success = (XSSFFont)workbook.CreateFont();
                var font_normal = (XSSFFont)workbook.CreateFont();
                font_fail.SetColor(color_fail);
                font_fail.FontName = "Arial";
                font_fail.FontHeightInPoints = 9;
                font_success.SetColor(color_success);
                font_success.FontName = "Arial";
                font_success.FontHeightInPoints = 9;
                font_normal.FontName = "Arial";
                font_normal.FontHeightInPoints = 9;
                // Fail
                var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
                style_fail.SetFont(font_fail);
                // Success
                var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                style_success.SetFont(font_success);
                ISheet sheet_Summary = workbook.GetSheetAt(0); // Summary duplicate call
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                int rowIdx = 1;
                IEnumerable<SearchCallDto> summary = searchCallDao.SummaryDuplicateCall(json);
                foreach (SearchCallDto item in summary)
                {
                    IRow row = sheet_Summary.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustomerCode); // Cust code
                    row.CreateCell(1).SetCellValue(item.CustomerName); // Customer Name
                    row.CreateCell(2).SetCellValue(item.CustomerName); // Survey date
                    row.CreateCell(3).SetCellValue(item.CustomerPhone); // Phone Number
                    row.CreateCell(4).SetCellValue(item.SalemanCode); // Saleman code
                    row.CreateCell(5).SetCellValue(item.EmployeeName); // Employee Name
                    row.CreateCell(6).SetCellValue(item.Agent); // Agent
                    row.CreateCell(7).SetCellValue(item.Campaign); // Campaign
                    row.CreateCell(8).SetCellValue(item.NumberDuplicate); // Number duplicate
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// Active Outlet Daily
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IWorkbook ActiveOutletDaily(SearchCallDto json)
        {
            IWorkbook workbook;
            json.Months = json.MonthOfYear;
            IEnumerable<ActiveOutletDailyDto> activeOutletDaily = searchCallDao.ActiveOutletDaily(json);
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "ActiveOutletDaily.xlsx";
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
                var font_fail = (XSSFFont)workbook.CreateFont();
                var font_success = (XSSFFont)workbook.CreateFont();
                var font_normal = (XSSFFont)workbook.CreateFont();
                font_fail.SetColor(color_fail);
                font_fail.FontName = "Arial";
                font_fail.FontHeightInPoints = 9;
                font_success.SetColor(color_success);
                font_success.FontName = "Arial";
                font_success.FontHeightInPoints = 9;
                font_normal.FontName = "Arial";
                font_normal.FontHeightInPoints = 9;
                // Fail
                var style_fail = (XSSFCellStyle)workbook.CreateCellStyle();
                style_fail.SetFont(font_fail);
                // Success
                var style_success = (XSSFCellStyle)workbook.CreateCellStyle();
                style_success.SetFont(font_success);
                ISheet sheet_ActiveOutlet = workbook.GetSheetAt(0); // Summary duplicate call
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                int rowIdx = 2;
                int sumAllShare = 0;
                double rate = 0;
                foreach(ActiveOutletDailyDto item in activeOutletDaily)
                {
                    IRow row = sheet_ActiveOutlet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.TeleTeam); // Tele team
                    row.CreateCell(1).SetCellValue(item.Extension + "-" + item.FullName); // FullName
                    row.CreateCell(2).SetCellValue(item.AllCustShare.ToString()); // AllCustShare
                    row.CreateCell(3).SetCellValue(item.AllCustClassA.ToString()); // Tổng số KH Class A
                    row.CreateCell(4).SetCellValue(item.CalledShareClassA.ToString()); // Đã gọi Class A
                    row.CreateCell(5).SetCellValue(item.AllCustClassB.ToString()); // Tổng số KH Class B
                    row.CreateCell(6).SetCellValue(item.CalledShareClassB.ToString()); // Đã gọi Class B
                    row.CreateCell(7).SetCellValue(item.AllCustClassC.ToString()); // Tổng số KH Class C
                    row.CreateCell(8).SetCellValue(item.CalledShareClassC.ToString()); // Đã gọi Class C
                    row.CreateCell(9).SetCellValue(item.AllCustClassD.ToString()); // Tổng số KH Class D
                    row.CreateCell(10).SetCellValue(item.CalledShareClassD.ToString()); // Đã gọi Class D
                    row.CreateCell(11).SetCellValue(item.NotYetCallShare.ToString()); // Chưa gọi
                    row.CreateCell(12).SetCellValue(item.RateShare.ToString()); // Rate share
                    row.CreateCell(13).SetCellValue(item.AllCustTL.ToString()); // Tổng số KH TL
                    row.CreateCell(14).SetCellValue(item.AllCustTLClassA.ToString()); // Tổng số KH TL Class A
                    row.CreateCell(15).SetCellValue(item.CalledTLClassA.ToString()); // Đã gọi TL A
                    row.CreateCell(16).SetCellValue(item.AllCustTLClassB.ToString()); // Tổng số KH Class B
                    row.CreateCell(17).SetCellValue(item.CalledTLClassB.ToString()); //  Đã gọi TL B
                    row.CreateCell(18).SetCellValue(item.AllCustTLClassC.ToString()); //  Tổng số KH Class C
                    row.CreateCell(19).SetCellValue(item.CalledTLClassC.ToString()); //  Đã gọi TL C
                    row.CreateCell(20).SetCellValue(item.AllCustTLClassD.ToString()); //  Tổng số KH Class D
                    row.CreateCell(21).SetCellValue(item.CalledTLClassD.ToString()); //  Đã gọi TL D
                    row.CreateCell(22).SetCellValue(item.NotYetCallTL.ToString()); // Chưa gọi
                    row.CreateCell(23).SetCellValue(item.RateTL.ToString()); // rate TL
                    rate = (double)(item.NotYetCallShare + item.NotYetCallTL) / (double)(item.AllCustShare + item.AllCustTL);
                    row.CreateCell(24).SetCellValue(rate.ToString()); // % Chưa gọi Share + TL / Tổng KH Share + TL 
                    // SUM Grand Total
                    sumAllShare += (int)item.AllCustShare;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// upload customer
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public ResultMessageDto UploadCustomer(HttpPostedFileBase aFile, SearchCallParam dto)
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
                    // delete data customer whether exists
                    int del = searchCallDao.DeleteCustomerClass(dto);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = 0;
                    CustomerClassDto customer = null;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        customer = new CustomerClassDto();
                        IRow row = sheet.GetRow(i);
                        customer.CustCode = Utils.GetFormattedCellValue(row.GetCell(0));
                        customer.CustName = Utils.GetFormattedCellValue(row.GetCell(1));
                        customer.Province = Utils.GetFormattedCellValue(row.GetCell(2));
                        customer.Region = Utils.GetFormattedCellValue(row.GetCell(3));
                        customer.Channel = Utils.GetFormattedCellValue(row.GetCell(4));
                        customer.SaleRep = Utils.GetFormattedCellValue(row.GetCell(5));
                        customer.Extension = Utils.GetFormattedCellValue(row.GetCell(6));
                        customer.Classic = Utils.GetFormattedCellValue(row.GetCell(7));
                        customer.Groups = Utils.GetFormattedCellValue(row.GetCell(8));
                        customer.MonthOfYear = dto.MonthOfYear;
                        int j = searchCallDao.SaveCustomerClass(customer);
                        if (j >= 1)
                        {
                            rowCount += 1;
                        }
                    }
                    ResultMessageDto rs = ProcessActiveOutletDaily(dto);
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
        /// Update call by agent count from SurveyVote
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto ProcessActiveOutletDaily(SearchCallParam dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int process = searchCallDao.ProcessActiveOutletDaily(dto);
                foreach (string gr in Group)
                {
                    foreach (string cl in Classic)
                    {
                        dto.Group = gr;
                        dto.Classic = cl;
                        IEnumerable<ActiveOutletDailyDto> activeOulet = searchCallDao.CountActiveOutletDaily(dto);
                        int up = 0;
                        foreach(ActiveOutletDailyDto ac in activeOulet)
                        {
                            ac.Groups = gr;
                            ac.Classic = cl;
                            up += searchCallDao.UpdateCalledActiveOutlet(ac);
                        }
                    }
                }
                SearchCallDto json = new SearchCallDto();
                json.Months = dto.MonthOfYear;
                IEnumerable<ActiveOutletDailyDto> acd = searchCallDao.ActiveOutletDaily(json);
                int i = 0;
                foreach (ActiveOutletDailyDto item in acd)
                {
                    item.Months = dto.MonthOfYear;
                    i += searchCallDao.UpdateActiveOutletDaily(item);
                }
                if (i > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_PROCESS_SUCCESS;
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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private ResultMessageDto SaveCampaignResultTitle(SearchCallDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int i = 0;
                // Generate questions title      
                IEnumerable<SearchCallDto> services = searchCallDao.GetDistinctSurveyFromSearchCall(dto);
                QuestionDto questionDto = null;
                foreach (SearchCallDto service in services)
                {
                    questionDto = new QuestionDto();
                    questionDto.ServiceTypeID = service.Id;
                    IEnumerable<QuestionDto> questions = questionDao.GetByServiceType(questionDto);
                    AnswerDto answerDto = null;
                    int idxques = 0;
                    CampaignResultTitleDto title = null;
                    title.ServiceTypeID = (Guid)service.Id;
                    searchCallDao.DeleteCampaignResultTitle(title);
                    foreach (QuestionDto question in questions)
                    {
                        answerDto = new AnswerDto();
                        answerDto.QuestionID = question.ID;
                        int idxAnswer = 0;
                        IEnumerable<AnswerDto> answers = answerDao.GetByQuestion(answerDto);
                        foreach (AnswerDto answer in answers)
                        {
                            title = new CampaignResultTitleDto();
                            string answerText = "(" + answer.AnswerCode + " " + (string.IsNullOrEmpty(answer.ContentAnswer) ? answer.OrderIndex.ToString() : answer.ContentAnswer) + ") ";                            
                            title.ServiceQuestionsID = (Guid)question.ID;
                            title.AnswerID = answer.ID;
                            title.AnswerText = answerText;
                            i += searchCallDao.SaveCampaignResultTitle(title);
                            idxAnswer += 1;
                        }
                        idxques += 1;
                    }
                }
                if (i > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_PROCESS_SUCCESS;
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
        /// search effective campaign history
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<SearchCallDto> SearchEffectiveCampaignHistory(SearchCallDto json)
        {
            IEnumerable<SearchCallDto> result = searchCallDao.SearchEffectiveCampaignHistory(json);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IWorkbook CallCampaignHistory(SearchCallDto json)
        {
            string fileExcel = "CallCampaignHistory.xlsx";            
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
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY_HHMMSS);
                cellStyle.SetFont(font_normal);
                int rowIdx = 1;
                IEnumerable<SearchCallDto> result = searchCallDao.SearchEffectiveCampaignHistory(json);
                foreach (SearchCallDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.CustomerCode); // Cust code
                    row.GetCell(0).CellStyle = style_normal;
                    row.CreateCell(1).SetCellValue(item.CustomerName); // Customer Name
                    row.GetCell(1).CellStyle = style_normal;
                    row.CreateCell(2).SetCellValue(item.CustomerPhone); // Phone Number
                    row.GetCell(2).CellStyle = style_normal;
                    row.CreateCell(3).SetCellValue(item.EmployeeName); // Employee Name
                    row.GetCell(3).CellStyle = style_normal;
                    row.CreateCell(4).SetCellValue(item.SalemanCode); // Saleman Code
                    row.GetCell(4).CellStyle = style_normal;
                    row.CreateCell(5).SetCellValue(item.Agent); // Agent
                    row.GetCell(5).CellStyle = style_normal;
                    row.CreateCell(6).SetCellValue(item.contracts); // contracts
                    row.GetCell(6).CellStyle = style_normal;
                    // Calling Date
                    if (!string.IsNullOrEmpty(item.CallingDate))
                    {
                        row.CreateCell(7).SetCellValue(new DateTime(int.Parse(item.CallingDate.Substring(6, 4)),
                            int.Parse(item.CallingDate.Substring(3, 2)), int.Parse(item.CallingDate.Substring(0, 2)),
                            int.Parse(item.CallingDate.Substring(11, 2)), int.Parse(item.CallingDate.Substring(14, 2)),
                            int.Parse(item.CallingDate.Substring(17, 2))));
                        row.GetCell(7).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(item.CallingDate);
                    }
                    // Start time
                    if (!string.IsNullOrEmpty(item.StartTime))
                    {
                        row.CreateCell(8).SetCellValue(new DateTime(int.Parse(item.StartTime.Substring(6, 4)),
                            int.Parse(item.StartTime.Substring(3, 2)), int.Parse(item.StartTime.Substring(0, 2)),
                            int.Parse(item.StartTime.Substring(11, 2)), int.Parse(item.StartTime.Substring(14, 2)),
                            int.Parse(item.StartTime.Substring(17, 2))));
                        row.GetCell(8).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(8).SetCellValue(item.StartTime);
                    }
                    // End time
                    if (!string.IsNullOrEmpty(item.EndTime))
                    {
                        row.CreateCell(9).SetCellValue(new DateTime(int.Parse(item.EndTime.Substring(6, 4)),
                            int.Parse(item.EndTime.Substring(3, 2)), int.Parse(item.EndTime.Substring(0, 2)),
                            int.Parse(item.EndTime.Substring(11, 2)), int.Parse(item.EndTime.Substring(14, 2)),
                            int.Parse(item.EndTime.Substring(17, 2))));
                        row.GetCell(9).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(9).SetCellValue(item.EndTime);
                    }
                    row.CreateCell(10).SetCellValue(item.TalkTime); // Talk time   
                    row.GetCell(10).CellStyle = style_normal;
                    if (!string.IsNullOrEmpty(item.Status))
                    {
                        if (item.Status.Equals("0"))
                        {
                            row.CreateCell(11).SetCellValue(Constant.UNSUCCESS); // unsuccess
                            row.GetCell(11).CellStyle = style_fail;
                        }
                        else if (item.Status.Equals("1"))
                        {
                            row.CreateCell(11).SetCellValue(Constant.SUCCESS); // success
                            row.GetCell(11).CellStyle = style_success;
                        }
                        else if (item.Status.Equals("2"))
                        {
                            row.CreateCell(11).SetCellValue(Constant.CALLBACK); // call back
                            row.GetCell(11).CellStyle = style_callback;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.CallingDate))
                    {
                        row.CreateCell(12).SetCellValue(item.CallingDate.Substring(3, 7));
                        row.GetCell(12).CellStyle = style_normal;
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue(item.CallingDate);
                        row.GetCell(12).CellStyle = style_normal;
                    }
                    row.CreateCell(13).SetCellValue(item.SO); // SO
                    row.GetCell(13).CellStyle = style_normal;
                    row.CreateCell(14).SetCellValue(item.Campaign); // Campaign Name
                    row.GetCell(14).CellStyle = style_normal;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IWorkbook SFEReport(SearchCallDto json)
        {
            string fileExcel = "SFEReport.xlsx";
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
                int rowIdx = 1;
                IEnumerable<SFEReportDto> result = searchCallDao.SFEReport(json);
                foreach (SFEReportDto item in result)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(2).SetCellValue(item.cust_code); // Cust code
                    row.GetCell(2).CellStyle = style_normal;
                    row.CreateCell(3).SetCellValue(item.cust_name); // Customer Name
                    row.GetCell(3).CellStyle = style_normal;                    
                    row.CreateCell(10).SetCellValue(item.salesrep_code); // Saleman Code
                    row.GetCell(10).CellStyle = style_normal;
                    row.CreateCell(11).SetCellValue(item.salesrep_name); // Employee Name
                    row.GetCell(11).CellStyle = style_normal;
                    // Calling Date
                    if (!string.IsNullOrEmpty(item.txn_date))
                    {
                        row.CreateCell(13).SetCellValue(new DateTime(int.Parse(item.txn_date.Substring(6, 4)),
                            int.Parse(item.txn_date.Substring(0, 2)), int.Parse(item.txn_date.Substring(3, 2))));
                        row.GetCell(13).CellStyle = cellStyle;
                    }
                    else
                    {
                        row.CreateCell(13).SetCellValue(item.txn_date);
                    }
                    row.CreateCell(14).SetCellValue(item.txn_time);
                    row.GetCell(14).CellStyle = style_normal;
                    row.CreateCell(17).SetCellValue(item.cat_code); // cat_code
                    row.GetCell(17).CellStyle = style_normal;
                    row.CreateCell(18).SetCellValue(item.cat_name); // cat_name
                    row.GetCell(18).CellStyle = style_normal;
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }
    }
}