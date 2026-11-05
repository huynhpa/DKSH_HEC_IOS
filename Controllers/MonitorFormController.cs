using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using DKSHApplication.Common;
using System.IO;
using NPOI.SS.UserModel;

namespace DKSHApplication.Controllers
{
    public class MonitorFormController : Controller
    {
        //
        // GET: /MonitorForm/
        private MonitorService monitorService;

        public MonitorFormController() : this (new MonitorServiceImpl()) { }

        public MonitorFormController(MonitorService monitorService)
        {
            this.monitorService = monitorService;
        }

        /// <summary>
        /// initial page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
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
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchMonitorHistory(MonitorHistoryDto dto)
        {
            IEnumerable<MonitorHistoryDto> result = monitorService.SearchMonitorHistory(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get list agents by leader
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAgentByLeader(MonitorHistoryDto dto)
        {
            IEnumerable<AgentByLeaderDto> result = monitorService.GetAgentByLeader(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetLeaderList(MonitorHistoryDto dto)
        {
            IEnumerable<AgentByLeaderDto> result = monitorService.GetLeaderList();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get Record Info
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRecordInfoToMonitor(MonitorHistoryDto dto)
        {            
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.MonitorBy = usersession.AgentNumber;
                dto.contracts = usersession.contracts;
            }
            MonitorHistoryDto result = monitorService.GetRecordInfoToMonitor(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMonitorDetail(MonitorHistoryDto dto)
        {
            MonitorHistoryDto result = monitorService.GetMonitorDetail(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// CriterialEvaluationCall
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CriterialEvaluationCall()
        {
            IEnumerable<CriterialEvaluationCallDto> result = monitorService.CriterialEvaluationCall();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// EvaluationMark
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EvaluationMark()
        {
            IEnumerable<EvaluationMarkDto> result = monitorService.EvaluationMark();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// save evaluation call
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveEvaluationCall(EvaluationDataParam param, MonitorHistoryDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.CreateBy = usersession.UserName;
                dto.UpdateBy = usersession.UserName;
                dto.MonitorBy = usersession.Extension;
                result = monitorService.SaveEvaluationCall(param, dto);
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
        /// <param name="dto"></param>
        [HttpGet]
        public void MonitorHistoryExport(MonitorHistoryDto dto)
        {
            try
            {
                IWorkbook workbook = monitorService.MonitorHistoryExport(dto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Report_Call"));
                    Response.BinaryWrite(exportData.ToArray());
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
