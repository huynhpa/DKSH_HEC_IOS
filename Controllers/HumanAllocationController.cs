using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class HumanAllocationController : Controller
    {
        private HumanAllocationService humanAllocationService;
        public HumanAllocationController() : this(new HumanAllocationServiceImpl()) {}

        public HumanAllocationController(HumanAllocationService humanAllocationService)
        {
            this.humanAllocationService = humanAllocationService;
        }
        //
        // GET: /HumanAllocation/
        public ActionResult Index()
        {
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCampaignList()
        {
            IEnumerable<HumanAllocationDto> campaignList = humanAllocationService.GetCampaignList();
            return Json(campaignList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAgentList()
        {
            IEnumerable<AgentsDto> agentList = humanAllocationService.GetAgentList();
            return Json(agentList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LogIn And LogOut
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveLoginOrLogout(HumanAllocationDto dto)
        {
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (userSession != null)
            {
                dto.CreateBy = userSession.UserId;
                dto.UpdateBy = userSession.UserId;
                if (dto.IsLogin == true)
                {
                    result = humanAllocationService.LogInCallTime(dto);
                }
                else
                {
                    result = humanAllocationService.LogOutCallTime(dto);
                }
                
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Log Out Call Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LogOutCallTime(HumanAllocationDto dto)
        {
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (userSession != null)
            {
                dto.CreateBy = userSession.UserId;
                dto.UpdateBy = userSession.UserId;
                result = humanAllocationService.LogOutCallTime(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Login Or Logout By Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetLoginOrLogoutByAgent(HumanAllocationDto dto)
        {
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (userSession != null)
            {
                dto.CreateBy = userSession.UserId;
                dto.UpdateBy = userSession.UserId;
                result.HumanAllocateDto = humanAllocationService.GetLoginOrLogoutByAgent(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search Human Allocation
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchHumanAllocation(HumanAllocationDto dto)
        {
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (userSession != null)
            {
                dto.CreateBy = userSession.UserId;
                dto.UpdateBy = userSession.UserId;
                result.HumanAllocationList = humanAllocationService.SearchHumanAllocation(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAgentJob()
        {
            IEnumerable<AgentJobsDto> agentJobs = humanAllocationService.GetAgentJob();
            return Json(agentJobs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAgentJobDetail(LogAgentJobDto param)
        {
            LogAgentJobDto agentJobs = humanAllocationService.GetAgentJobDetail(param);
            return Json(agentJobs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchLogAgentJob(LogAgentJobDto param)
        {
            IEnumerable<LogAgentJobDto> result = humanAllocationService.SearchLogAgentJob(param);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert Log Agent Job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult LogInAndLogOutAgentJob(LogAgentJobDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                param.CreateBy = userSession.UserId;
                param.UpdateBy = userSession.UserId;
                result = humanAllocationService.InsertLogAgentJob(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Log Agent Job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateLogAgentJob(LogAgentJobDto param)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                param.UpdateBy = userSession.UserId;
                result = humanAllocationService.UpdateLogAgentJob(param);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.code = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Human Allocation Report
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void HumanAllocationReport(HumanAllocationDto dto)
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + Constant.MARKETING_TEMPLATE;
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
    }
}
