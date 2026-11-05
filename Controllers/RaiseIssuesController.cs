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
    public class RaiseIssuesController : Controller
    {
        private RaiseIssueService issueService = null;
        private CCCRequestService cccRequestService = null;
        private FileStoreService fileStoreService = null;
        public RaiseIssuesController() : this(new RaiseIssueServiceImpl(), new CCCRequestServiceImpl(), new FileStoreServiceImpl()) { }

        public RaiseIssuesController(RaiseIssueService issueService, CCCRequestService cccRequestService, FileStoreService fileStoreService)
        {
            this.issueService = issueService;
            this.cccRequestService = cccRequestService;
            this.fileStoreService = fileStoreService;
        }
        /// <summary>
        /// INIT PAGE
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportIssue()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                IEnumerable<TicketNoDto> ticketList = cccRequestService.GetTicketNoList();
                ViewBag.TicketNoList = ticketList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search Issue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchIssue(RaiseIssueParam jsonParam)
        {
            IEnumerable<RaiseIssueDto> issueLst = issueService.SearchIssue(jsonParam);
            var jsonRs = Json(issueLst, JsonRequestBehavior.AllowGet);
            jsonRs.MaxJsonLength = int.MaxValue;
            return jsonRs;
        }

        /// <summary>
        /// Get Issue Detail
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetIssueDetail(RaiseIssueParam jsonParam)
        {
            RaiseIssueDto obj = issueService.GetIssueDetail(jsonParam);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto jsonRsMsg = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                jsonParam.CreateBy = userSession.UserId;
                jsonParam.UpdateBy = userSession.UserId;
                jsonRsMsg = issueService.InsertIssue(jsonParam);
            }
            else
            {
                jsonRsMsg.code = Constant.SESSION_EXPIRED_CODE;
                jsonRsMsg.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(jsonRsMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Issue Detail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateIssue(RaiseIssueParam param)
        {
            ResultMessageDto jsonRsMsg = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                param.UpdateBy = userSession.UserId;
                jsonRsMsg = issueService.UpdateIssue(param);
            }
            else
            {
                jsonRsMsg.code = Constant.SESSION_EXPIRED_CODE;
                jsonRsMsg.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(jsonRsMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Report bug 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ReportBug()
        {
            ReportBugDto dto = new ReportBugDto();
            IEnumerable<TicketNoDto> ticketList = cccRequestService.GetTicketNoList();
            int IssueNo = Utils.GenerateRandomNo();
            dto.ticketList = ticketList;
            dto.IssueNo = IssueNo.ToString();
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get File Store By Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFileByIssue(FileStoreDto jsonParam)
        {
            IEnumerable<FileStoreDto> fileStoreList = fileStoreService.GetFileByIssue(jsonParam);
            return Json(fileStoreList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteIssue(RaiseIssueParam jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                jsonParam.UpdateBy = userSession.UserId;
                result = issueService.DeleteIssue(jsonParam);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
