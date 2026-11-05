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
using Asterisk.NET.Manager;
using Asterisk.NET.Util;
using Asterisk.NET.IO;
using Asterisk.NET.Manager.Action;
using Asterisk.NET.Manager.Response;
using System.Globalization;
using System.Data.SqlClient;
using System.Text;
using System.Net;

namespace DKSHApplication.Service
{
    public class DoCampaignServiceImpl : DoCampaignService
    {
        DoCampaignDao campaignDao = new DoCampaignDao();
        CampaignDao servicetypeDao = new CampaignDao(); 
        CustomerLicensesDao customerLicensesDao = new CustomerLicensesDao();
        PhoneNumberDao phoneNumberDao = new PhoneNumberDao();
        AssignDao assignDao = new AssignDao();
        HrEmployeeDao hrEmpDao = new HrEmployeeDao();
        iMindDao iMind = new iMindDao();
        CallingTimeDao callingTimeDao = new CallingTimeDao();
        CallPlanDao callPlanDao = new CallPlanDao();
        SurveyVoteDao surveyVoteDao = new SurveyVoteDao();
        ServiceTypeDao serviceTypeDao = new ServiceTypeDao();
        SearchCallDao searchCallDao = new SearchCallDao();
        ZaloTrackingDao zaloDao = new ZaloTrackingDao();
        SurveyResultDao surveyResultDao = new SurveyResultDao();
        ServiceQuestionsDao serviceQuestionsDao = new ServiceQuestionsDao();
        AnswerDao answerDao = new AnswerDao();
        private const string yyyy_MM_dd = "yyyy-MM-dd";
        private const string yyyy_MM_dd_HH_mm = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// Get General Survey Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public OutboundSurveyDataResult GetGeneralSurveyData(OBSurveyParam param)
        {
            OutboundSurveyDataResult dataResult = new OutboundSurveyDataResult();
            try
            {
                //string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                //param.SurveyDate = currDate;
                dataResult.Status = campaignDao.LoadStatus(); // get status
                dataResult.HrEmployee = campaignDao.GetGroupIDByWindowsUser(param); // Get Group ID By window user
                if (dataResult.HrEmployee != null && dataResult.HrEmployee.Count() > 0)
                {
                    param.ProjectID = dataResult.HrEmployee.ElementAt(0).GroupID;
                }
                dataResult.ServiceType = campaignDao.GetActiveServiceType(param); // get service type list by project
                OBSurveyParam dto = new OBSurveyParam();
                CampaignScriptResult campaignScriptResult = null;
                foreach (ServiceTypeDto service in dataResult.ServiceType)
                {
                    dto = new OBSurveyParam();
                    campaignScriptResult = new CampaignScriptResult();
                    dto.ServiceTypeId = service.ID;
                    campaignScriptResult.Campaign = service;
                    campaignScriptResult.ServiceQuestionList = serviceQuestionsDao.GetServiceQuestionsByServiceType(dto).ToList();
                    List<AnswerDto> AnswerList = new List<AnswerDto>();
                    AnswerDto answerDto = new AnswerDto();
                    List<string> RadionButtonList = new List<string>();
                    // Get answer by question
                    for (int j = 0; j < campaignScriptResult.ServiceQuestionList.Count(); j++)
                    {
                        dto.QuestionID = campaignScriptResult.ServiceQuestionList.ElementAt(j).QuestionID;
                        campaignScriptResult.ServiceQuestionList.ElementAt(j).AnswerList = answerDao.GetByQuestionIDNew(dto);
                        int ans = 0;
                        foreach (AnswerDto answer in campaignScriptResult.ServiceQuestionList.ElementAt(j).AnswerList)
                        {
                            if (answer.AnswerTypeCode.Equals("4"))
                            {
                                for (int n = 0; n < answer.MaxValue; n++)
                                {
                                    campaignScriptResult.ServiceQuestionList.ElementAt(j).AnswerList.ElementAt(ans).RadionButtonList.Add("rdType4_" + n);
                                }
                            }
                            ans += 1;
                        }
                    }
                    dataResult.CampaignScriptResult.Add(campaignScriptResult); // add object into list          
                }
            }
            catch(Exception ex)
            {
                dataResult.CODE = Constant.ERROR;
                dataResult.MESSAGE = ex.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// GetLicenseStatus
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private CustomerLicensesDto GetLicenseStatus(CustomerLicensesDto dto)
        {
            CustomerLicensesDto result = customerLicensesDao.GetLicenseStatus(dto);
            return result;
        }

        /// <summary>
        /// Login Asterisk
        /// </summary>
        /// <param name="loginAsteriskDto"></param>
        /// <returns></returns>
        public ResultMessageDto LoginAsterisk(LoginAsteriskDto loginAsteriskDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            string ASTERISK_SERVER = Constant.ASTERISK_SERVER_VIME;
            int ASTERISK_PORT = Constant.ASTERISK_PORT_VIME;
            string ASTERISK_USER = Constant.ASTERISK_USER_VIME;
            string ASTERISK_PASSWORD = Constant.ASTERISK_PASSWORD_VIME;
            if (loginAsteriskDto.contracts.Equals(Constant.GIGAMED))
            {
                ASTERISK_SERVER = Constant.ASTERISK_SERVER_GIGA;
                ASTERISK_PORT = Constant.ASTERISK_PORT_GIGA;
                ASTERISK_USER = Constant.ASTERISK_USER_GIGA;
                ASTERISK_PASSWORD = Constant.ASTERISK_PASSWORD_GIGA;
                //if (loginAsteriskDto.PhoneNumber.Substring(0, 1).Equals("9"))
                //{
                //    loginAsteriskDto.PhoneNumber = loginAsteriskDto.PhoneNumber.Remove(0, 1);
                //}
                Click2CallDto click2Call = new Click2CallDto()
                {
                    extension = loginAsteriskDto.UserExt.Trim(),
                    phone_number = loginAsteriskDto.PhoneNumber,
                    contracts = loginAsteriskDto.contracts
                };
                result = Click2CallGigamed(click2Call);
            }
            
            Asterisk.NET.Manager.ManagerConnection manager = new ManagerConnection(ASTERISK_SERVER, ASTERISK_PORT, ASTERISK_USER, ASTERISK_PASSWORD);                
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // .NET 4.5
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .NET 4.0
                manager.Login();
                OriginateAction oc = new OriginateAction();
                oc.Context = "from-internal";
                oc.Priority = 1;
                oc.Channel = "SIP/" + loginAsteriskDto.UserExt.ToString().Substring(0, 3);
                oc.CallerId = loginAsteriskDto.PhoneNumber + " <" + loginAsteriskDto.UserExt.Trim().ToString().Substring(0, 3) + ">";
                if (!string.IsNullOrEmpty(loginAsteriskDto.PhoneNumber) && loginAsteriskDto.PhoneNumber.StartsWith("0"))
                {
                    oc.Exten = loginAsteriskDto.PhoneNumber;
                }
                else if (!string.IsNullOrEmpty(loginAsteriskDto.PhoneNumber) && loginAsteriskDto.PhoneNumber.StartsWith("9"))
                {
                    oc.Exten = loginAsteriskDto.PhoneNumber.Substring(1);
                    loginAsteriskDto.PhoneNumber = loginAsteriskDto.PhoneNumber.Substring(1);
                }
                //if (loginAsteriskDto.PhoneNumber.Substring(0, 1).Equals("9"))
                //{
                //    loginAsteriskDto.PhoneNumber = loginAsteriskDto.PhoneNumber.Remove(0, 1);
                //}
                oc.Timeout = 30000;
                oc.Async = true;
                ManagerResponse originateResponse = manager.SendAction(oc, oc.Timeout);
                result.message = oc.ToString();
                manager.Logoff();
            }
            catch (Asterisk.NET.Manager.TimeoutException timeoutex)
            {
                result.code = Constant.ERROR;
                result.message = timeoutex.Message;
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            finally
            {
                manager.Logoff();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
            return result;
        }

        /*
            - action=autocall : fix
            - exten=236 : số máy nhánh của nhân viên
            - stringcode=as7dkf8ie2343it26fg32kaken43f123fdkjkek : fix
            - click2callid=new Guid() : bên crm sinh ra và gửi sang tổng đài
            - number=0903969239 : đầu số khách hàng cần gọi ra
            vime dksh
            http://10.210.148.36/clicktocalldksh/doAction.php?action=autocall&exten=236&stringcode=as7dkf8ie2343it26fg32kaken43f123fdkjkek&click2callid=call1iddfsd3fsdf1&number=0903969239            

            http://10.210.148.36/clicktocalldksh/doAction.php?action=getrecordingscall&stringcode=as7dkf8ie2343it26fg32kaken43f123fdkjkek&click2callid=call1iddfsd3feesdf1
            https://stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c 
        */
        /// <summary>
        /// Click 2 Call
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="phone_number"></param>
        public ResultMessageDto Click2Call(Click2CallDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var request = (HttpWebRequest)WebRequest.Create(Constant.CLICK_2_CALL_VIME);
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";            
            Guid click2callid = Guid.NewGuid();
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                // Append json parameters
                string postData = "{\"action\":\"" + Constant.ACTION_AUTOCALL + "\"," +
                            "\"exten\":\"" + dto.extension + "\"," +
                            "\"stringcode\":\"" + Constant.CLICK_2_CALL_CODE_VIME + "\"," +
                            "\"click2callid\":\"" + click2callid.ToString() + "\"," +
                            "\"number\":\"" + dto.phone_number + "\"}";
                streamWriter.Write(postData);
                streamWriter.Flush();
            }
            // Validate ssl
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                try
                {
                    var rs = streamReader.ReadToEnd();
                    Click2CallDto click2Call = new Click2CallDto();
                    click2Call.click2callid = click2callid;
                    click2Call.extension = dto.extension;
                    click2Call.phone_number = dto.phone_number;
                    click2Call.CustCode = dto.CustCode;
                    click2Call.action_autocall = Constant.ACTION_AUTOCALL;
                    click2Call.stringcode = dto.stringcode;
                    click2Call.contracts = dto.contracts;
                    result.click2Call = click2Call;
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        campaignDao.Click2Call(click2Call);
                        result.code = Constant.SUCCESS;
                        result.message = Constant.HTTP_MESSAGE_OK;
                    }
                }
                catch (HttpException ex)
                {
                    result.code = Constant.ERROR;
                    result.message = ex.Message;
                }
            }
            
            return result;
        }


        /*
            - action=autocall : fix
            - exten=236 : số máy nhánh của nhân viên
            - stringcode=as7dkf8ie2343it26fg32kaken43f123fdkjkek : fix
            - click2callid=new Guid() : bên crm sinh ra và gửi sang tổng đài
            - number=0903969239 : đầu số khách hàng cần gọi ra
            vime dksh
            https://gigamed.voip247.vn/clicktocallgigamed/doAction.php?action=autocall&exten=329&stringcode=as7dkf8ie2343it26fg32kasdfd3f123fdkjkek&click2callid=call1iddf1212sd3fsdf1&number=0903969239

            http://10.210.148.36/clicktocalldksh/doAction.php?action=getrecordingscall&stringcode=as7dkf8ie2343it26fg32kaken43f123fdkjkek&click2callid=call1iddfsd3feesdf1
            https://stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c 
        */
        public ResultMessageDto Click2CallGigamed(Click2CallDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var request = (HttpWebRequest)WebRequest.Create(Constant.CLICK_2_CALL_GIGAMED);
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            Guid click2callid = Guid.NewGuid();
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                // Append json parameters
                string postData = "{\"action\":\"" + Constant.ACTION_AUTOCALL + "\"," +
                            "\"exten\":\"" + dto.extension + "\"," +
                            "\"stringcode\":\"" + Constant.CLICK_2_CALL_CODE_GIGAMED + "\"," +
                            "\"click2callid\":\"" + click2callid.ToString() + "\"," +
                            "\"number\":\"" + dto.phone_number + "\"}";
                streamWriter.Write(postData);
                streamWriter.Flush();
            }
            // Validate ssl
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                try
                {
                    var rs = streamReader.ReadToEnd();
                    Click2CallDto click2Call = new Click2CallDto();
                    click2Call.click2callid = click2callid;
                    click2Call.extension = dto.extension;
                    click2Call.phone_number = dto.phone_number;
                    click2Call.CustCode = dto.CustCode;
                    click2Call.action_autocall = Constant.ACTION_AUTOCALL;
                    click2Call.stringcode = dto.stringcode;
                    click2Call.contracts = dto.contracts;
                    result.click2Call = click2Call;
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        campaignDao.Click2Call(click2Call);
                        result.code = Constant.SUCCESS;
                        result.message = Constant.HTTP_MESSAGE_OK;
                    }
                }
                catch (HttpException ex)
                {
                    result.code = Constant.ERROR;
                    result.message = ex.Message;
                }
            }

            return result;
        }


        /// <summary>
        /// SAVE SURVEY
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto SaveSurvey(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/FileUpload/ZaloTracking/"));
            try
            {
                int saveSurvey = 0;
                SurveyVoteDto svObj = GetSurveyVote(param);
                if (!string.IsNullOrEmpty(param.PhoneNumber))
                {
                    hrEmployeeDto hrEmployee = new hrEmployeeDto();
                    hrEmployee.ID = FGuid.ToGuid(param.UserId);
                    hrEmployee = hrEmpDao.GetEmployeeDetail(hrEmployee);
                    if (param.IsUpdate == true && (svObj != null
                        || (!string.IsNullOrEmpty(param.SurveyVoteID.ToString()) && param.SurveyVoteID != Guid.Empty)))
                    {
                        if (string.IsNullOrEmpty(param.SurveyId.ToString()) || param.SurveyId == Guid.Empty)
                        {
                            param.SurveyId = param.SurveyVoteID;
                            svObj = GetSurveyVote(param);
                        }
                        if (svObj != null && ((!string.IsNullOrEmpty(svObj.ID.ToString()) && svObj.ID != Guid.Empty)
                         || (!string.IsNullOrEmpty(param.SurveyVoteID.ToString()) && param.SurveyVoteID != Guid.Empty)))
                        {
                            svObj.UpdatedBy = param.WindowsUser;
                            svObj.CustomField10 = param.phone_number; // Customer phone number 90xxxxxxxxx
                            svObj.CustomField9 = param.ContactPerson;
                            svObj.CustomField8 = param.SO;
                            svObj.CustomField7 = param.Remark;
                            svObj.CustomField4 = hrEmployee.EmployeeID; //sale man code
                            svObj.SurveyStatus = param.Status;
                            if (param.CallPlan == true)
                            {
                                svObj.CustomField6 = Constant.CALL_PLAN;
                            }
                            saveSurvey = surveyVoteDao.UpdateSurveyVote(svObj);
                        }
                        else
                        {
                            svObj = new SurveyVoteDto();
                            result.code = Constant.ERROR;
                            result.message = Constant.EDIT_CALL_PERMISSION_MESSAGE;
                            svObj.FeedBackStatus = Constant.FEEDBACK_STATUS_NORMAL;
                            svObj.SurveyDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        // END SURVEY
                    }
                    else
                    {                                              
                        if (string.IsNullOrEmpty(param.ServiceTypeId.ToString()))
                        {
                            param.ServiceTypeId = param.ServiceId;
                        }
                        ServiceTypeDto ServiceObj = serviceTypeDao.GetServiceTypeByID(param);
                        if (ServiceObj != null)
                        {
                            svObj.MSBCH = serviceTypeDao.GetNextServiceID(ServiceObj);
                        }
                        hrEmployeeDto empDto = new hrEmployeeDto();
                        empDto.ID = FGuid.ToGuid(param.UserId);
                        empDto = hrEmpDao.GetEmployeeDetail(empDto);
                        svObj.CreatedBy = param.WindowsUser;
                        svObj.CustomField10 = param.phone_number;
                        svObj.CustomField9 = param.ContactPerson;
                        svObj.CustomField8 = param.SO;
                        svObj.CustomField7 = param.Remark;
                        svObj.CustomField4 = empDto.EmployeeID;
                        svObj.SurveyStatus = param.Status;
                        if (param.CallPlan == true)
                        {
                            svObj.CustomField6 = Constant.CALL_PLAN;
                        }
                        if (string.IsNullOrEmpty(svObj.ID.ToString()) || svObj.ID == Guid.Empty)
                        {
                            string code = "";
                            SurveyVoteDto temp = null;
                            ServiceTypeDto svtypeObj = serviceTypeDao.GetServiceTypeByID(param);
                            if (svtypeObj != null)
                            {
                                code = svtypeObj.ServiceCode;
                            }
                            if (code != "03" && code != "04") // khong la MB, FC
                            {
                                temp = surveyVoteDao.GetByPhoneNumber(param);
                            }
                            else // la MB hoac FC
                            {
                                temp = surveyVoteDao.GetByPhoneNumberNotCode(param);
                                if (temp != null) // la MB hoac FC nhung da goi trong trong dich vu khac roi
                                {
                                    string[] strParam = new string[2];
                                    strParam[0] = empDto.AgentNumber;
                                    strParam[1] = temp.SurveyDate;
                                    result.code = Constant.ERROR;
                                    result.message = string.Format(Constant.PHONE_NUMBER_ALREADY_SURVEY_AGENT, strParam);
                                }
                                else
                                {
                                    if (code == "03")// dang chon dich vu MB
                                    {
                                        // kiem tra da goi MB chua
                                        param.ServiceCode = "03";
                                        temp = surveyVoteDao.GetByPhoneNumber(param);
                                        if (temp != null)
                                        {
                                            string[] strParam = new string[2];
                                            strParam[0] = empDto.AgentNumber;
                                            strParam[1] = temp.SurveyDate;
                                            result.code = Constant.ERROR;
                                            result.message = string.Format(Constant.PHONE_NUMBER_ALREADY_SURVEY_MOBI_INTERNET, strParam);
                                        }
                                    }
                                    else if (code == "04")// dang khao sat dich vu FC
                                    {
                                        param.ServiceCode = "04";
                                        temp = surveyVoteDao.GetByPhoneNumberService(param);
                                        if (temp != null)
                                        {
                                            string[] strParam = new string[2];
                                            strParam[0] = empDto.AgentNumber;
                                            strParam[1] = temp.SurveyDate;
                                            result.message = string.Format(Constant.PHONE_NUMBER_ALREADY_SURVEY_FAST_CONNECT, strParam);
                                            result.code = Constant.ERROR;

                                        }
                                    }
                                }
                            }
                            // INSERT SURVEY VOTE
                            svObj.CreatedBy = param.WindowsUser;
                            svObj.UpdatedBy = param.WindowsUser;
                            if (!string.IsNullOrEmpty(param.StartTime))
                            {
                                svObj.StartTime = Utils.StringToDateDDMMYYYYHHMMSS(param.StartTime, Constant.YYYYMMDDHHMMSS).ToString();
                            }                            
                            svObj.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (param.SurveyVoteID != null && param.SurveyVoteID != Guid.Empty)
                            {
                                svObj.ID = (Guid)param.SurveyVoteID;
                            }
                            else
                            {
                                svObj.ID = Guid.NewGuid();
                            }
                            svObj.EmployeeID = param.UserId; // Employee ID
                            svObj.CustomerName = param.CustName; // Customer name
                            svObj.CustomerAddress = param.CustomerAddress; // Customer address
                            svObj.CustomerPhone = param.SurveyCustCode; // Cust code
                            svObj.ServiceTypeID = param.ServiceTypeId; // Campaign ID
                            svObj.SurveyStatus = param.Status;
                            saveSurvey = surveyVoteDao.InsertSurveyVote(svObj);
                            result.SurveyVote = svObj; // set Survey Vote result  
                            // save call by campaign effective 
                            
                        }

                        /*  cap nhat lai tình trạng thuê bao cua assign 
                            1:Bình thường;
                            2:Thuê bao bị khóa;
                            3:Thuê bao đã hủy;
                            4:Thuê bao tắt máy;
                            5:Không liên lạc được;
                            6:Khách hàng ROAMING
                        */
                        if (param.AssignID != null && param.AssignID != Guid.Empty)
                        {
                            AssignDto obj = new AssignDto();
                            PhoneNumberDto pn = new PhoneNumberDto();
                            pn.ID = param.AssignID;
                            pn.ProjectId = param.ProjectID;
                            obj = assignDao.GetAssignDetail(pn);
                            obj.ID = param.AssignID;
                            obj.Status = svObj.CustomField1;
                            //Neu cuoc goi nay thanh cong thi cap nhat lai tinh trang phan cong(assign) la thanh cong
                            if (param.Status.Equals("1"))
                            {
                                obj.Status = "10";
                            }
                            assignDao.UpdateAssign(obj);
                        }
                        //else
                        //{
                        //    result.code = Constant.ERROR;
                        //    result.message = Constant.ERROR_SAVE_SURVEY;
                        //}
                    }
                    if (saveSurvey > 0)
                    {                        
                        if (param.IsUpdate == false)
                        {
                            result.code = Constant.SUCCESS_NEXT_ASSIGN;
                            result.message = Constant.MESSAGE_SUCCESS;
                        }
                        else
                        {
                            result.code = Constant.SUCCESS;
                            result.message = Constant.MESSAGE_SUCCESS;
                        }
                        result.ServiceType = new ServiceTypeDto();
                        result.ServiceType.ID = (Guid)param.ServiceTypeId;
                    }
                    
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.CUSTCODE_404;
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
        /// GetSurveyVote
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private SurveyVoteDto GetSurveyVote(OBSurveyParam param)
        {
            SurveyVoteDto surveyVoteDto = new SurveyVoteDto();
            try
            {
                if (string.IsNullOrEmpty(param.SurveyId.ToString()) || param.SurveyId == Guid.Empty)
                {
                    surveyVoteDto.SurveyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    surveyVoteDto = surveyVoteDao.GetSurveyVote(param);
                    if (surveyVoteDto != null)
                    {
                        surveyVoteDto.SurveyDate = surveyVoteDto.SurveyDate;
                    }
                }
            }
            catch (Exception ex)
            {
                surveyVoteDto.code = Constant.ERROR;
                surveyVoteDto.message = ex.Message;
            }
            return surveyVoteDto;
        }


        /// <summary>
        /// Get Next Assign
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public LoadPhoneNumberResult GetNextAssign(OBSurveyParam param)
        {
            LoadPhoneNumberResult result = new LoadPhoneNumberResult();
            result.iMindCustomerInfo = new List<iMindCustomerInfoDto>();
            result.CallingTime = new List<CallingTimeDto>();
            result.OutboundHistory = new List<DoCampaignResultDto>();
            result.PhoneNumber = new List<PhoneNumberDto>();
            string isCallback = string.Empty;
            PhoneNumberDto pnDto = new PhoneNumberDto();
            PhoneNumberDto phoneNumberResult = null;
            pnDto.PhoneNumber = param.CustCode;
            pnDto.WindowsUser = param.WindowsUser;
            if (param.ServiceTypeId != null)
            {
                pnDto.ServiceType = param.ServiceTypeId;
                pnDto.ProjectId = param.ProjectID;
            }
            else if (param.ServiceId != null)
            {
                pnDto.ServiceType = param.ServiceId;
                pnDto.ProjectId = param.ProjectID;
            }
            try
            {
                PhoneNumberDto vnz0code = phoneNumberDao.GetCustomerVime(param); // VNZ0code
                if (vnz0code != null && !string.IsNullOrEmpty(vnz0code.CustCode))
                {
                    param.CustCode = vnz0code.CustCode;
                    pnDto.PhoneNumber = vnz0code.CustCode;
                }
                // Get call plan
                PhoneNumberDto phone_call_plan = phoneNumberDao.GetPhoneNumberFromCallPlan(param);
                if (phone_call_plan != null && !string.IsNullOrEmpty(param.CustCode) && phone_call_plan.PhoneNumber.Equals(param.CustCode))
                {                    
                    phoneNumberResult = phone_call_plan;
                }
                else
                {
                    IEnumerable<PhoneNumberDto> cntPhone = phoneNumberDao.CountPhoneNumber(pnDto);
                    if (cntPhone.Count() > 0 && cntPhone.ElementAt(0).CountPhoneNumber == 0)
                    {
                        isCallback = "0";
                        phoneNumberResult = phoneNumberDao.Top1PhoneNumberCurrentDate(pnDto);
                        if (phoneNumberResult == null)
                        {
                            phoneNumberResult = phoneNumberDao.Top1PhoneNumber(pnDto);
                        }
                    }
                    else
                    {
                        isCallback = "1";
                        phoneNumberResult = phoneNumberDao.Top1PhoneNumberAppointmentDate(pnDto);
                    }
                }
                AssignDto assignDto = new AssignDto();
                PhoneNumberDto pnEntity = new PhoneNumberDto();
                hrEmployeeDto hrempDto = new hrEmployeeDto();        
                if (phoneNumberResult != null)
                {
                    // get customer wherther focus the campaign to active script
                    CallPlanByCampaignDto focus = new CallPlanByCampaignDto()
                    {
                        CustCode = phoneNumberResult.PhoneNumber
                    };
                    IEnumerable<CallPlanByCampaignDto> focus_campaign = campaignDao.CustomerFocusCampaign(focus);
                    result.FocusCampaign = focus_campaign;
                    // set start time current date time
                    phoneNumberResult.StartTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    if (string.IsNullOrEmpty(phoneNumberResult.PhoneNumber))
                    {
                        phoneNumberResult.PhoneNumber = param.CustCode;
                    }
                    string assignID = phoneNumberResult.AssignID.ToString();
                    if (!assignID.Equals(Guid.Empty.ToString()))
                    {
                        assignDto.ID = new Guid(assignID);
                        assignDto.AgentID = param.AgentID;
                        assignDto.Status = "-2";
                        int udt = assignDao.UpdateAssign(assignDto);
                    }
                    else
                    {
                        assignDto.Status = "-2";
                        assignDto.ID = Guid.NewGuid();
                        assignDto.AgentID = param.AgentID;
                        assignDto.ProjectId = (Guid)param.ProjectID;
                        hrempDto.ID = FGuid.ToGuid(param.AgentID);
                        hrEmployeeDto emp = hrEmpDao.GetEmployeeDetail(hrempDto);
                        if (emp != null)
                        {
                            assignDto.CreatedBy = emp.WindowsUser;
                        }
                        assignDto.PhoneNumberID = new Guid(phoneNumberResult.ID.ToString());
                        int insert = assignDao.InsertAssign(assignDto);
                        pnEntity.ID = assignDto.PhoneNumberID;
                        pnEntity.IsAssigned = true;
                        int udtP = phoneNumberDao.UpdateIsAssigned(pnEntity);
                    }
                    result.assignDto = assignDto;
                    // Get Customer Info from iMind
                    ContactsDto contactDto = new ContactsDto();
                    contactDto.code = phoneNumberResult.PhoneNumber;
                    List<iMindCustomerInfoDto> iMindInfo = new List<iMindCustomerInfoDto>();
                    // begin customize abbott
                    if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0,3).Equals(Constant.DOCTOR_ABBOTT))
                    {
                        DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                        if (doctor != null)
                        {
                            iMindInfo.ElementAt(0).phone_number = doctor.PhoneNumber;
                            iMindInfo.ElementAt(0).CustName = doctor.PhoneNumber;
                            iMindInfo.ElementAt(0).code = doctor.DoctorCode;
                        }
                    }
                    else
                    {
                        iMindInfo = iMind.CustomeriMindInfo(contactDto).ToList();
                    }
                    //IEnumerable<iMindCustomerInfoDto> iMindInfo = iMind.CustomeriMindInfo(contactDto);                                     
                    // Get calling time
                    param.CustCode = phoneNumberResult.PhoneNumber;
                    IEnumerable<CallingTimeDto> callingTime = callingTimeDao.CallingTime(param);
                    iMindCustomerInfoDto imindCust = null;
                    if (iMindInfo != null && iMindInfo.Count() > 0)
                    {
                        iMindInfo.ElementAt(0).ID = assignDto.ID;
                        imindCust = new iMindCustomerInfoDto();
                        imindCust.code = iMindInfo.ElementAt(0).code;
                        imindCust.contact_id = iMindInfo.ElementAt(0).contact_id;
                        imindCust.CustName = iMindInfo.ElementAt(0).name;
                        imindCust.addr_detail = iMindInfo.ElementAt(0).addr_detail;
                        imindCust.phone_number = iMindInfo.ElementAt(0).phone_number;
                        imindCust.note = iMindInfo.ElementAt(0).note;
                        imindCust.Remark = phoneNumberResult.CustName;
                        imindCust.VNZ0Code = phoneNumberResult.VNZ0Code;
                        result.iMindCustomerInfo.Add(imindCust);
                        if (callingTime.Count() > 0)
                        {
                            string[] priority1 = callingTime.ElementAt(0).Priority1.ToString().Split('-');
                            string startTime = priority1[0];
                            string endTime = priority1[1];
                            DateTime dtNow = DateTime.Now;
                            DateTime startDate = DateTime.ParseExact(dtNow.ToString(yyyy_MM_dd) + " " + startTime.Trim(), yyyy_MM_dd_HH_mm, CultureInfo.InvariantCulture);
                            DateTime endDate = DateTime.ParseExact(dtNow.ToString(yyyy_MM_dd) + " " + endTime.Trim(), yyyy_MM_dd_HH_mm, CultureInfo.InvariantCulture);
                            CallingTimeDto calltime = new CallingTimeDto();
                            if (dtNow < startDate || dtNow > endDate)
                            {
                                calltime.Priority1 = callingTime.ElementAt(0).Priority1.ToString();
                                calltime.Priority2 = callingTime.ElementAt(0).Priority2.ToString();
                                calltime.isCallback = isCallback;
                                result.CallingTime.Add(calltime);
                            }
                            ContactsDto contacts = new ContactsDto();
                            contacts.code = phoneNumberResult.PhoneNumber;
                            // get list phone number from iMind
                            IEnumerable<iMindContactInfoDto> PhoneNumber = iMind.ContactPhoneiMind(contacts);
                            if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                            {
                                DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                                if (doctor != null)
                                {
                                    PhoneNumber.ElementAt(0).phone_number = doctor.PhoneNumber;
                                    PhoneNumber.ElementAt(0).person_name = doctor.PhoneNumber;
                                    PhoneNumber.ElementAt(0).code = doctor.DoctorCode;
                                }
                            }
                            if (PhoneNumber.Count() > 0)
                            {
                                int i = 0;
                                PhoneNumberDto phone = null;
                                foreach (iMindContactInfoDto item in PhoneNumber)
                                {
                                    phone = new PhoneNumberDto();
                                    phone.PersonName = item.person_name;
                                    phone.PhoneNumber = item.phone_number;
                                    phone.Extension = param.WindowsUser.Substring(0, 3);
                                    result.PhoneNumber.Add(phone);
                                    i += 1;
                                }
                            }
                        }
                        else
                        {
                            ContactsDto contacts = new ContactsDto();
                            contacts.code = phoneNumberResult.PhoneNumber;
                            contacts.VNZ0Code = phoneNumberResult.VNZ0Code;
                            IEnumerable<iMindContactInfoDto> PhoneNumber = iMind.ContactPhoneiMind(contacts);
                            if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                            {
                                DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                                if (doctor != null)
                                {
                                    PhoneNumber.ElementAt(0).phone_number = doctor.PhoneNumber;
                                    PhoneNumber.ElementAt(0).person_name = doctor.PhoneNumber;
                                    PhoneNumber.ElementAt(0).code = doctor.DoctorCode;
                                }
                            }
                            if (PhoneNumber.Count() > 0)
                            {
                                int i = 0;
                                PhoneNumberDto phone = null;
                                foreach (iMindContactInfoDto item in PhoneNumber)
                                {
                                    phone = new PhoneNumberDto();
                                    phone.PersonName = item.person_name;
                                    phone.PhoneNumber = item.phone_number;
                                    phone.Extension = param.WindowsUser.Substring(0, 3);
                                    result.PhoneNumber.Add(phone);
                                    i += 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        ContactsDto contacts = new ContactsDto();
                        contacts.code = phoneNumberResult.PhoneNumber;
                        IEnumerable<iMindContactInfoDto> iMindContactInfo = iMind.Top1ContactPhoneiMind(contacts);
                        if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                        {
                            DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                            if (doctor != null)
                            {
                                iMindContactInfo.ElementAt(0).phone_number = doctor.PhoneNumber;
                                iMindContactInfo.ElementAt(0).person_name = doctor.PhoneNumber;
                                iMindContactInfo.ElementAt(0).code = doctor.DoctorCode;
                            }
                        }
                        if (iMindContactInfo.Count() > 0)
                        {
                            iMindCustomerInfoDto iMindCust = new iMindCustomerInfoDto();
                            iMindCust.code = iMindContactInfo.ElementAt(0).code;
                            iMindCust.contact_id = iMindInfo.ElementAt(0).contact_id;
                            iMindCust.CustName = iMindInfo.ElementAt(0).name;
                            iMindCust.CustName1 = iMindContactInfo.ElementAt(0).name;
                            iMindCust.phone_number = iMindContactInfo.ElementAt(0).phone_number;
                            iMindCust.note = iMindContactInfo.ElementAt(0).note;
                            imindCust.Remark = phoneNumberResult.CustName;
                            imindCust.VNZ0Code = phoneNumberResult.VNZ0Code;
                            result.iMindCustomerInfo.Add(iMindCust);

                            if (callingTime.Count() > 0)
                            {
                                string[] priority1 = callingTime.ElementAt(0).Priority1.ToString().Split('-');
                                string startTime = priority1[0];
                                string endTime = priority1[1];
                                DateTime dtNow = DateTime.Now;
                                DateTime startDate = DateTime.ParseExact(dtNow.ToString(yyyy_MM_dd) + " " + startTime.Trim(), yyyy_MM_dd_HH_mm, CultureInfo.InvariantCulture);
                                DateTime endDate = DateTime.ParseExact(dtNow.ToString(yyyy_MM_dd) + " " + endTime.Trim(), yyyy_MM_dd_HH_mm, CultureInfo.InvariantCulture);
                                CallingTimeDto calltime = new CallingTimeDto();
                                if (dtNow < startDate || dtNow > endDate)
                                {
                                    calltime.Priority1 = callingTime.ElementAt(0).Priority1.ToString();
                                    calltime.Priority2 = callingTime.ElementAt(0).Priority2.ToString();
                                    calltime.isCallback = isCallback;
                                    result.CallingTime.Add(calltime);
                                }
                                IEnumerable<iMindContactInfoDto> ContactPhone = iMind.ContactPhoneiMind(contacts);
                                if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                                {
                                    DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                                    if (doctor != null)
                                    {
                                        ContactPhone.ElementAt(0).phone_number = doctor.PhoneNumber;
                                        ContactPhone.ElementAt(0).person_name = doctor.PhoneNumber;
                                        ContactPhone.ElementAt(0).code = doctor.DoctorCode;
                                    }
                                }
                                if (ContactPhone.Count() > 0)
                                {
                                    PhoneNumberDto phone = null;
                                    foreach (iMindContactInfoDto item in ContactPhone)
                                    {
                                        phone = new PhoneNumberDto();
                                        phone.PersonName = item.person_name;
                                        phone.PhoneNumber = item.phone_number;
                                        result.PhoneNumber.Add(phone);
                                    }
                                }
                            }
                            else
                            {
                                IEnumerable<iMindContactInfoDto> ContactPhone = iMind.ContactPhoneiMind(contacts);
                                if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                                {
                                    DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                                    if (doctor != null)
                                    {
                                        ContactPhone.ElementAt(0).phone_number = doctor.PhoneNumber;
                                        ContactPhone.ElementAt(0).person_name = doctor.PhoneNumber;
                                        ContactPhone.ElementAt(0).code = doctor.DoctorCode;
                                    }
                                }
                                if (ContactPhone.Count() > 0)
                                {
                                    PhoneNumberDto phone = null;
                                    foreach (iMindContactInfoDto item in ContactPhone)
                                    {
                                        phone = new PhoneNumberDto();
                                        phone.PersonName = item.person_name;                                        
                                        phone.PhoneNumber = item.phone_number;
                                        result.PhoneNumber.Add(phone);
                                    }
                                }
                            }
                        }
                        else
                        {
                            IEnumerable<iMindContactInfoDto> ContactCustomer = iMind.ContactCustomer(contacts);
                            iMindCustomerInfoDto iMindCust = new iMindCustomerInfoDto();
                            if (!string.IsNullOrEmpty(param.CustCode) && param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                            {
                                DoctorInformationDto doctor = campaignDao.GetDoctorInformation(param);
                                if (doctor != null)
                                {
                                    ContactCustomer.ElementAt(0).phone_number = doctor.PhoneNumber;
                                    ContactCustomer.ElementAt(0).person_name = doctor.PhoneNumber;
                                    ContactCustomer.ElementAt(0).code = doctor.DoctorCode;
                                }
                            }
                            if (ContactCustomer.Count() > 0)
                            {
                                
                                iMindCust.code = ContactCustomer.ElementAt(0).code;
                                iMindCust.contact_id = iMindInfo.ElementAt(0).contact_id;
                                iMindCust.CustName = ContactCustomer.ElementAt(0).name;
                                iMindCust.addr_detail = ContactCustomer.ElementAt(0).addr_detail;
                                iMindCust.note = ContactCustomer.ElementAt(0).note;
                                imindCust.Remark = phoneNumberResult.CustName;
                                imindCust.VNZ0Code = phoneNumberResult.VNZ0Code;
                            }
                            else
                            {
                                IEnumerable<ContactsDto> Contact = iMind.Contacts(contacts);
                                if (Contact.Count() > 0)
                                {
                                    iMindCust.code = Contact.ElementAt(0).code;
                                    iMindCust.contact_id = iMindInfo.ElementAt(0).contact_id;
                                    iMindCust.CustName = Contact.ElementAt(0).name;
                                    iMindCust.CustName1 = Contact.ElementAt(0).name;
                                }
                            }
                            result.iMindCustomerInfo.Add(imindCust);
                        }
                    }
                    // check exist survey by customer code
                    PhoneInfo phoneIf = new PhoneInfo();
                    IEnumerable<SearchCallDto> call = campaignDao.CheckSurveyDuplicate(param);
                    if (call != null && call.Count() > 0)
                    {
                        phoneIf.agentNumber = call.ElementAt(0).Agent;
                        phoneIf.date = call.ElementAt(0).CallingDate;
                        phoneIf.type = 5;
                        result.PhoneInfo = phoneIf;
                    }
                    
                    // end phone info
                    ContactsDto obh = new ContactsDto();
                    obh.code = phoneNumberResult.PhoneNumber;
                    IEnumerable<DoCampaignResultDto> OutboundHistory = campaignDao.OutboundHistory(obh);
                    result.OutboundHistory = OutboundHistory.ToList();
                    //CustomerLicensesDto customerLicenses = new CustomerLicensesDto();
                    //customerLicenses.CustCode = phoneNumberResult.PhoneNumber;
                    //result.CustomerLicenses = customerLicensesDao.GetLicenseStatus(customerLicenses);
                    result.ServiceTypeList = GetCampaignFocusByCustomer(param).DistinctBy(s => s.ID).ToList();
                }// end phone number result check not null
                result.PhoneNumberObj = phoneNumberResult;
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Load PhoneNumber
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public LoadPhoneNumberResult LoadPhoneNumber(OBSurveyParam param)
        {
            LoadPhoneNumberResult result = new LoadPhoneNumberResult();
            try
            {
                
                result = GetNextAssign(param);
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Quick Call
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto QuickCall(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            SurveyVoteDto surveyVote = new SurveyVoteDto();
            try
            {
                surveyVote.StartTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                result.SurveyVote = surveyVote;
                hrEmployeeDto empDto = new hrEmployeeDto();
                empDto.ID = FGuid.ToGuid(param.UserId);
                hrEmployeeDto profile = hrEmpDao.GetEmployeeDetail(empDto);
                PhoneNumberDto phoneNumberDto = new PhoneNumberDto();
                phoneNumberDto.ID = Guid.NewGuid();
                phoneNumberDto.ServiceType = param.ServiceId;
                phoneNumberDto.PhoneGroup = param.WindowsUser;
                phoneNumberDto.CreatedBy = param.WindowsUser;
                phoneNumberDto.IsDeleted = false;
                if (!string.IsNullOrEmpty(param.CustCode))
                {                                        
                    phoneNumberDto.PhoneNumber = param.CustCode;
                    phoneNumberDto.UpdatedBy = Constant.QUICK_CALLING;
                    phoneNumberDto.ProjectId = param.ProjectID;
                    PhoneNumberDto vnz0code = phoneNumberDao.GetCustomerVime(param); // VNZ0code
                    if (vnz0code != null && !string.IsNullOrEmpty(vnz0code.CustCode))
                    {
                        param.CustCode = vnz0code.CustCode;
                        phoneNumberDto.PhoneNumber = vnz0code.CustCode;
                    }
                    int insrt = phoneNumberDao.InsertPhoneNumber(phoneNumberDto);
                    LoadPhoneNumberResult loadphone = LoadPhoneNumber(param);
                    result.LoadPhone = loadphone;
                    result.ServiceTypeList = GetCampaignFocusByCustomer(param).DistinctBy(s => s.ServiceCode).ToList();
                }
                else if (param.CallPlan == true)
                {
                    CallPlanDto callPlanDto = new CallPlanDto();
                    callPlanDto.salesman_code = profile.EmployeeID;
                    CallPlanDto callPlan = callPlanDao.Top1CallPlan(callPlanDto);
                    if (callPlan != null)
                    {
                        phoneNumberDto.PhoneNumber = callPlan.cust_code;
                        phoneNumberDto.UpdatedBy = Constant.CALL_PLAN;
                        // insert phone number
                        int insrt = phoneNumberDao.QuickCall(phoneNumberDto);
                        LoadPhoneNumberResult loadphone = LoadPhoneNumber(param);
                        result.LoadPhone = loadphone;
                        // update call plan
                        callPlanDto.callplan_id = callPlan.callplan_id;
                        int udt = callPlanDao.UpdateIsAssignCallPlan(callPlanDto);
                        // insert call plan result
                        CallPlanResultDto callPlanResultDto = new CallPlanResultDto();
                        callPlanResultDto.callresult_id = Guid.NewGuid();
                        callPlanResultDto.callplan_id = callPlan.callplan_id;
                        callPlanResultDto.salesman_code = callPlan.salesman_code;
                        callPlanResultDto.cust_code = callPlan.cust_code;
                        int ist = callPlanDao.InsertCallPlanResult(callPlanResultDto);
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

        /// <summary>
        /// Get Call By Agent
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto GetAgentByUserName(hrEmployeeDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            hrEmployeeDto hrEmployee = new hrEmployeeDto();
            try
            {
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                OBSurveyParam obs = new OBSurveyParam()
                {
                    SalesmanCode = param.EmployeeID,
                    SurveyDate = currDate
                };
                CallPlanDto cp = new CallPlanDto()
                {
                    route_date = currDate,
                    salesman_code = param.EmployeeID
                };                
                result.CallPlan = callPlanDao.CallPlanBySaleman(cp);
                hrEmployee = hrEmpDao.GetEmployeeByUserName(param);
                IEnumerable<SearchCallDto> searchCall = searchCallDao.GetCallByAgent(param);
                param.AgentNumber = hrEmployee.AgentNumber;
                //result.CallOnPlan = searchCallDao.CallOnPlan(obs);
                //result.CallUnPlan = searchCallDao.CallUnPlan(obs);
                result.hrEmployee = hrEmployee;
                result.SearchCall = searchCall;
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// DeleteSurvey
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteSurvey(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                SurveyVoteDto surveyDto = new SurveyVoteDto();
                if (param.SurveyId == null || param.SurveyId == Guid.Empty)
                {
                    surveyDto.ID = param.Id;
                }
                else
                {
                    surveyDto.ID = (Guid)param.SurveyId;
                }
                result.number = campaignDao.DeleteSurvey(surveyDto);
                ZaloTrackingDto zalo = new ZaloTrackingDto();
                zalo.Id = param.ZaloId;
                zalo.SurveyId = surveyDto.ID;
                int zaloDel = zaloDao.DeleteZaloTracking(zalo);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE, "1");
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
        /// GetQuestionAnswerBySurvey
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ResultMessageDto GetQuestionAnswerBySurvey(OBSurveyParam para)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {               
                if (para.ServiceTypeId != Guid.Empty && (string.IsNullOrEmpty(para.SurveyId.ToString()) || para.SurveyId == Guid.Empty))
                {
                    string code = string.Empty;
                    SurveyVoteDto temp = null;
                    hrEmployeeDto hrObje = new hrEmployeeDto();
                    hrObje.ID = FGuid.ToGuid(para.UserId);
                    hrObje = hrEmpDao.GetEmployeeDetail(hrObje);
                    string[] strTemp = new string[2];
                    if (!string.IsNullOrEmpty(hrObje.AgentNumber))
                    {
                        strTemp[0] = hrObje.AgentNumber;
                    }
                    ServiceTypeDto Sobj = serviceTypeDao.GetServiceTypeByID(para);
                    result.ServiceType = Sobj;
                    if (Sobj != null)
                    {
                        code = Sobj.ServiceCode;
                    }
                    if (code != "03" && code != "04") // khong la MB, FC
                    {
                        temp = surveyVoteDao.GetByPhoneNumber(para);
                    }
                    else // la MB hoac FC
                    {
                        temp = surveyVoteDao.GetByPhoneNumberNotCode(para);
                        if (temp != null) // la MB hoac FC nhung da goi trong trong dich vu khac roi
                        {
                            strTemp[1] = temp.SurveyDate;
                            result.code = Constant.ERROR;
                            result.message = string.Format(Constant.PHONE_NUMBER_ALREADY_SURVEY_AGENT, strTemp);
                        }
                        else
                        {
                            if (code == "03")// dang chon dich vu MB
                            {
                                // kiem tra da goi MB chua
                                para.ServiceCode = "03";
                                temp = surveyVoteDao.GetByPhoneNumberService(para);
                                if (temp != null)
                                {
                                    result.code = Constant.ERROR;
                                    result.message = string.Format(Constant.PHONE_NUMBER_ALREADY_SURVEY_MOBI_INTERNET, strTemp);
                                }
                            }
                            else if (code == "04")// dang khao sat dich vu FC
                            {
                                para.ServiceCode = "04";
                                temp = surveyVoteDao.GetByPhoneNumberService(para);
                                if (temp != null)
                                {                                        
                                    result.code = Constant.ERROR;
                                    result.message = string.Format(Constant.PHONE_NUMBER_ALREADY_SURVEY_FAST_CONNECT, strTemp);
                                }
                            }
                        }
                    }                    
                }

                // get campaign whether customer focus
                List<Guid> serviceTypeList = new List<Guid>();
                List<string> focus = new List<string>();
                if (!string.IsNullOrEmpty(para.FocusCampaignID))
                {
                    focus = para.FocusCampaignID.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                }
                SurveyVoteDto surveyVote = null;
                if (para.SurveyVoteID != null && para.SurveyVoteID != Guid.Empty)
                {
                    surveyVote = surveyVoteDao.GetSurveyVote(para);
                    if (surveyVote != null)
                    {
                        para.CustCode = surveyVote.CustomerPhone;
                    }
                }
                List<ServiceTypeDto> service_type_focus = new List<ServiceTypeDto>();
                if (!string.IsNullOrEmpty(para.CustCode))
                {
                    //// Get campaign focus
                    //CallPlanByCampaignDto focus = new CallPlanByCampaignDto()
                    //{
                    //    CustCode = para.CustCode,
                    //    GroupCode = para.GroupCode
                    //};
                    //IEnumerable<CallPlanByCampaignDto> focus_campaign = campaignDao.CustomerFocusCampaign(focus);
                    //result.FocusCampaign = focus_campaign;
                    //// get service type list by focus campaign from customer
                    //OSSCallPlanDto oss = new OSSCallPlanDto()
                    //{
                    //    cust_code = para.CustCode,
                    //    user_ext = para.UserExt,
                    //    group_code = para.GroupCode
                    //};
                    
                    foreach (string sv in focus)
                    {
                        if (!string.IsNullOrEmpty(sv))
                        {
                            ServiceTypeDto fc = new ServiceTypeDto()
                            {
                                ID = Guid.Parse(sv)
                            };
                            fc = serviceTypeDao.GetServiceTypeByID(fc);
                            if (!para.ServiceTypeId.ToString().Equals(fc.ID.ToString()))
                            {
                                fc.IsFocusCampaign = true;
                            }
                            else
                            {
                                fc.IsFocusCampaign = false;
                            }
                            service_type_focus.Add(fc);
                        }
                    }
                    //
                    //SearchCallDto sc = new SearchCallDto()
                    //{
                    //    CustomerCode = para.CustCode
                    //};
                    //result.FocusCampaignHistoryByCustomer = FocusCampaignHistory(sc);
                }                
                // Get all campaign by SurveyVoteID
                List<AnswerDto> AnswerList = null;
                AnswerDto answerDto = null;
                List<string> RadionButtonList = null;                
                CampaignScriptResult dataSurvey = null;
                //result.HrEmployee = campaignDao.GetGroupIDByWindowsUser(para); // Get Group ID By window user
                //if (result.HrEmployee != null && result.HrEmployee.Count() > 0)
                //{
                //    para.ProjectID = result.HrEmployee.ElementAt(0).GroupID;
                //}
                result.ServiceTypeList = campaignDao.GetActiveServiceType(para); // get service type list by project
                if (service_type_focus.Count() > 0)
                {
                    result.ServiceTypeList = service_type_focus.DistinctBy(s => s.ServiceCode);
                }
                if (result.ServiceTypeList.Count() >  0)
                {
                    foreach (ServiceTypeDto item in result.ServiceTypeList)
                    {
                        dataSurvey = new CampaignScriptResult();
                        para.ServiceTypeId = item.ID;
                        result.ServiceQuestionList = serviceQuestionsDao.GetServiceQuestionsByServiceType(para);
                        AnswerList = new List<AnswerDto>();
                        answerDto = new AnswerDto();
                        RadionButtonList = new List<string>();
                        // Get answer by question
                        if (!string.IsNullOrEmpty(para.SurveyVoteID.ToString()) || para.SurveyVoteID != Guid.Empty)
                        {
                            answerDto.SurveyVoteID = para.SurveyVoteID;
                            for (int i = 0; i < result.ServiceQuestionList.Count(); i++)
                            {
                                answerDto.QuestionID = result.ServiceQuestionList.ElementAt(i).QuestionID;
                                result.ServiceQuestionList.ElementAt(i).AnswerList = answerDao.GetAnswerByQuestionID(answerDto);
                                int ans = 0;
                                foreach (AnswerDto answer in result.ServiceQuestionList.ElementAt(i).AnswerList)
                                {
                                    if (answer.AnswerTypeCode.Equals("4"))
                                    {
                                        for (int n = 0; n < answer.MaxValue; n++)
                                        {
                                            result.ServiceQuestionList.ElementAt(i).AnswerList.ElementAt(ans).RadionButtonList.Add("rdType4_" + n);
                                        }
                                    }
                                    ans += 1;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < result.ServiceQuestionList.Count(); j++)
                            {
                                para.QuestionID = result.ServiceQuestionList.ElementAt(j).QuestionID;
                                result.ServiceQuestionList.ElementAt(j).AnswerList = answerDao.GetByQuestionIDNew(para);
                                int ans = 0;
                                foreach (AnswerDto answer in result.ServiceQuestionList.ElementAt(j).AnswerList)
                                {
                                    if (answer.AnswerTypeCode.Equals("4"))
                                    {
                                        for (int n = 0; n < answer.MaxValue; n++)
                                        {
                                            result.ServiceQuestionList.ElementAt(j).AnswerList.ElementAt(ans).RadionButtonList.Add("rdType4_" + n);
                                        }
                                    }
                                    ans += 1;
                                }
                            }
                        }
                        // add data survey for earch campaign
                        if (para.SurveyVoteID != null && para.SurveyVoteID != Guid.Empty)
                        {
                            SurveyResultDto surveyResult = new SurveyResultDto();
                            surveyResult.SurveyVoteID = (Guid)para.SurveyVoteID;
                            List<SurveyResultDto> surveyResultList = surveyResultDao.GetSurveyResult(surveyResult).ToList();
                            result.SurveyResult = surveyResultList;
                            dataSurvey.SurveyResult = surveyResultList;
                        }
                        dataSurvey.ServiceQuestionList = result.ServiceQuestionList.ToList();
                        dataSurvey.Campaign = item;
                        result.CampaignScriptResult.Add(dataSurvey);
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


        /// <summary>
        /// check customer whether focus in campaign
        /// </summary>
        /// <param name="oss"></param>
        /// <returns></returns>
        public IEnumerable<OSSCallPlanDto> CheckCustomerWhetherFocusCampaign(OSSCallPlanDto oss)
        {
            IEnumerable<OSSCallPlanDto> oss_lst = callPlanDao.CheckCustomerWhetherFocusCampaign(oss);
            return oss_lst;
        }


        /// <summary>
        /// get survey by id
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public LoadPhoneNumberResult GetSurveyById(OBSurveyParam para)
        {
            LoadPhoneNumberResult result = new LoadPhoneNumberResult();
            ContactsDto contact = new ContactsDto();
            List<PhoneNumberDto> phonenumber = new List<PhoneNumberDto>();
            SurveyVoteDto surveyVote = null;
            PhoneInfo phoneInfo = new PhoneInfo();
            try
            {
                PhoneNumberDto vnz0code = phoneNumberDao.GetCustomerVime(para); // VNZ0code
                if (vnz0code != null && !string.IsNullOrEmpty(vnz0code.CustCode))
                {
                    para.CustCode = vnz0code.CustCode;
                }
                if (para.SurveyVoteID != null && para.SurveyVoteID != Guid.Empty)
                {
                    SurveyResultDto surveyResult = new SurveyResultDto()
                    {
                        SurveyVoteID = (Guid)para.SurveyVoteID
                    };
                    surveyResult.SurveyVoteID = (Guid)para.SurveyVoteID;
                    List<SurveyResultDto> surveyResultList = surveyResultDao.GetSurveyResult(surveyResult).ToList();
                    result.SurveyResult = surveyResultList;
                }
                surveyVote = surveyVoteDao.GetSurveyVote(para);
                result.SurveyVote = surveyVote;
                if (surveyVote != null)
                {
                    List<iMindCustomerInfoDto> iMindCustomerInfo = new List<iMindCustomerInfoDto>();
                    List<iMindContactInfoDto> iMindContact = new List<iMindContactInfoDto>();
                    contact.code = surveyVote.CustomerPhone; // CustCode                    
                    // begin abbott customize 24-04-2020
                    if (!string.IsNullOrEmpty(contact.code) && contact.code.Substring(0,3).Equals(Constant.DOCTOR_ABBOTT))
                    {
                        para.CustCode = surveyVote.CustomerPhone;
                        DoctorInformationDto doctor = campaignDao.GetDoctorInformation(para);
                        if (doctor != null)
                        {
                            iMindCustomerInfoDto info = new iMindCustomerInfoDto()
                            {
                                code = doctor.DoctorCode,
                                name = doctor.DoctorNameEN,
                                phone_number = doctor.PhoneNumber                                
                            };
                            iMindContactInfoDto contactInfo = new iMindContactInfoDto()
                            {
                                person_name = doctor.DoctorNameEN,
                                phone_number = doctor.PhoneNumber
                            };
                            iMindCustomerInfo.Add(info);
                            iMindContact.Add(contactInfo);
                        }
                    }
                    else
                    {
                        iMindCustomerInfo = iMind.CustomeriMindInfo(contact).ToList();
                        iMindContact = iMind.ContactPhoneiMind(contact).ToList();
                    }
                    // end abbott customize 24-04-2020
                    para.CustCode = surveyVote.CustomerPhone; // set customer code calling time
                    IEnumerable<CallingTimeDto> callingTime = callingTimeDao.CallingTime(para);
                    result.iMindCustomerInfo = iMindCustomerInfo;
                    result.CallingTime = callingTime.ToList();
                    PhoneNumberDto contactPhone = null;
                    foreach(iMindContactInfoDto item in iMindContact)
                    {
                        contactPhone = new PhoneNumberDto();
                        contactPhone.PersonName = item.person_name;
                        contactPhone.PhoneNumber = item.phone_number;
                        phonenumber.Add(contactPhone);
                    }
                    result.PhoneNumber = phonenumber;
                    // Get Zalo Tracking By Survey ID
                    //ZaloTrackingDto zaloDto = new ZaloTrackingDto();
                    //zaloDto.SurveyId = para.SurveyId;
                    //zaloDto.ServiceTypeId = surveyVote.ServiceTypeID;
                    //zaloDto = zaloDao.GetZaloById(zaloDto);
                    //if (zaloDto != null && zaloDto.ImageCode != null && zaloDto.ImageCode.Length > 0)
                    //{
                    //    byte[] data = new byte[0];
                    //    data = zaloDto.ImageCode;
                    //    MemoryStream mem = new MemoryStream(data);
                    //    zaloDto.ImageSrc = Convert.ToBase64String(data);
                    //}
                    //result.ZaloDto = zaloDto;
                }
                // initialize abbott doctor information
                SurveyVoteDto sv = null;
                if (!string.IsNullOrEmpty(para.PhoneNumber) && para.PhoneNumber.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                {
                    DoctorInformationDto doctor = campaignDao.GetDoctorInformation(para);
                    sv = new SurveyVoteDto()
                    {
                        CustomerName = doctor.DoctorNameVN,
                        CustomerPhone = doctor.PhoneNumber,
                        EmployeeID = para.UserId
                    };
                }
                else
                {
                    para.PhoneNumber = para.CustCode;
                    sv = surveyVoteDao.GetByPhoneNumber(para);
                }
                if (sv != null)
                {
                    hrEmployeeDto hrDto = new hrEmployeeDto();
                    hrDto.ID = Guid.Parse(sv.EmployeeID.ToString());
                    hrEmployeeDto hrObj = hrEmpDao.GetEmployeeDetail(hrDto);
                    string now = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!string.IsNullOrEmpty(sv.SurveyDate) && sv.SurveyDate.Equals(now))
                    {
                        phoneInfo.type = 5;
                        phoneInfo.date = sv.SurveyDate;
                        if (hrObj != null)
                        {
                            phoneInfo.agentNumber = hrObj.AgentNumber;
                        }
                    }
                    else
                    {
                        phoneInfo.type = 2;
                        phoneInfo.date = sv.SurveyDate;
                        if (hrObj != null)
                        {
                            phoneInfo.agentNumber = hrObj.AgentNumber;
                        }
                    }
                }
                else
                {
                    phoneInfo.type = 0;
                }
                // Campaign result
                CampaignScriptResult campaignScript = null;
                result.HrEmployee = campaignDao.GetGroupIDByWindowsUser(para); // Get Group ID By window user
                if (result.HrEmployee != null && result.HrEmployee.Count() > 0)
                {
                    para.ProjectID = result.HrEmployee.ElementAt(0).GroupID;
                }
                result.ServiceTypeList = campaignDao.GetActiveServiceType(para); // get service type list by project
                //result.ServiceTypeList = GetCampaignFocusByCustomer(para).ToList().DistinctBy(s => s.ServiceCode);
                if (surveyVote != null)
                {
                    foreach (ServiceTypeDto item in result.ServiceTypeList)
                    {
                        campaignScript = new CampaignScriptResult();
                        para.ServiceTypeId = item.ID;
                        ServiceTypeDto Sobj = serviceTypeDao.GetServiceTypeByID(para);
                        result.ServiceType = Sobj;
                        campaignScript.Campaign = item;
                        result.CampaignScriptResult.Add(campaignScript);
                    }
                }
                //CustomerLicensesDto customerLicenses = new CustomerLicensesDto();
                //customerLicenses.CustCode = para.CustCode;
                //result.CustomerLicenses = customerLicensesDao.GetLicenseStatus(customerLicenses);                
                result.PhoneInfo = phoneInfo;
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        

        /// <summary>
        /// SaveSurveyByQuestion
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultMessageDto SaveSurveyByQuestion(DataSurvey param, OBSurveyParam outbound)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {                
                List<string> focus = new List<string>();
                if (!string.IsNullOrEmpty(outbound.FocusCampaignID))
                {
                    focus = outbound.FocusCampaignID.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                }
                // Delete old survey result
                SurveyResultDto delete = new SurveyResultDto();
                Guid SurveyVoteID = Guid.NewGuid();
                if (param != null && param.DataSurveyCampaign.Count() > 0)
                {
                    // delete survey result to re-insert
                    if (param.DataSurveyCampaign.ElementAt(0).SurveyVoteID == null || param.DataSurveyCampaign.ElementAt(0).SurveyVoteID == Guid.Empty)
                    {
                        outbound.SurveyVoteID = SurveyVoteID;
                        delete.SurveyVoteID = SurveyVoteID;
                    }
                    else
                    {
                        delete.SurveyVoteID = (Guid)outbound.SurveyVoteID;
                    }
                    if (outbound.IsUpdate == true)
                    {
                        int del = surveyResultDao.DeleteBySurveyVoteId(delete);
                    }
                    SurveyVoteDto sv = new SurveyVoteDto();
                    if (outbound.IsUpdate == false)
                    {
                        sv.ID = SurveyVoteID;
                    }
                    else
                    {
                        sv.ID = (Guid)outbound.SurveyVoteID;
                    }
                    result.SurveyVote = sv;
                    // Insert new survey result
                    int cntRs = 0;
                    int idx = 0;
                    int i = 0;
                    EffectiveCampaignHistoryDto effective = null;
                    foreach(string item in focus)
                    {
                        if (!param.DataSurveyCampaign.ElementAt(i).ServiceTypeId.ToString().Equals(item))
                        {
                            DataCallFlow cf = new DataCallFlow()
                            {
                                ServiceTypeId = Guid.Parse(item)
                            };
                            param.DataSurveyCampaign.Add(cf);
                        }
                        i += 1;
                    }
                    param.DataSurveyCampaign.DistinctBy(s => s.ServiceTypeId);
                    foreach (DataCallFlow callFlow in param.DataSurveyCampaign)
                    {
                        effective = new EffectiveCampaignHistoryDto();
                        effective.ServiceTypeID = callFlow.ServiceTypeId;
                        effective.SurveyVoteID = (outbound.SurveyVoteID == null || outbound.SurveyVoteID == Guid.Empty) ? SurveyVoteID : (Guid)outbound.SurveyVoteID;
                        effective.CreateBy = param.DataSurveyCampaign.ElementAt(0).UserName;
                        idx += campaignDao.SaveEffectiveCampaignHistory(effective);
                        SurveyResultDto insertSR = null;
                        // type 1 checkbox
                        if (callFlow.checkBox1 != null && callFlow.checkBox1.Count() > 0)
                        {
                            foreach (AnswerTypeParam checkbox in callFlow.checkBox1)
                            {
                                insertSR = new SurveyResultDto();
                                insertSR.ID = Guid.NewGuid();
                                insertSR.SurveyVoteID = (Guid)effective.SurveyVoteID;
                                insertSR.QuestionID = checkbox.QuestionID;
                                insertSR.AnswerID = checkbox.AnswerID;
                                insertSR.AnswerContent = checkbox.AnswerContent;
                                insertSR.IsDeleted = false;
                                cntRs += surveyResultDao.InsertSurveyResult(insertSR);
                            }
                        }
                        // type 2 radio
                        if (callFlow.radiobutton2 != null && callFlow.radiobutton2.Count() > 0)
                        {
                            foreach (AnswerTypeParam radio in callFlow.radiobutton2)
                            {
                                insertSR = new SurveyResultDto();
                                insertSR.ID = Guid.NewGuid();
                                insertSR.SurveyVoteID = (Guid)effective.SurveyVoteID;
                                insertSR.QuestionID = radio.QuestionID;
                                insertSR.AnswerID = radio.AnswerID;
                                insertSR.AnswerContent = radio.AnswerContent;
                                insertSR.IsDeleted = false;
                                cntRs += surveyResultDao.InsertSurveyResult(insertSR);
                            }
                        }
                        // type 3 textbox
                        if (callFlow.textbox3 != null && callFlow.textbox3.Count() > 0)
                        {
                            foreach (AnswerTypeParam text3 in callFlow.textbox3)
                            {
                                insertSR = new SurveyResultDto();
                                insertSR.ID = Guid.NewGuid();
                                insertSR.SurveyVoteID = (Guid)effective.SurveyVoteID;
                                insertSR.QuestionID = text3.QuestionID;
                                insertSR.AnswerID = text3.AnswerID;
                                insertSR.AnswerContent = text3.AnswerContent;
                                insertSR.IsDeleted = false;
                                cntRs += surveyResultDao.InsertSurveyResult(insertSR);
                            }
                        }
                        // type 4 radion button
                        if (callFlow.radiobuttonlist4 != null && callFlow.radiobuttonlist4.Count() > 0)
                        {
                            foreach (AnswerTypeParam radioList in callFlow.radiobuttonlist4)
                            {
                                insertSR = new SurveyResultDto();
                                insertSR.ID = Guid.NewGuid();
                                insertSR.SurveyVoteID = (Guid)effective.SurveyVoteID;
                                insertSR.QuestionID = radioList.QuestionID;
                                insertSR.AnswerID = radioList.AnswerID;
                                insertSR.AnswerContent = radioList.AnswerContent;
                                insertSR.IsDeleted = false;
                                cntRs += surveyResultDao.InsertSurveyResult(insertSR);
                            }
                        }
                        // type 5 textbox
                        if (callFlow.textbox5 != null && callFlow.textbox5.Count() > 0)
                        {
                            foreach (AnswerTypeParam text5 in callFlow.textbox5)
                            {
                                insertSR = new SurveyResultDto();
                                insertSR.ID = Guid.NewGuid();
                                insertSR.SurveyVoteID = (Guid)effective.SurveyVoteID;
                                insertSR.QuestionID = text5.QuestionID;
                                insertSR.AnswerID = text5.AnswerID;
                                insertSR.AnswerContent = text5.AnswerContent;
                                insertSR.IsDeleted = false;
                                cntRs += surveyResultDao.InsertSurveyResult(insertSR);
                            }
                        }
                    }
                    
                    SurveyVoteDto svObj = GetSurveyVote(outbound);
                    // save oss call plan status
                    OSSCallPlanDto oss = new OSSCallPlanDto()
                    {
                        cust_code = outbound.CustCode,
                        user_ext = outbound.UserExt,
                        group_code = outbound.GroupCode
                    };
                    //IEnumerable<OSSCallPlanDto> focus_campaign = CheckCustomerWhetherFocusCampaign(oss);
                    foreach (string item in focus)
                    {
                        OSSCallPlanDto oss_udt = new OSSCallPlanDto()
                        {
                            indicator = Constant.INDICATOR_CALLPLAN,
                            cust_code = outbound.CustCode,
                            service_type_code = item,
                            project_id = outbound.ProjectID,
                            saleman_code = outbound.SalesmanCode,
                            user_ext = outbound.UserExt,
                            update_by = outbound.WindowsUser,
                            call_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            call_status = outbound.Status
                        };
                        callPlanDao.UpdateOSSCallPlan(oss_udt);
                    }
                    // assign service object
                    if (svObj.ID == null || svObj.ID == Guid.Empty)
                    {
                        svObj.ID = SurveyVoteID;
                    }
                    result.SurveyVote = svObj; // set Survey Vote result
                    if (idx > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    // ZALO TRACKING UPDATE | INSERT
                    // var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/FileUpload/ZaloTracking/"));
                    //if (outbound.IsUpdate == true && svObj != null)
                    //{
                    //    ZaloTrackingDto zalo = new ZaloTrackingDto();
                    //    zalo.SurveyId = svObj.ID;
                    //    zalo.ServiceTypeId = outbound.ServiceTypeId;
                    //    zalo = zaloDao.GetZaloById(zalo);
                    //    if (zalo != null)
                    //    {
                    //        zalo.UpdateBy = outbound.WindowsUser;
                    //        // Read the Base64 string from Hidden Field.
                    //        if (!string.IsNullOrEmpty(param.DataSurveyCampaign.ElementAt(0).ImageCode))
                    //        {
                    //            string base64 = param.DataSurveyCampaign.ElementAt(0).ImageCode.Replace("data:image/png;base64,", string.Empty);
                    //            //Convert Base64 string to Byte Array.
                    //            byte[] bytes = Convert.FromBase64String(base64);
                    //            //Save the Byte Array as Image File.
                    //            //string str = string.Format(root + "{0}.png", zalo.ImageName);
                    //            //File.WriteAllBytes(str, bytes);
                    //            zalo.ImageCode = bytes;
                    //            zalo.ImageStatus = true;
                    //            int udt = zaloDao.UpdateZaloTracking(zalo);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        string name = svObj.ID + "_" + outbound.WindowsUser + "_" + outbound.PhoneNumber + "_" + DateTime.Now.ToString("yyyyMMdd");
                    //        //string filePath = root + "/" + name;
                    //        if (!string.IsNullOrEmpty(param.DataSurveyCampaign.ElementAt(0).ImageCode))
                    //        {
                    //            ZaloTrackingDto zaloDto = new ZaloTrackingDto();
                    //            zaloDto.Id = Guid.NewGuid();
                    //            zaloDto.SurveyId = svObj.ID;
                    //            zaloDto.ServiceTypeId = outbound.ServiceTypeId;
                    //            //zaloDto.ImagePath = root;
                    //            zaloDto.ImageName = name;
                    //            zaloDto.CreateBy = outbound.WindowsUser;
                    //            zaloDto.CustCode = param.DataSurveyCampaign.ElementAt(0).CustCode;
                    //            zaloDto.EmployeeId = outbound.UserId;

                    //            string base64 = param.DataSurveyCampaign.ElementAt(0).ImageCode.Replace("data:image/png;base64,", string.Empty);
                    //            //Convert Base64 string to Byte Array.
                    //            byte[] bytes = Convert.FromBase64String(base64);
                    //            //Save the Byte Array as Image File.
                    //            //string str = string.Format(root + "{0}.png", zaloDto.ImageName);
                    //            //File.WriteAllBytes(str, bytes);
                    //            zaloDto.ImageCode = bytes;
                    //            int cnt = zaloDao.InsertZaloTracking(zaloDto);
                    //            zaloDto.ImageStatus = true;
                    //            zaloDao.UpdateImageCode(zaloDto);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    string name = sv.ID + "_" + outbound.WindowsUser + "_" + outbound.PhoneNumber + "_" + DateTime.Now.ToString("yyyyMMdd");
                    //    //string filePath = root + "/" + name;
                    //    svObj.ID = sv.ID;
                    //    //Read the Base64 string from param
                    //    if (!string.IsNullOrEmpty(param.DataSurveyCampaign.ElementAt(0).ImageCode))
                    //    {
                    //        ZaloTrackingDto zaloDto = new ZaloTrackingDto();
                    //        zaloDto.Id = Guid.NewGuid();
                    //        zaloDto.SurveyId = sv.ID;
                    //        zaloDto.ServiceTypeId = outbound.ServiceTypeId;
                    //        //zaloDto.ImagePath = root;
                    //        zaloDto.ImageName = name;
                    //        zaloDto.CreateBy = outbound.WindowsUser;
                    //        zaloDto.CustCode = outbound.CustCode;
                    //        zaloDto.EmployeeId = outbound.UserId;
                    //        string base64 = param.DataSurveyCampaign.ElementAt(0).ImageCode.Replace("data:image/png;base64,", string.Empty);
                    //        //Convert Base64 string to Byte Array.
                    //        byte[] bytes = Convert.FromBase64String(base64);
                    //        //Save the Byte Array as Image File.
                    //        //string str = string.Format(root + "{0}.png", zaloDto.ImageName);
                    //        //File.WriteAllBytes(str, bytes);
                    //        zaloDto.ImageCode = bytes;
                    //        int cnt = zaloDao.InsertZaloTracking(zaloDto);
                    //        zaloDto.ImageStatus = true;
                    //        zaloDao.UpdateImageCode(zaloDto);
                    //    }
                    //}

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
        /// search survey by agent
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto SearchSurveyByAgent(SearchCallParam json)
        {
            ResultMessageDto result = new ResultMessageDto();
            IEnumerable<SearchCallDto> searchCall = searchCallDao.SearchSurveyByAgent(json);
            OBSurveyParam obs = new OBSurveyParam()
            {
                SalesmanCode = json.EmployeeID,
                SurveyDate = Utils.StringDateFormat(json.SurveyDate)
            };
            CallPlanDto cp = new CallPlanDto()
            {
                route_date = Utils.StringDateFormat(json.SurveyDate),
                salesman_code = json.EmployeeID
            };
            result.CallPlan = callPlanDao.CallPlanBySaleman(cp);
            result.CallOnPlan = searchCallDao.CallOnPlan(obs);
            result.CallUnPlan = searchCallDao.CallUnPlan(obs);
            result.SearchCall = searchCall;
            return result;
        }

        /// <summary>
        /// Focus campaign history
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public List<FocusCampaignHistoryDto> FocusCampaignHistory(SearchCallDto json)
        {
            List<FocusCampaignHistoryDto> focushistory = new List<FocusCampaignHistoryDto>();
            IEnumerable<SearchCallDto> resultCampaign = searchCallDao.SearchEffectiveCampaignHistoryByCustomer(json);
            FocusCampaignHistoryDto focus = null;
            foreach (SearchCallDto item in resultCampaign)
            {
                focus = new FocusCampaignHistoryDto();
                focus.ServiceName = item.ServiceName;
                focus.SurveyDate = item.CallingDate;
                AnswerDto a = new AnswerDto()
                {
                    ServiceTypeID = item.ServiceTypeID
                };
                IEnumerable<AnswerDto> answers = answerDao.GetQuestionAnswerByServiceType(a);
                foreach (AnswerDto ans in answers)
                {                    
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
                            focus.CustomerFeedback = surveyResult.ElementAt(0).AnswerContent;
                        }
                    }
                }
                focushistory.Add(focus);
            } // end foreach
            return focushistory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DoctorInformationDto GetDoctorInfo(OBSurveyParam para)
        {
            DoctorInformationDto doctor = new DoctorInformationDto();
            if (!string.IsNullOrEmpty(para.CustCode) && para.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
            {
                doctor = campaignDao.GetDoctorInformation(para);                
            }
            return doctor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IEnumerable<DoctorInformationDto> GetDoctorCallplan(OBSurveyParam para)
        {
            IEnumerable<DoctorInformationDto> doctor = campaignDao.GetDoctorCallplan(para);            
            return doctor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<ServiceTypeDto> GetCampaignFocusByCustomer(OBSurveyParam para)
        {
            List<ServiceTypeDto> service_type_focus = new List<ServiceTypeDto>();
            OSSCallPlanDto oss = new OSSCallPlanDto()
            {
                cust_code = para.CustCode,
                user_ext = para.UserExt,
                group_code = para.GroupCode
            };
            CampaignDto dto = new CampaignDto()
            {
                ProjectID = para.ProjectID
            };
            IEnumerable<ServiceTypeDto> svt = servicetypeDao.SearchCampaign(dto);
            foreach (ServiceTypeDto item in svt)
            {
                ServiceTypeDto sv = new ServiceTypeDto()
                {
                    ID = item.ID,//(Guid)para.ServiceTypeId;
                    IsFocusCampaign = false
                };
                sv = serviceTypeDao.GetServiceTypeByID(sv);
                service_type_focus.Add(sv);
            }
            IEnumerable<OSSCallPlanDto> oss_lst = CheckCustomerWhetherFocusCampaign(oss);            
            foreach (OSSCallPlanDto oss_item in oss_lst)
            {
                ServiceTypeDto focus = new ServiceTypeDto()
                {
                    ID = (Guid)oss_item.service_type_id                   
                };
                focus = serviceTypeDao.GetServiceTypeByID(focus);
                focus.IsFocusCampaign = true;
                service_type_focus.Add(focus);
            }
            
            return service_type_focus;
        }

        /// <summary>
        /// AddContactList
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto AddContactList(iMindContactInfoDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = campaignDao.AddContactList(json);
                if (result.number > 0)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultMessageDto SaveSpecialNote(iMindContactInfoDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = campaignDao.SaveSpecialNote(json);
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
        }
    }
}