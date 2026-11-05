using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.CampaignManagement
{
    public class CallPlanController : Controller
    {
        private CallPlanService callPlanService;
        private CampaignService campaignService;
        public CallPlanController() : this (new CallPlanServiceImpl(), new CampaignServiceImpl())
        {

        }
        public CallPlanController(CallPlanService callPlanService, CampaignService campaignService)
        {
            this.callPlanService = callPlanService;
            this.campaignService = campaignService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        /// <summary>
        /// Search CallPlan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchCallPlan(CallPlanDto dto)
        {
            IEnumerable<CallPlanDto> rs = callPlanService.SearchCallPlan(dto);
            var js = Json(rs, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        [HttpGet]
        public JsonResult SearchLimitedCustomer(LimitedCustomerDto dto)
        {
            IEnumerable<LimitedCustomerDto> rs = callPlanService.SearchLimitedCustomer(dto);
            var js = Json(rs, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// Upload Call Plan
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadCallPlan(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                result = callPlanService.UploadCallPlan(aFile);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CallPlan Template
        /// </summary>
        [HttpGet]
        public void CallPlanTemplate()
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + "CallPlanTemplate.xlsx";
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.TransmitFile(fileName);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// import campaign by customer
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportCampaignByCustomer(HttpPostedFileBase aFile, CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                string WindowUser = userSession.WindowsUser;
                result = callPlanService.ImportCampaignByCustomer(aFile, WindowUser, dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImportDailyInTimeSummary(HttpPostedFileBase aFile, CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                string WindowUser = userSession.WindowsUser;
                result = callPlanService.ImportDailyInTimeSummary(aFile, WindowUser, dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImportLimitedCustomer(HttpPostedFileBase aFile, LimitedCustomerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                result = callPlanService.ImportLimitedCustomer(aFile, dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RouteToPhoneNumber(CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                string WindowUser = userSession.WindowsUser;
                result = callPlanService.RouteToPhoneNumber(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcessCallPlan(CallPlanByCampaignDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                string WindowUser = userSession.WindowsUser;
                result = callPlanService.ProcessCallPlanByCampaign(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get call plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetCallPlan(CallPlanDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                string WindowUser = userSession.WindowsUser;
                result = callPlanService.GetCallPlan(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Campaign By Customer Template
        /// </summary>
        [HttpGet]
        public void CampaignByCustomerTemplate()
        {
            try
            {
                string fileNm = "CallPlanByCampaign.xlsx";
                CampaignDto dto = new CampaignDto();
                dto.Status = "1";
                IWorkbook workbook = callPlanService.CampaignByCustomerTemplate(dto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileNm));
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
        /// ExportCampaignFocus
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ExportCampaignFocus(CallPlanDto dto)
        {
            try
            {
                string fileNm = "ExportCampaignFocus.xlsx";
                IWorkbook workbook = callPlanService.ExportCampaignFocus(dto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileNm));
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AllActiveCampaign()
        {
            CampaignDto dto = new CampaignDto();
            dto.Status = "1";
            IEnumerable<ServiceTypeDto> result = callPlanService.AllCampaignActive(dto);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Campaign By Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchCallPlanByCampaign(CallPlanByCampaignDto dto)
        {
            IEnumerable<CallPlanByCampaignDto> result = callPlanService.SearchCallPlanByCampaign(dto);
            var js = Json(result, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchDailyInTimeSummary(CallPlanDto dto)
        {
            IEnumerable<DailyInTimeSummaryDto> result = callPlanService.SearchDailyInTimeSummary(dto);
            var js = Json(result, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }
    }
}
