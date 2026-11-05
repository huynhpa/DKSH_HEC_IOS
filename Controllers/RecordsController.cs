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
    public class RecordsController : Controller
    {
        private RecordsService recordService;
        private MonitorService monitorService;
        private AgentService agentService;

        /// <summary>
        /// 
        /// </summary>
        public RecordsController() : this(new RecordsServiceImpl(), new MonitorServiceImpl(), new AgentServiceImpl())
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callingTimeService"></param>
        public RecordsController(RecordsService recordService, MonitorService monitorService, AgentService agentService)
        {
            this.recordService = recordService;
            this.monitorService = monitorService;
            this.agentService = agentService;
        }
        // INITPAGE: /Records/
        public ActionResult Index()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                AgentGroupDto ag = new AgentGroupDto()
                {
                    IsDelete = false
                };
                ViewBag.AGENT_GROUP = agentService.AgentGroup(ag);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveRecord(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                result = recordService.SaveRecord(aFile);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search Record
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchRecord(Records46Dto param)
        {
            IEnumerable<Records46Dto> result = recordService.SearchRecord(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult SearchTeleOrgchart(TeleSaleOrgChartDto param)
        {
            IEnumerable<TeleSaleOrgChartDto> result = recordService.SearchTeleOrgchart(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpGet]
        public JsonResult GetTeleOrgChartDetail(TeleSaleOrgChartDto param)
        {
            TeleSaleOrgChartDto result = recordService.GetTeleOrgChartDetail(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }


        [HttpPost]
        public JsonResult SaveTeleSaleOrgChart(TeleSaleOrgChartDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = recordService.SaveTeleSaleOrgChart(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteTeleSaleOrgChart(TeleSaleOrgChartDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = recordService.DeleteTeleSaleOrgChart(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Record Export
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void RecordExport(Records46Dto dto)
        {
            try
            {
                string fileNm = "RecordsReport.xlsx";
                IWorkbook workbook = recordService.RecordExport(dto);
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
        /// Talk Time
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void TalkTime(Records46Dto dto)
        {
            try
            {
                string fileNm = "TalkTime.xlsx";
                IWorkbook workbook = recordService.TalkTime(dto);
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
        /// Fetch Record Issabel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FetchRecordIssabel(Records46Dto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = recordService.FetchRecordIssabel(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletet Monitor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteMonitor(MonitorHistoryDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = recordService.DeleteMonitor(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
