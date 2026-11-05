using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class CustomerSurveyController : Controller
    {
        //
        // GET: /TransferNotificationProvince/
        private CustomerSurveyService customerSurveyService = null;
        private UpSaleCrossSaleService USCSService = null;

        public CustomerSurveyController() : this(new CustomerSurveyServiceImpl(), new UpSaleCrossSaleServiceImpl()) { }

        public CustomerSurveyController(CustomerSurveyService customerSurveyService, UpSaleCrossSaleService USCSService)
        {
            this.customerSurveyService = customerSurveyService;
            this.USCSService = USCSService;
        }
        //
        // GET: /CustomerSurvey/

        public ActionResult Index()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Question Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Question()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("Question");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        
        /// <summary>
        /// Survey list page
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyList()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("SurveyList");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        /// <summary>
        /// Get Customer By CustCode
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCustomerByCustCode(CustomerSurveyInfoDto json)
        {
            ResultMessageDto obj = customerSurveyService.GetCustomerByCustCode(json);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Questionnaire
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetQuestionnaire(CustomerSurveyInfoDto json)
        {
            QuestionnaireDto questionnaire = customerSurveyService.GetQuestionnaire(json);
            return Json(questionnaire, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Customer Survey Info
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveCustomerSurveyInfo(CustomerSurveyInfoDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userDto = (UserDto)Session[Constant.USER_SESSION];
            if (userDto != null)
            {
                json.UpdateBy = userDto.UserId;
                json.CreateBy = userDto.UserId;
                result = customerSurveyService.SaveCustomerSurveyInfo(json);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        

        /// <summary>
        /// Get Question Title
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetQuestionTitle()
        {
            IEnumerable<QuestionnaireTitleDto> result = customerSurveyService.GetQuestionTitle();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Question Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetQuestionDetail(CustomerSurveyInfoDto dto)
        {
            CustomerSurveyInfoDto obj = customerSurveyService.GetQuestionDetail(dto);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// New Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NewQuestion(CustomerSurveyInfoDto dto)
        {
            ResultMessageDto obj = customerSurveyService.NewQuestion(dto);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edit Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditQuestion(CustomerSurveyInfoDto dto)
        {
            ResultMessageDto obj = customerSurveyService.EditQuestion(dto);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteQuestion(CustomerSurveyInfoDto dto)
        {
            ResultMessageDto obj = customerSurveyService.DeleteQuestion(dto);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Customer Survey Report
        /// </summary>
        [HttpGet]
        public void CustomerSurveyReport()
        {
            try
            {
                IWorkbook workbook = customerSurveyService.CustomerSurveyReport();
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "CustomerSurveyReport"));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Survey History
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSurveyHistory(CustomerSurveyInfoDto json)
        {
            IEnumerable<CustomerSurveyInfoDto> obj = customerSurveyService.GetSurveyHistory(json);
            var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserSurveys()
        {
            IEnumerable<SalemanPOTextDto> obj = USCSService.GetSalemanPO();
            var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get iMind Agents
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetiMindAgents()
        {
            IEnumerable<iMindUserDto> obj = customerSurveyService.GetiMindAgents();
            var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}
