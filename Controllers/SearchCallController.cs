using DKSHApplication.Common;
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
    public class SearchCallController : Controller
    {
        private SearchCallService searchCallService = null;

        public SearchCallController() : this(new SearchCallServiceImpl()) { }

        public SearchCallController(SearchCallService searchCallService)
        {
            this.searchCallService = searchCallService;
        }
        //
        // GET: /SearchCall/

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
        /// Search
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Search(SearchCallDto json)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                json.Extension = usersession.Extension;
            }
            IEnumerable<SearchCallDto> result = searchCallService.Search(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Search EffectiveCampaign History
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchEffectiveCampaignHistory(SearchCallDto json)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                json.Extension = usersession.Extension;
            }
            IEnumerable<SearchCallDto> result = searchCallService.SearchEffectiveCampaignHistory(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Report Call
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void ReportCall(SearchCallDto json)
        {
            try
            {
                MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
                if (usersession != null)
                {
                    json.Extension = usersession.Extension;
                }
                IEnumerable<SearchCallDto> searchCall = searchCallService.Search(json);
                IWorkbook workbook = searchCallService.ReportCall(searchCall, false);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void EffectiveCampaignHistory(SearchCallDto json)
        {
            try
            {
                MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
                if (usersession != null)
                {
                    json.Extension = usersession.Extension;
                }
                //IEnumerable<SearchCallDto> result = searchCallService.SearchEffectiveCampaignHistory(json);
                IWorkbook workbook = searchCallService.ExportEffectiveCampaign(json);
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

        public void CallCampaignHistory(SearchCallDto json)
        {
            try
            {
                MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
                if (usersession != null)
                {
                    json.Extension = usersession.Extension;
                }
                IWorkbook workbook = searchCallService.CallCampaignHistory(json);
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

        /// <summary>
        /// Get Campaign List
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCampaignList()
        {
            IEnumerable<ServiceTypeDto> rs = searchCallService.GetCampaignList();
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Survey Result
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void SurveyResult(SearchCallDto dto)
        {
            try
            {
                string now = DateTime.Now.ToString("dd-MM-yyyy-HHmmss");
                string fileName = string.Format("SurveyResult_{0}", now);
                //IEnumerable<SearchCallDto> searchCall = searchCallService.Search(dto);
                IWorkbook workbook = searchCallService.SurveyResult(dto);
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
        /// duplicate call report
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void DuplicateCall(SearchCallDto json)
        {
            try
            {
                IWorkbook workbook = searchCallService.DuplicateCall(json);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "DuplicateCall"));
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
        /// Actice outlet daily report
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void ActiveOutletDaily(SearchCallDto json)
        {
            try
            {
                IWorkbook workbook = searchCallService.ActiveOutletDaily(json);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ActiveOutletDaily"));
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
        /// 
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void SFEReport(SearchCallDto json)
        {
            try
            {
                IWorkbook workbook = searchCallService.SFEReport(json);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "SFEReport"));
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
        /// Get Project List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProjectList()
        {
            IEnumerable<ProjectDto> rs = searchCallService.GetProjectList();
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// upload customer
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadCustomer(HttpPostedFileBase aFile, SearchCallParam dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                if (aFile != null)
                {
                    dto.UserId = usersession.UserId;
                    dto.UserName = usersession.UserName;
                    result = searchCallService.UploadCustomer(aFile, dto);
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
        /// Process Active Outlet Daily
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcessActiveOutletDaily(SearchCallParam dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                
                result = searchCallService.ProcessActiveOutletDaily(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
