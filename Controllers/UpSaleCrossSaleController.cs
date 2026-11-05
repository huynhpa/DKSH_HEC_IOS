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
    public class UpSaleCrossSaleController : Controller
    {
        private UpSaleCrossSaleService USCRService;

        public UpSaleCrossSaleController() : this(new UpSaleCrossSaleServiceImpl())
        {

        }
        public UpSaleCrossSaleController(UpSaleCrossSaleService USCRService)
        {
            this.USCRService = USCRService;
        }

        //
        // GET: /UpSaleCrossSale/

        public ActionResult Index()
        {
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
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
        /// Get Group Saleman POText
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGroupSalemanPOText()
        {
            IEnumerable<SalemanPOTextDto> result = USCRService.GetGroupSalemanPOText();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get SalemanPO
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSalemanPO()
        {
            IEnumerable<SalemanPOTextDto> result = USCRService.GetSalemanPO();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Upload Saleman POText
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadSalemanPOText(HttpPostedFileBase aFile, string Monthly)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                string userId = usersession.UserId;
                if (aFile != null)
                {
                    result = USCRService.UploadSalemanPOText(aFile, Monthly);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_FILE_SELECTED;
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
        /// Upload Data Calls
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadDataCalls(HttpPostedFileBase aFile, string Monthly)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                string userId = usersession.UserId;
                if (aFile != null)
                {
                    DataCallsDto dto = new DataCallsDto();
                    dto.Monthly = Monthly;
                    result = USCRService.UploadDataCalls(aFile, userId, dto);
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_FILE_SELECTED;
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
        /// Upload Data Sale
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadDataSale(HttpPostedFileBase aFile, DataSaleCSUSDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (aFile != null)
            {
                string fileNm = "DataSale-" + DateTime.Now.ToString("MMyyyy");
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + fileNm + "." + file.Split('.')[1];
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                aFile.SaveAs(path);
                result.code = Constant.SUCCESS;
                result.message = Constant.FILE_UPLOAD_SUCCESS;
                result.FileNm = fileNm;
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.FILE_EXTENSION_INVALID;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Data Sale 
        /// </summary>
        /// <param name="dataSale"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDataSale(HttpPostedFileBase aFile, DataSaleCSUSDto dataSale)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                //string filePath = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + dataSale.FileNm + ".xlsx";
                dataSale.CreateBy = usersession.UserId;
                dataSale.UpdateBy = usersession.UserId;
                result = USCRService.SaveDataSale(aFile, dataSale);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search Up Sale Cross Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchUpSaleCrossSale(CrossSaleUpSaleReportDto dto)
        {
            IEnumerable<CrossSaleUpSaleReportDto> result = USCRService.SearchUpSaleCrossSale(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Process Up Sale Cross Sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcessUpSaleCrossSale(CrossSaleUpSaleReportDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.CreateBy = usersession.UserId;
                dto.UpdateBy = usersession.UserId;
                result = USCRService.ProcessUpSaleCrossSale(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export Up Sale Cross Sales
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ExportUpSaleCrossSales(CrossSaleUpSaleReportDto dto)
        {
            IEnumerable<CrossSaleUpSaleReportDto> uscsResult = USCRService.SearchUpSaleCrossSale(dto);
            try
            {
                IWorkbook workbook = USCRService.ExportUpSaleCrossSales(uscsResult);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "UpSaleCrossSale"));
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
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDataSale(DataSaleParam dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = USCRService.DeleteDataSale(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Data Call Saleman
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDataCallSaleman(DataCallSalemanParam pr)
        {
            var result = new object();
            if (pr != null && pr.Type.Equals("DATACALL"))
            {
                IEnumerable<DataCallsDto> dc = USCRService.GetDataCallByMonth(pr);
                result = dc;
            }
            else if (pr != null && pr.Type.Equals("SALEMAN"))
            {
                IEnumerable<SalemanPOTextDto> smpo = USCRService.GetAllSalemanPOText();
                result = smpo;
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
