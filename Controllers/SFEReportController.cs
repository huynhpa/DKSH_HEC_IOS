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
    public class SFEReportController : Controller
    {
        private SFEReportService sfeReportService = null;

        public SFEReportController() : this(new SFEReportServiceImpl()) { }

        public SFEReportController(SFEReportService sfeReportService)
        {
            this.sfeReportService = sfeReportService;
        }

        /// <summary>
        /// initialize
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSFEReportConst(SFEReportConstDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = sfeReportService.SaveSFEReportConst(json);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SFE Orgchart
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SFEOrgchart(SFEOrgchartDto dto)
        {
            IEnumerable<SFEOrgchartDto> result = sfeReportService.SFEOrgchart(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpGet]
        public JsonResult SFEPerformanceReport(SFEOrgchartDto dto)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.user_id = usersession.UserName;
            }
            IEnumerable<SFEReportDto> result = sfeReportService.SFEPerformanceReport(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ExportSFEPerformance(SFEOrgchartDto dto)
        {
            try
            {
                IWorkbook workbook = sfeReportService.ExportSFEPerformance(dto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "SFEPerformance"));
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
        /// SaveTargetCallTeleSale
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveTargetCallTeleSale(HttpPostedFileBase aFile, TargetCallTeleSaleDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
                if (usersession != null)
                {
                    json.update_by = usersession.UserName;
                }
                result = sfeReportService.SaveTargetCallTeleSale(aFile, json);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
