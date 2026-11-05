using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class CDIManagementController : Controller
    {
        private CDIService cdiService = null;

        private UserService userService = null;

        private CDIDashboardService cdiDashboardService = null;

        public CDIManagementController() : this (new CDIServiceImpl(), new UserServiceImpl(), new CDIDashboardServiceImpl()) { }
        public CDIManagementController(CDIService cdiService, UserService userService, CDIDashboardService cdiDashboardService)
        {
            this.cdiService = cdiService;
            this.userService = userService;
            this.cdiDashboardService = cdiDashboardService;
        }

        // Init page
        // GET: /CDIManagement/
        public ActionResult Index()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                CDIDto dto = new CDIDto();
                //ResultMessageDto result = cdiService.ProcessCustomer(dto);
                IEnumerable<UserDto> userTeamCDI = userService.GetUserByTeamCDI();
                IEnumerable<CustGroupCode> custGroupCode = cdiService.GetCustGroup();
                ViewBag.UserTeamCDI = userTeamCDI;
                ViewBag.CustGroup = custGroupCode;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// ShipTo
        /// </summary>
        /// <returns></returns>
        public ActionResult ShipTo()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                IEnumerable<UserDto> userTeamCDI = userService.GetUserByTeamCDI();
                IEnumerable<CustGroupCode> custGroupCode = cdiService.GetCustGroup();
                ViewBag.UserTeamCDI = userTeamCDI;
                ViewBag.CustGroup = custGroupCode;
                return View("ShipTo");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search ShipTo
        /// </summary>
        /// <param name="shipto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchShipTo(CustomerShipToDto shipto)
        {
            IEnumerable<CustomerShipToDto> result = cdiService.SearchShipTo(shipto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        /// <summary>
        /// Report
        /// </summary>
        /// <returns></returns>
        public ActionResult Report()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                CDIDto dto = new CDIDto();
                //ResultMessageDto result = cdiService.ProcessCustomer(dto);
                IEnumerable<UserDto> userTeamCDI = userService.GetUserByTeamCDI();
                IEnumerable<CustGroupCode> custGroupCode = cdiService.GetCustGroup();
                ViewBag.UserTeamCDI = userTeamCDI;
                ViewBag.CustGroup = custGroupCode;
                return View("Report");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        /// <summary>
        /// Search CDI by condition
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllCdi(CDIDto dto)
        {
            IEnumerable<CDIDto> cdiMngList = cdiService.GetAllCDI(dto);
            var jsonResult = Json(cdiMngList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get detail information
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getCdiDetail(CDIDto dto)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            CDIDto rs = cdiService.GetCDIDetail(dto);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Add new CDI
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertCdi(CDIDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UserId = userSession.UserId;
                dto.CreateBy = userSession.UserId;
                rs = cdiService.InsertCdi(dto);
            }
            else
            {
                rs.code = Constant.SESSION_EXPIRED_CODE;
                rs.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Update CDI
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateCdi(CDIDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UserId = userSession.UserId;
                rs = cdiService.UpdateCdi(dto);
            }
            else
            {
                rs.code = Constant.SESSION_EXPIRED_CODE;
                rs.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(CDIDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UserId = userSession.UserId;
                rs = cdiService.UnusedCDI(dto);
            }
            else
            {
                rs.code = Constant.SESSION_EXPIRED_CODE;
                rs.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// GetUserByTeamCDI
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserByTeamCDI()
        {
            IEnumerable<UserDto> userTeamCDI = userService.GetUserByTeamCDI();
            return Json(userTeamCDI, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export Result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public void ExportResult(CDIDto dto)
        {
            dto.NotUse = Constant.NOT_USE;
            IEnumerable<CDIDto> cdiMngList = cdiService.GetAllCDI(dto);         
            try
            {
                IWorkbook workbook = cdiService.ExportExcelBySearch(cdiMngList, Constant.CERTIFICATION, Constant.CERTIFICATION_TEMPLATE);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "CDI_Report"));
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
        /// Process Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcessCustomer(CDIDto dto)
        {
            ResultMessageDto result = cdiService.ProcessCustomer(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Certification History
        /// </summary>
        /// <param name="jsonPr"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CertificationHistory(CDIDto jsonPr)
        {
            IEnumerable<CertificationHistoryDto> result = cdiService.CertificationHistory(jsonPr);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Check Cust Code Exist
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckCustCodeExist(CDIDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = cdiService.CheckCustCodeExist(json);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                CDIDto dto = new CDIDto();
                //ResultMessageDto result = cdiService.ProcessCustomer(dto);
                IEnumerable<UserDto> userTeamCDI = userService.GetUserByTeamCDI();
                IEnumerable<CustGroupCode> custGroupCode = cdiService.GetCustGroup();
                ViewBag.UserTeamCDI = userTeamCDI;
                ViewBag.CustGroup = custGroupCode;
                return View("New");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Daily Report
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public void DailyReport(CDIDto js)
        {
            try
            {
                js.SheetName = Constant.DAILY_REPORT;
                js.FileTemplate = Constant.DAILY_REPORT_TEMPLATE;
                IWorkbook workbook = cdiService.DailyReport(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "DailyReport"));
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
        /// Export G950
        /// </summary>
        /// <param name="js"></param>
        public void ExportG950(CDIDto js)
        {
            try
            {
                js.SheetName = Constant.REPORT_G950;
                js.FileTemplate = Constant.REPORT_G950_TEMPLATE;
                IWorkbook workbook = cdiService.ExportG950(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "G950"));
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
        /// Export Product By File
        /// </summary>
        /// <param name="js"></param>
        public void ExportProductByFile(CDIDto js)
        {
            try
            {
                string zipFile = cdiService.ExportProductByFile(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = Constant.ZIP_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", zipFile));
                    Response.TransmitFile(zipFile);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Required Licences
        /// </summary>
        /// <returns></returns>
        public JsonResult RequiredLicences(CDIDto js)
        {
            IEnumerable<CDIDto> result = cdiService.RequiredLicences(js);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Export Required Licences
        /// </summary>
        /// <param name="js"></param>
        public void ExportRequiredLicenses(CDIDto js)
        {
            try
            {
                IWorkbook workbook = cdiService.ExportRequiredLicenses(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "RequiredLicences"));
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
        /// Report Update Create By User
        /// </summary>
        /// <param name="js"></param>
        [HttpGet]
        public void SummaryReport(CdiDashboardDto js)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.ExportLicensesSummary(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "SummaryReport"));
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
        /// Channel History
        /// </summary>
        /// <param name="js"></param>
        [HttpGet]
        public void ChannelHistory(CdiDashboardDto js)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.ChannelHistoryReport(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ChannelHistory"));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public void FourMonths(CdiDashboardDto js)
        {
            try
            {
                IWorkbook workbook = cdiDashboardService.ExportLicensesSummary(js);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ChannelHistory"));
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
        /// Process And Backup
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessAndBackup()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("ProcessAndBackup");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Backup Data
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public JsonResult BackupData(CDIDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UserId = userSession.UserId;
                result = cdiService.BackupData(dto);
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Upload Customer  NHA THUOC BENH VIEN CONG (NTBVC)
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadCustomerNTBVC(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                result = cdiService.UploadCustomerNTBVC(aFile);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// auto alarm certification expired and coming expire
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AutoAlarmCDIReportDaily()
        {
            ResultMessageDto result = cdiDashboardService.AutoAlarmCDIReportDaily();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Setup Timeline
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetupTimeline(SetupTimelineDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userDto = (UserDto)Session[Constant.USER_SESSION];
            if (userDto != null)
            {
                dto.UpdateBy = userDto.UserId;
                result = cdiService.SetupTimeline(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Upload code ship to
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadCodeShipTo(HttpPostedFileBase aFile, string userId)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userDto = (UserDto)Session[Constant.USER_SESSION];
            if (userDto != null)
            {
                if (aFile != null)
                {
                    userId = userDto.UserId;
                    result = cdiService.UploadCodeShipTo(aFile, userId);
                }
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);            
        }

        /// <summary>
        /// Get ShipTo By CustCode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDetailLicenseShipTo(CustomerShipToDto dto)
        {
            var usersession = (UserDto)Session[Constant.USER_SESSION];
            CustomerShipToDto shipto = null;
            if (usersession != null)
            {
                dto.UpdateBy = usersession.UserId;
                shipto = cdiService.GetDetailLicenseShipTo(dto);
            }
            var result = Json(shipto, JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Get ShipTo By CustCode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetShipToByCustCode(CustomerShipToDto dto)
        {
            IEnumerable<CustomerShipToDto> shipto = cdiService.GetShipToByCustCode(dto);
            return Json(shipto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update ShipTo License
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateShipToLicense(CustomerShipToDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UpdateBy = userSession.UserId;
                result = cdiService.UpdateShipToLicense(dto);
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
        /// Get ShipTo By SoldTo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetShipToBySoldTo(CDIDto dto)
        {
            IEnumerable<CustomerShipToDto> result = cdiService.GetShipToBySoldTo(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// ShipTo List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ShipToList(CustomerShipToDto dto)
        {
            IEnumerable<CustomerShipToDto> result = cdiService.ShipToList(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Export ShipTo
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ExportShipTo(CustomerShipToDto dto)
        {
            try
            {
                IEnumerable<CustomerShipToDto> shiptoList = cdiService.ShipToList(dto);
                IWorkbook workbook = cdiService.ExportShipTo(shiptoList, Constant.SHIP_TO_LICENSES, Constant.SHIP_TO_LICENSES_TEMPLATE);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ShipToList"));
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
