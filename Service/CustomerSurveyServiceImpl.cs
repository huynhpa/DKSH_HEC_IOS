using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.DAO;
using DKSHApplication.Common;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using DKSHApplication.DB;

namespace DKSHApplication.Service
{
    public class CustomerSurveyServiceImpl : CustomerSurveyService
    {
        UserDao userDao = new UserDao();
        CustomerDao customerDao = new CustomerDao();
        CrossSaleUpSaleDao CSUSDao = new CrossSaleUpSaleDao();
        CustomerSurveyDao surveyDao = new CustomerSurveyDao();

        

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteQuestion(CustomerSurveyInfoDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int del = surveyDao.DeleteQuestion(dto);
                if (del > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto EditQuestion(CustomerSurveyInfoDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                int edit = surveyDao.EditQuestion(dto);
                if (edit > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Customer By CustCode
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto GetCustomerByCustCode(CustomerSurveyInfoDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.CustomerSurveyInfo = surveyDao.GetCustomerSurveyInfo(json);
            result.CustomerSurveyMark = surveyDao.GetCustomerSurveyMark(json);
            result.ContactInfo = surveyDao.GetContactInfo(json);
            return result;
        }

        /// <summary>
        /// Get Question Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerSurveyInfoDto GetQuestionDetail(CustomerSurveyInfoDto dto)
        {
            CustomerSurveyInfoDto result = surveyDao.GetQuestionDetail(dto);
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Questionnaire
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public QuestionnaireDto GetQuestionnaire(CustomerSurveyInfoDto json)
        {
            QuestionnaireDto questionnaire = new QuestionnaireDto();
            questionnaire.CustomerCareCenter = surveyDao.CustomerCareCenter();
            questionnaire.Transportation = surveyDao.Transportation();
            questionnaire.Credit = surveyDao.Credit();
            questionnaire.SalesRep = surveyDao.SalesRep();
            return questionnaire;
            throw new NotImplementedException();
        }   

        /// <summary>
        /// Get Question Title
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionnaireTitleDto> GetQuestionTitle()
        {
            IEnumerable<QuestionnaireTitleDto> result = surveyDao.GetQuestionTitle();
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// New Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto NewQuestion(CustomerSurveyInfoDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (string.IsNullOrEmpty(dto.QuestionChildId))
                {
                    dto.QuestionChildId = dto.QuestionParentId;
                }
                CustomerSurveyInfoDto exist = surveyDao.CheckQuestionExisted(dto);
                if (exist != null)
                {
                    result.code = Constant.EXISTED;
                    result.message = Constant.MESSAGE_QUESTION_EXISTED;
                }
                else
                {
                    result.number = surveyDao.NewQuestion(dto);
                    if (result.number > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save Customer Survey Info
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto SaveCustomerSurveyInfo(CustomerSurveyInfoDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                /* delete customer if surveyed */
                int del = surveyDao.DeleteCustomerSurveyed(json);
                int ins = surveyDao.SaveCustomerSurveyInfo(json);
                QuestionnaireDto questionnaire = new QuestionnaireDto();
                questionnaire.CustomerCareCenter = surveyDao.CustomerCareCenter();
                questionnaire.Transportation = surveyDao.Transportation();
                questionnaire.Credit = surveyDao.Credit();
                questionnaire.SalesRep = surveyDao.SalesRep();
                List<string> CCC = json.CCC.Split(',').ToList();
                List<string> Transportation = json.Transportation.Split(',').ToList();
                List<string> Credit = json.Credit.Split(',').ToList();
                List<string> SalesRep = json.SalesRep.Split(',').ToList();
                int i = 0;
                foreach(QuestionChildDto cccItem in questionnaire.CustomerCareCenter)
                {
                    json.QuestionnaireTitleId = "CCC";
                    json.QuestionChildId = cccItem.QuestionChildId;
                    json.QuestionParentId = cccItem.QuestionParentId;
                    json.Remark = int.Parse(CCC[i].ToString());
                    result.number += surveyDao.SaveCustomerSurveyMark(json);
                    i += 1;
                }
                //int j = 0;
                //foreach (QuestionChildDto cccItem in questionnaire.Transportation)
                //{
                //    json.QuestionnaireTitleId = "Transportation";
                //    json.QuestionChildId = cccItem.QuestionChildId;
                //    json.QuestionParentId = cccItem.QuestionParentId;
                //    json.Remark = int.Parse(Transportation[j].ToString());
                //    result.number += surveyDao.SaveCustomerSurveyMark(json);
                //    j += 1;
                //}
                //int m = 0;
                //foreach (QuestionChildDto cccItem in questionnaire.Credit)
                //{
                //    json.QuestionnaireTitleId = "Credit";
                //    json.QuestionChildId = cccItem.QuestionChildId;
                //    json.QuestionParentId = cccItem.QuestionParentId;
                //    json.Remark = int.Parse(Credit[m].ToString());
                //    result.number += surveyDao.SaveCustomerSurveyMark(json);
                //    m += 1;
                //}
                //int n = 0;
                //foreach (QuestionChildDto cccItem in questionnaire.SalesRep)
                //{
                //    json.QuestionnaireTitleId = "SalesRep";
                //    json.QuestionChildId = cccItem.QuestionChildId;
                //    json.QuestionParentId = cccItem.QuestionParentId;
                //    json.Remark = int.Parse(SalesRep[n].ToString());
                //    result.number += surveyDao.SaveCustomerSurveyMark(json);
                //    n += 1;
                //}

                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Customer Survey Report
        /// </summary>
        /// <returns></returns>
        public IWorkbook CustomerSurveyReport()
        {
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + Constant.CUSTOMER_SURVER_REPORT_XLSX;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(Constant.DDMYYYY);
                var color_expired = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_warning = new XSSFColor(new byte[] { 255, 153, 0 });
                var color_x = new XSSFColor(new byte[] { 51, 204, 51 });
                var color_red = new XSSFColor(new byte[] { 255, 0, 0 });
                var color_info = new XSSFColor(new byte[] { 24, 213, 247 });
                var font_expired = (XSSFFont)workbook.CreateFont();
                var font_warning = (XSSFFont)workbook.CreateFont();
                var font_x = (XSSFFont)workbook.CreateFont();
                var font_red = (XSSFFont)workbook.CreateFont();
                var font_info = (XSSFFont)workbook.CreateFont();
                font_expired.SetColor(color_expired);
                font_warning.SetColor(color_warning);
                font_x.SetColor(color_x);
                font_red.SetColor(color_red);
                font_expired.IsBold = true;
                font_warning.IsBold = true;
                font_x.IsBold = true;
                font_red.IsBold = true;
                font_info.SetColor(color_info);
                font_info.IsBold = true;
                // Expired
                var style_expired = (XSSFCellStyle)workbook.CreateCellStyle();
                style_expired.SetFont(font_expired);
                // Coming expire
                var style_warning = (XSSFCellStyle)workbook.CreateCellStyle();
                style_warning.SetFont(font_warning);
                // Style for X value
                var style_x = (XSSFCellStyle)workbook.CreateCellStyle();
                style_x.SetFont(font_x);
                // style for red color
                var style_red = (XSSFCellStyle)workbook.CreateCellStyle();
                style_red.SetFont(font_red);
                // style for info
                var style_info = (XSSFCellStyle)workbook.CreateCellStyle();
                style_info.SetFont(font_info);
                // Create sheet
                ISheet sheet = workbook.GetSheet("DATA");
                int rowTitleIdx = 1;
                int rowQuestionIdx = 1;
                IEnumerable<CustomerSurveyInfoDto> surveyInfo = surveyDao.CustomerSurveyInfo();
                IEnumerable<QuestionnaireTitleDto> questionnairetitle = surveyDao.GetQuestionTitle();
                List<int> posSub = new List<int>();
                foreach (QuestionnaireTitleDto item in questionnairetitle)
                {
                    IRow rowTitle = sheet.CreateRow(rowTitleIdx);
                    rowTitle.CreateCell(0).SetCellValue(item.DescEN);
                    rowQuestionIdx = rowTitleIdx + 1;
                    IEnumerable<CustomerSurveyInfoDto> questionList = surveyDao.GetCustomerSurveyReport(item);
                    
                    foreach(CustomerSurveyInfoDto question in questionList)
                    {
                        IRow rowQuestion = sheet.CreateRow(rowQuestionIdx);
                        string cellVal = question.QuestionChildId + ". " + question.QuestionEN;
                        rowQuestion.CreateCell(0).SetCellValue(cellVal);
                        rowQuestionIdx += 1;
                        rowTitleIdx = rowQuestionIdx + 1;
                    }
                    rowTitleIdx += 1;
                }
                IRow subTotal = sheet.CreateRow(rowQuestionIdx);
                subTotal.CreateCell(0).SetCellValue("Total");
                subTotal.GetCell(0).CellStyle = style_red;
                int colIdx = 1;
                int rowMark = 2;
                int total = 0;
                foreach (CustomerSurveyInfoDto obj in surveyInfo)
                {
                    IRow rowCustCode = sheet.GetRow(1);
                    rowCustCode.CreateCell(colIdx).SetCellValue(obj.CustName);
                    IEnumerable<CustomerSurveyMarkDto> surveyMark = surveyDao.GetCustomerSurveyMark(obj);
                    foreach (CustomerSurveyMarkDto mark in surveyMark)
                    {
                        IRow userSurvey = sheet.GetRow(0);
                        userSurvey.CreateCell(colIdx).SetCellValue(mark.UserSurvey + "-" + mark.UserGroup);
                        userSurvey.GetCell(colIdx).CellStyle = style_x;
                        IRow rowsurveymark = sheet.GetRow(rowMark);
                        
                        if (rowsurveymark != null)
                        {
                            rowsurveymark.CreateCell(colIdx).SetCellValue(mark.Remark == 0 ? "" : mark.Remark.ToString());
                        }
                        if (total == surveyMark.Count() - 1)
                        {
                            IRow totalRow = sheet.GetRow(rowMark + 1);
                            if (totalRow != null)
                            {
                                totalRow.CreateCell(colIdx).SetCellValue(obj.TotalMark.ToString());
                                totalRow.GetCell(colIdx).CellStyle = style_red;
                            }
                        }
                        total += 1;
                        rowMark += 1;
                    }
                    total = 0;
                    rowMark = 2;
                    colIdx += 1;
                }
                return workbook;
            }
        }

        /// <summary>
        /// Get Survey History
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IEnumerable<CustomerSurveyInfoDto> GetSurveyHistory(CustomerSurveyInfoDto json)
        {
            if (String.IsNullOrEmpty(json.SurveyToDate))
            {
                json.SurveyToDate = json.SurveyFromDate;
            }
            IEnumerable<CustomerSurveyInfoDto> result = surveyDao.GetSurveyHistory(json);
            return result;
        }

        /// <summary>
        /// Get iMind Agents
        /// </summary>
        /// <returns></returns>
        public IEnumerable<iMindUserDto> GetiMindAgents()
        {
            IEnumerable<iMindUserDto> result = userDao.GetActiveAgent();
            return result;
        }
    }
}