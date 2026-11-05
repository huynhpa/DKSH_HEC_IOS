using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class QuickReportController : Controller
    {
        private QuickReportService quickReportService = null;
        private CCCRequestService cccRequestService = null;
        private RaiseIssueService raiseIssueService = null;

        public QuickReportController() : this (new QuickReportServiceImpl(), 
            new CCCRequestServiceImpl(), new RaiseIssueServiceImpl())
        { }
        public QuickReportController(QuickReportService quickReportService, 
                                    CCCRequestService cccRequestService, RaiseIssueService raiseIssueService)
        {
            this.quickReportService = quickReportService;
            this.cccRequestService = cccRequestService;
            this.raiseIssueService = raiseIssueService;
        }

        // GET: /QuickReport/
        public ActionResult Index()
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                IEnumerable<TicketNoDto> ticketList = cccRequestService.GetTicketNoList();
                ViewBag.TicketNoList = ticketList;
                IEnumerable<RaiseIssueDto> IssueNoList = raiseIssueService.GetIssueNoList();
                ViewBag.IssueNoList = IssueNoList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search Quick Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult SearchQuickReport(QuickReportDto json)
        {
            IEnumerable<QuickReportDto> result = quickReportService.SearchQuickReport(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get Quick Report Detail
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetQuickReportDetail(QuickReportDto json)
        {
            QuickReportDto jsonResult = quickReportService.GetQuickReportDetail(json);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertQuickReport(QuickReportDto json)
        {
            QuickReportResult jsonResult = new QuickReportResult();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                json.CreateBy = userSession.UserId;
                json.UpdateBy = userSession.UserId;
                jsonResult = quickReportService.InsertQuickReport(json);
            }            
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateQuickReport(QuickReportDto json)
        {
            QuickReportResult jsonResult = new QuickReportResult();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                json.UpdateBy = userSession.UserId;
                jsonResult = quickReportService.UpdateQuickReport(json);
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Quick Report
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteQuickReport(QuickReportDto json)
        {
            QuickReportResult jsonResult = quickReportService.DeleteQuickReport(json);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AddQuickReport
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AddQuickReport()
        {
            IEnumerable<TicketNoDto> ticketList = cccRequestService.GetTicketNoList();
            return Json(ticketList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get IssueNo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetIssueNo(TicketNoDto obj)
        {
            IEnumerable<RaiseIssueDto> lst = raiseIssueService.GetIssueNoByTicket(obj);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

    }
}
