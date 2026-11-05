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

namespace DKSHApplication.Controllers
{
    public class ScheduleTimerController : Controller
    {
        private ScheduleTimerService scheduleTimerService = null;

        public ScheduleTimerController() : this(new ScheduleTimerServiceImpl()) { }

        public ScheduleTimerController(ScheduleTimerService scheduleTimerService)
        {
            this.scheduleTimerService = scheduleTimerService;
        }
        // GET: /ScheduleTimer/
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
        /// Search Agents
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public JsonResult SearchAgents(ShiftAgentsDto dto)
        {
            IEnumerable<ShiftAgentsDto> result = scheduleTimerService.SearchAgents(dto);
            var jsonRS = Json(result, JsonRequestBehavior.AllowGet);
            jsonRS.MaxJsonLength = int.MaxValue;
            return jsonRS;
        }

        /// <summary>
        /// Export Schedule
        /// </summary>
        /// <param name="scheduleTimerDto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ExportSchedule(ScheduleTimerDto scheduleTimerDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IWorkbook workbook = scheduleTimerService.ExportSchedule(scheduleTimerDto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ScheduleTimer"));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Generate Random Number
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GenerateRandomNumber()
        {
            ResultMessageDto result = new ResultMessageDto();
            result.message = Utils.GenerateRandomNo().ToString();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// New Agents
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NewAgents(ShiftAgentsDto agent)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto user = (UserDto)Session[Constant.USER_SESSION];
            if (user != null)
            {
                agent.Updateby = user.UserId;
                agent.CreateBy = user.UserId;
                result = scheduleTimerService.NewAgents(agent);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Agents
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateAgents(ShiftAgentsDto agent)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto user = (UserDto)Session[Constant.USER_SESSION];
            if (user != null)
            {
                agent.Updateby = user.UserId; 
                result = scheduleTimerService.UpdateAgents(agent);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Agent Info
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAgentInfo(ShiftAgentsDto agent)
        {
            ShiftAgentsDto result = scheduleTimerService.GetAgentInfo(agent);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check Agent Existed
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckAgentExisted(ShiftAgentsDto dto)
        {
            ShiftAgentsDto result = scheduleTimerService.CheckAgentExisted(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        

    }
}
