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
    public class CCCRequestController : Controller
    {
        private CCCRequestService cccRequestService = null;
        private FileStoreService fileStoreService = null;

        public CCCRequestController() : this(new CCCRequestServiceImpl(), new FileStoreServiceImpl()) { }
        public CCCRequestController(CCCRequestService cccRequestService, FileStoreService fileStoreService)
        {
            this.cccRequestService = cccRequestService;
            this.fileStoreService = fileStoreService;
        }
        /// <summary>
        /// INIT PAGE
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
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
        /// Search Request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchRequest(CCCRequestParam param)
        {
            IEnumerable<CCCRequestDto> listRequest = cccRequestService.SearchRequest(param);
            var jsonResult = Json(listRequest, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestDetail(CCCRequestParam param)
        {
            CCCRequestDto requestDto = cccRequestService.GetRequestDetail(param);
            return Json(requestDto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult InsertRequest(CCCRequestDto param)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto jsonResult = new ResultMessageDto();
            if (userSession != null)
            {
                param.CreateBy = userSession.UserId;
                param.UpdateBy = userSession.UserId;
                param.Team = userSession.Team;
                param.Email = userSession.Email;
                param.IsManager = userSession.IsManager;
                jsonResult = cccRequestService.InsertRequest(param);
            }
            else
            {
                jsonResult.code = Constant.SESSION_EXPIRED_CODE;
                jsonResult.message = Constant.SESSION_EXPIRED_MESSAGE;
            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>        
        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult UpdateRequest(CCCRequestDto param)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto jsonResult = new ResultMessageDto();
            if (userSession != null)
            {
                param.UpdateBy = userSession.UserId;
                jsonResult = cccRequestService.UpdateRequest(param);
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get ticket no
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTicketNo(CCCRequestParam jsonParam)
        {
            CCCRequestDto requestDto = new CCCRequestDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                jsonParam.UserId = userSession.UserId;
                jsonParam.Team = userSession.Team;
                requestDto = cccRequestService.GetTicketNo(jsonParam);
            }
            
            return Json(requestDto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get File Store By Request
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFileByTicket(FileStoreDto jsonParam)
        {
            IEnumerable<FileStoreDto> fileStoreList = fileStoreService.GetFileByTicket(jsonParam);
            return Json(fileStoreList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete request
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteRequest(CCCRequestDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = cccRequestService.DeleteRequest(json);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Approval request by manager
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Approval(CCCRequestDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                json.Email = userSession.Email;
                result = cccRequestService.Approval(json);
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
