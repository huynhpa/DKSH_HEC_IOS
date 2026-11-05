using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using MP.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.DoCampaign
{
    public class DoCampaignController : Controller
    {

        private DoCampaignService campaignService;
        private QuestionService questionService;
        private EffectionCallService effectionCallService;
        private AssignmentService assignmentService;
        private ServiceTypeService serviceTypeService;
        private CallPlanService callPlanService;

        public DoCampaignController()
            : this(new DoCampaignServiceImpl(), new QuestionServiceImpl(), 
                  new EffectionCallServiceImpl(), new AssignmentServiceImpl(), 
                  new ServiceTypeServiceImpl(), new CallPlanServiceImpl())
        {
        }
        public DoCampaignController(DoCampaignService campaignService, QuestionService questionService,
            EffectionCallService effectionCallService, AssignmentService assignmentService, 
            ServiceTypeService serviceTypeService, CallPlanService callPlanService)
        {
            this.campaignService = campaignService;
            this.questionService = questionService;
            this.effectionCallService = effectionCallService;
            this.assignmentService = assignmentService;
            this.serviceTypeService = serviceTypeService;
            this.callPlanService = callPlanService;
        }


        // GET: /DoCampaign/
        public ActionResult Index(string SurveyVoteID)
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                if (!string.IsNullOrEmpty(SurveyVoteID))
                {
                    ViewBag.SurveyVoteID = SurveyVoteID;
                }                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Get general data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult PageInitialize(OBSurveyParam param)
        {
            OutboundSurveyDataResult dataResult = new OutboundSurveyDataResult();
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            if (user != null)
            {
                param.WindowsUser = user.UserName;
                param.SalesmanCode = user.EmployeeID;
                dataResult = campaignService.GetGeneralSurveyData(param);
            }
            var jsonResult = Json(dataResult, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Load Service Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LoadServiceDetail(OBSurveyParam param)
        {
            IEnumerable<QuestionDto> result = questionService.LoadServiceDetail(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Quick Call
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult QuickCall(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            //param.ContactPerson = "HUYNH";
            //param.PhoneNumber = "0386228428";
            //param.UserExt = "227thotd";
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            try
            {
                if (usersession != null)
                {
                    param.UserId = usersession.UserId;
                    param.WindowsUser = usersession.UserName;
                    param.AgentID = usersession.UserId;
                    param.ProjectID = usersession.GroupID;
                    param.contracts = usersession.contracts;
                    param.UserExt = usersession.AgentNumber;
                    result = campaignService.QuickCall(param);
                    DoctorInformationDto doctor = new DoctorInformationDto();
                    if (param.CustCode != null && param.CustCode.Substring(0,3).Equals(Constant.DOCTOR_ABBOTT))
                    {
                        doctor = campaignService.GetDoctorInfo(param);
                    }
                    if (result.LoadPhone != null)
                    {
                        LoginAsteriskDto loginAsterisk = new LoginAsteriskDto();
                        loginAsterisk.UserExt = usersession.UserName.Substring(0, 3);
                        if (result.LoadPhone.iMindCustomerInfo != null)
                        {
                            if (result.LoadPhone != null && result.LoadPhone.PhoneNumber != null && result.LoadPhone.PhoneNumber.Count() > 0)
                            {
                                loginAsterisk.PhoneNumber = result.LoadPhone.PhoneNumber.ElementAt(0).PhoneNumber;
                            }
                            else if (param.CustCode.Substring(0, 3).Equals(Constant.DOCTOR_ABBOTT))
                            {
                                loginAsterisk.PhoneNumber = doctor.PhoneNumber;
                            }
                            //LoginAsterisk(loginAsterisk);
                        }
                    }
                }
                else
                {
                    result.code = Constant.SESSION_EXPIRED_CODE;
                    result.message = Constant.SESSION_EXPIRED_MESSAGE;
                }
            }
            catch(Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save survey
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveSurvey(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                param.UserId = usersession.UserId;
                param.WindowsUser = usersession.UserName;
                param.contracts = usersession.contracts;
                param.AgentID = usersession.UserId;
                result = campaignService.SaveSurvey(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Next Assign
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetNextAssign(OBSurveyParam param)
        {
            LoadPhoneNumberResult result = new LoadPhoneNumberResult();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (Session[Constant.USER_SESSION] != null)
            {
                param.WindowsUser = usersession.UserName;
                param.AgentID = usersession.UserId;
                param.ProjectID = usersession.GroupID;
                param.GroupCode = usersession.GroupCode;
                param.SalesmanCode = usersession.EmployeeID;
                param.contracts = usersession.contracts;
                param.UserExt = usersession.AgentNumber;
                result = campaignService.GetNextAssign(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Login Asterisk
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginAsterisk(LoginAsteriskDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (Session[Constant.USER_SESSION] != null)
            {                
                param.UserExt = usersession.AgentNumber;
                param.contracts = usersession.contracts;
                result = campaignService.LoginAsterisk(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Click 2 call
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Click2Call(Click2CallDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (Session[Constant.USER_SESSION] != null)
            {                
                if (param.phone_number.Substring(0, 1).Equals("9"))
                {
                    param.phone_number = param.phone_number.Remove(0, 1);
                }                
                param.extension = usersession.AgentNumber.Trim();
                //if (usersession.contracts.Equals(Constant.GIGAMED))
                //{
                //Guid click2callid = Guid.NewGuid();
                //string[] str = new string[3];
                //str[0] = param.extension;
                //str[1] = click2callid.ToString();
                //str[2] = param.phone_number;
                //string _url = string.Format("https://gigamed.voip247.vn/clicktocallgigamed/doAction.php?action=autocall&exten={0}&stringcode=as7dkf8ie2343it26fg32kasdfd3f123fdkjkek&click2callid={1}&number={2}", str);
                ////result = campaignService.Click2CallGigamed(param);
                //System.Diagnostics.Process.Start(_url);
                //result = campaignService.Click2CallGigamed(param);
                //}
                //else
                //{
                //    result = campaignService.Click2Call(param);
                //}           
                result = campaignService.Click2Call(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Call
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Call(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            LoginAsteriskDto loginAsterisk = new LoginAsteriskDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (Session[Constant.USER_SESSION] != null)
            {
                loginAsterisk.UserExt = usersession.UserName.Substring(0, 3);
                loginAsterisk.PhoneNumber = param.PhoneNumber;
                loginAsterisk.contracts = usersession.contracts;
                result = campaignService.LoginAsterisk(loginAsterisk);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCallByAgent
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAgentByUserName(hrEmployeeDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                param.ID = FGuid.ToGuid(usersession.UserId);
                param.WindowsUser = usersession.UserName;
                param.EmployeeID = usersession.EmployeeID;
                param.contracts = usersession.contracts;
                param.AgentNumber = usersession.AgentNumber;
                param.contracts = usersession.contracts;
                result = campaignService.GetAgentByUserName(param);                
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// DeleteSurvey
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteSurvey(OBSurveyParam param)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                param.UserId = usersession.UserId;
                param.WindowsUser = usersession.UserName;
                param.contracts = usersession.contracts;
                result = campaignService.DeleteSurvey(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get Campaign Focus By Customer
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCampaignFocusByCustomer(OBSurveyParam para)
        {
            List<ServiceTypeDto> result = new List<ServiceTypeDto>();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                para.AgentID = usersession.UserId;
                para.UserId = usersession.UserId;
                para.WindowsUser = usersession.UserName;
                para.GroupCode = usersession.GroupCode;
                para.contracts = usersession.contracts;
                para.SalesmanCode = usersession.EmployeeID;
                para.UserExt = usersession.AgentNumber;
                result = campaignService.GetCampaignFocusByCustomer(para);
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Continue
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult Continue(OBSurveyParam para)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                para.AgentID = usersession.UserId;
                para.UserId = usersession.UserId;
                para.WindowsUser = usersession.UserName;
                para.GroupCode = usersession.GroupCode;
                para.contracts = usersession.contracts;
                para.SalesmanCode = usersession.EmployeeID;
                para.UserExt = usersession.AgentNumber;
                result = campaignService.GetQuestionAnswerBySurvey(para);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// GetSurveyById
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSurveyById(OBSurveyParam para)
        {
            LoadPhoneNumberResult result = new LoadPhoneNumberResult();            
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                para.AgentID = usersession.UserId;
                para.UserId = usersession.UserId;
                para.WindowsUser = usersession.UserName;
                para.GroupCode = usersession.GroupCode;
                para.SalesmanCode = usersession.EmployeeID;
                para.contracts = usersession.contracts;
                para.UserExt = usersession.AgentNumber;
                ResultMessageDto checklimit = new ResultMessageDto();
                LimitedCustomerDto lmc = new LimitedCustomerDto()
                {
                    extension = usersession.AgentNumber,
                    call_date = DateTime.Now.ToString("yyyy-MM-dd"),
                    cust_code = para.PhoneNumber
                };
                checklimit = callPlanService.CheckLimitedCustomer(lmc);
                if (checklimit.code.Equals(Constant.SUCCESS))
                {
                    result = campaignService.GetSurveyById(para);
                }
                else
                {
                    result.code = checklimit.code;
                    result.message = checklimit.message;
                }
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckLimitedCustomer(OBSurveyParam para)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                
                LimitedCustomerDto lmc = new LimitedCustomerDto()
                {
                    extension = usersession.AgentNumber,
                    call_date = DateTime.Now.ToString("yyyy-MM-dd"),
                    cust_code = para.PhoneNumber
                };
                result = callPlanService.CheckLimitedCustomer(lmc);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Effection Call
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EffectionCall(OBSurveyParam para)
        {
            EffectionCallDto result = new EffectionCallDto();
            result = effectionCallService.EffectionCall(para);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get Doctor Info
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDoctorInfo(OBSurveyParam para)
        {
            DoctorInformationDto doc = campaignService.GetDoctorInfo(para);            
            var jsonResult = Json(doc, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Save Effection Call
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveEffectionCall(EffectionCallDto para)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = effectionCallService.SaveEffectionCall(para);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveStatus(AssignDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                param.AgentID = usersession.UserId;
                param.contracts = usersession.contracts;
                result = assignmentService.SaveStatus(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Survey By Question
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveSurveyByQuestion(DataSurvey param, OBSurveyParam outbound)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null && param != null)
            {
                if (param.DataSurveyCampaign.Count() > 0)
                {
                    param.DataSurveyCampaign.ElementAt(0).UserName = usersession.UserName;
                }
                outbound.UserId = usersession.UserId;
                outbound.AgentID = usersession.UserId;
                outbound.WindowsUser = usersession.WindowsUser;
                outbound.ProjectID = usersession.GroupID;
                outbound.GroupCode = usersession.GroupCode;
                outbound.SalesmanCode = usersession.EmployeeID;
                outbound.UserExt = usersession.AgentNumber;
                outbound.contracts = usersession.contracts;
                result = campaignService.SaveSurveyByQuestion(param, outbound);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SearchSurveyByAgent
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchSurveyByAgent(SearchCallParam json)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                json.AgentNumber = usersession.AgentNumber;
                json.contracts = usersession.contracts;
                json.EmployeeID = usersession.EmployeeID;
                result = campaignService.SearchSurveyByAgent(json);
                var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get service type by id
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetServiceTypeById(ServiceTypeDto json)
        {
            ServiceTypeDto result = new ServiceTypeDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = serviceTypeService.GetServiceTypeById(json);
                var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDoctorCallplan
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDoctorCallplan(OBSurveyParam param)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                param.UserExt = usersession.AgentNumber;
                param.WindowsUser = usersession.WindowsUser;
            }
            IEnumerable<DoctorInformationDto> result = campaignService.GetDoctorCallplan(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddContactList(iMindContactInfoDto json) 
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                json.update_by = usersession.WindowsUser;
                result = campaignService.AddContactList(json);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSpecialNote(iMindContactInfoDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                json.update_by = usersession.WindowsUser;
                result = campaignService.SaveSpecialNote(json);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
