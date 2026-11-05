using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.Dashboard
{
    public class DashboardCDIController : Controller
    {
        private CDIDashboardService cdiDashboardService = null;

        private CDIService cdiService = null;

        public DashboardCDIController() : this (new CDIDashboardServiceImpl(), new CDIServiceImpl()) { }
        public DashboardCDIController(CDIDashboardService cdiDashboardService, CDIService cdiService)
        {
            this.cdiDashboardService = cdiDashboardService;
            this.cdiService = cdiService;
        }
        // Init page
        // GET: /DashboardCDI/
        public ActionResult CDISummary()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                cdiService.ScheduleTimer(16); // AUTO RUN BACK UP DATA FOR DAILY REPORT
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// ShipTo Summary
        /// </summary>
        /// <returns></returns>
        public ActionResult ShipToSummary()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("ShipToSummary");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        /// <summary>
        /// View summary information CDI
        /// </summary>
        /// <returns></returns>
        public JsonResult Dashboard()
        {
            CdiDashboardDto view = new CdiDashboardDto();
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get CDI Dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCDIDashboard(CDIDashboardParam dto)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            CdiDashboardDto cdiDashboard = null;
            if (userSession != null)
            {
                dto.UserId = userSession.UserId;
                cdiDashboard = cdiDashboardService.GetCDIDashboard(dto);
            }
            
            return Json(cdiDashboard, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public void Export(CdiDashboardDto dto)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.ExportLicensesSummary(dto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", dto.LicenseType));
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
        /// Send Mail To Supervisor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SendMailToSuperVisor()
        {
            ResultMessageDto rs = new ResultMessageDto();
            //rs = cdiDashboardService.SendMailToSuperVisor();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SummaryChannelHistory(CDIDashboardParam dto)
        {
            IEnumerable <SummaryChannelHistoryDto> rs = cdiDashboardService.SummaryChannelHistory(dto);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// View Detail Channel
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ViewDetailChannel(CdiDashboardDto dto)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.ViewDetailChannel(dto);
                string fileName = dto.ToMonth + "_" + dto.CurrentChannel;
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
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
        /// View Detail Channel
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ViewTotalEveryMonth(CdiDashboardDto dto)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.ViewTotalEveryMonth(dto);
                string fileName = dto.ToMonth;
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
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
        /// Compare Any Month
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void CompareAnyMonth(CdiDashboardDto dto)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.CompareAnyMonth(dto);
                string fileName = dto.FromMonth + "_" + dto.ToMonth;
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
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
