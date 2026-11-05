using DKSHApplication.Common;
using DKSHApplication.DB;
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
    public class TeleMarketingController : Controller
    {
        private MarketingProjectService mktService = null;

        public TeleMarketingController() : this(new MarketingProjectServiceImpl()){}

        public TeleMarketingController(MarketingProjectService mktService)
        {
            this.mktService = mktService;
        }
        //
        // GET: /Marketing Project/

        public ActionResult Index()
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Allocate Human Resource
        /// </summary>
        /// <returns></returns>
        public ActionResult AllocateHumanResource()
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View("AllocateHumanResource");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Redirect detail of marketing project
        /// </summary>
        /// <returns></returns>
        public ActionResult MarketingProjectDetail()
        {
            return View("Detail");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProjectName()
        {
            IEnumerable<MarketingProjectDto> projectNameList = mktService.GetProjectNameList();
            var jsonResult = Json(projectNameList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult Search(MarketingProjectDto collection)
        {
            IEnumerable<MarketingProjectDto> lst = mktService.Search(collection);
            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get Project By Id
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetProjectById(MarketingProjectDto collection)
        {
            MarketingProjectDto obj = mktService.GetProjectById(collection);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// INSERT
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Insert(MarketingProjectDto json)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (userSession != null)
            {
                json.CreateBy = userSession.UserId;
                json.UpdateBy = userSession.UserId;
                result = mktService.Insert(json);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Update(MarketingProjectDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = mktService.Update(json);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Delete(MarketingProjectDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = mktService.Delete(json);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Saleman Code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSalemanCode(SalemanCodeDto json)
        {
            MarketingResultDto dto = new MarketingResultDto();
            IEnumerable<SalemanCodeDto> saleman = mktService.GetSalemanCode(json);
            IEnumerable<string> team = mktService.TeamList();
            IEnumerable<ChannelDto> channel = mktService.GetChannel();
            dto.SalemanCodeList = saleman;
            dto.TeamList = team;
            dto.Channel = channel;
            var jsonResult = Json(dto, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// get data general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDataGeneral(SalemanCodeDto saleman)
        {
            MarketingResultDto dto = new MarketingResultDto();
            IEnumerable<ChannelDto> channel = mktService.GetChannel();
            IEnumerable<RegionDto> region = mktService.GetRegion();
            if (saleman.isView == false)
            {
                dto.ProjectNumber = Utils.GenerateRandomNo().ToString();
            }
            dto.Channel = channel;
            dto.RegionList = region;
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download Template (Products, Customers)
        /// </summary>
        [HttpGet]
        public void DownloadTemplate(MarketingProjectDto json)
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + Constant.MARKETING_TEMPLATE;
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = Constant.ZIP_CONTENT_TYPE;
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

        /// <summary>
        /// upload products
        /// </summary>
        /// <param name="aFile"></param>
        [HttpPost]
        public JsonResult UploadProduct(HttpPostedFileBase aFile, string projectNumber)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + projectNumber.ToString() + "_Products";
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                result.code = file.Split('.')[1];
                result.message = Constant.FILE_UPLOAD_SUCCESS;
            }
            else
            {
                result.message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// upload customer
        /// </summary>
        /// <param name="aFile"></param>
        [HttpPost]
        public JsonResult UploadCustomer(HttpPostedFileBase aFile, string projectNumber)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + projectNumber + "_Customers";
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                result.code = file.Split('.')[1];
                result.message = Constant.FILE_UPLOAD_SUCCESS;
            }
            else
            {
                result.message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// upload sale man resources
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="projectNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadSaleman(HttpPostedFileBase aFile, string projectNumber)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + projectNumber + "_Resources";
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                result.code = file.Split('.')[1];
                result.message = Constant.FILE_UPLOAD_SUCCESS;
            }
            else
            {
                result.message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// download project details
        /// </summary>
        [HttpGet]
        public void DownloadProjectDetail(MarketingProjectDto jsonObj)
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + Constant.MARKETING_TEMPLATE;
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = Constant.ZIP_CONTENT_TYPE;
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

        /// <summary>
        /// upload customer
        /// </summary>
        /// <param name="aFile"></param>
        [HttpPost]
        public JsonResult UploadDataRaw(HttpPostedFileBase aFile)
        {
            string message = string.Empty;
            if (aFile != null)
            {
                string currDate = Utils.Now(Constant.YYYYMMDD);
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + currDate + "_" + Constant.INHOUSE;
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                message = Constant.FILE_UPLOAD_SUCCESS;
            }
            else
            {
                message = Constant.NO_FILE_SELECTED;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadDataPick(HttpPostedFileBase aFile, MarketingProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            if (aFile != null && userSession != null)
            {
                dto.UpdateBy = userSession.UserId;
                dto.CreateBy = userSession.UserId;
                result = mktService.UpdateDataPick(aFile, dto);
            }
            else if (userSession == null)
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Process project selected
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcessProject(ProjectProcessParam jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                string createBy = userSession.UserId;
                string currDate = Utils.Now(Constant.YYYYMMDD);
                IEnumerable<MarketingProjectDto> projectList = mktService.GetProjectSelected(jsonParam);
                result = mktService.ProcessProject(projectList, currDate, createBy);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export Data Pick
        /// </summary>
        /// <param name="projectDto"></param>
        [HttpGet]
        public JsonResult ExportDataPick(MarketingProjectDto projectDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IEnumerable<MarketingDataRawDto> projectDataPick = mktService.GetMarketingProjectByProjectNumber(projectDto);
                IWorkbook workbook = mktService.ExportDataPick(projectDataPick, projectDto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", projectDto.ProjectName));
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
        /// Get Marketing Project By Project Number
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMarketingProjectByProjectNumber(MarketingProjectDto projectDto)
        {
            IEnumerable<MarketingDataRawDto> result = mktService.GetMarketingProjectByProjectNumber(projectDto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Count Data Raw By Project Number
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CountDataRawByProjectNumber(ProjectProcessParam jsonParam)
        {
            IEnumerable<DataRawResult> result = mktService.CountDataRawByProjectNumber(jsonParam);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Project Over View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ProjectOverView(MarketingProjectDto param)
        {
            IEnumerable<MarketingProjectDto> result = mktService.ProjectOverView(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Download Product
        /// </summary>
        /// <param name="proj"></param>
        [HttpGet]
        public JsonResult DownloadProduct(MarketingProjectDto proj)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + proj.ProjectNumber + "_Products.xlsx";
                if (System.IO.File.Exists(fileName))
                {
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
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_NOT_EXIST;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download Resource
        /// </summary>
        /// <param name="proj"></param>
        [HttpGet]
        public JsonResult DownloadResource(MarketingProjectDto proj)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + proj.ProjectNumber + "_Resources.xlsx";
                if (System.IO.File.Exists(fileName))
                {
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
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_NOT_EXIST;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download Customer
        /// </summary>
        /// <param name="proj"></param>
        [HttpGet]
        public JsonResult DownloadCustomer(MarketingProjectDto proj)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + proj.ProjectNumber + "_Customers.xlsx";
                if (System.IO.File.Exists(fileName))
                {
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
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_NOT_EXIST;
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
