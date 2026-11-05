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
    public class TransferNotificationProvinceController : Controller
    {
        //
        // GET: /TransferNotificationProvince/
        private TransferNotificationProvinceService tfProvinceService = null;

        public TransferNotificationProvinceController() : this(new TransferNotificationProvinceServiceImpl()) { }

        public TransferNotificationProvinceController(TransferNotificationProvinceService tfProvinceService)
        {
            this.tfProvinceService = tfProvinceService;
        }
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
        /// Upload And Process
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public JsonResult UploadAndProcess(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto user = (UserDto)Session[Constant.USER_SESSION];
            if (aFile != null)
            {
                if (Utils.CheckExcelFormat(aFile.FileName.Split('.')[1]))
                {
                    if (user != null)
                    {
                        result = tfProvinceService.UploadAndProcess(aFile);
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_INCORRECT_FORMAT;
                }
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search Invoice
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public JsonResult SearchInvoice(TransferNotificationProvinceDto dto)
        {
            IEnumerable<TransferNotificationProvinceDto> result = tfProvinceService.SearchInvoice(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Download Template
        /// </summary>
        [HttpGet]
        public void DownloadTemplate()
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + Constant.TRANSFER_NOTIFICATE_PROVINCE_TEMPLATE;
                Response.Clear();
                Response.BufferOutput = false;
                Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                Response.TransmitFile(fileName);
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UploadEmail(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto user = (UserDto)Session[Constant.USER_SESSION];
            if (aFile != null)
            {
                if (Utils.CheckExcelFormat(aFile.FileName.Split('.')[1]))
                {
                    if (user != null)
                    {
                        result = tfProvinceService.UploadEmail(aFile);
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_INCORRECT_FORMAT;
                }
            }
            else
            {
                result.code = Constant.ERROR;
                result.message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export pdf
        /// </summary>
        /// <param name="DeliveryNoList"></param>
        [HttpGet]
        public void ExportPDF(TransferNotificationProvinceDto invoice)
        {
            try
            {
                string fileNm = tfProvinceService.ExportPDF(invoice);
                string pdfFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileNm + Constant.PDF_EXTENSION;
                Response.Clear();
                Response.ContentType = Constant.PDF_CONTENT_TYPE;
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", pdfFile));
                Response.TransmitFile(pdfFile);
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// View Invoice Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ViewInvoiceDetail(TransferNotificationProvinceDto dto)
        {
            IEnumerable<TransferNotificationProvinceDto> province = tfProvinceService.GetInvoinceDetailProvince(dto);
            return Json(province, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download Invoices
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DownloadInvoices(TransferNotificationProvinceDto jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result = tfProvinceService.DownloadInvoices(jsonParam);
                Response.Clear();
                Response.BufferOutput = false;
                Response.ContentType = Constant.ZIP_CONTENT_TYPE;
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", result.filePath));
                Response.TransmitFile(result.filePath);
                Response.End();
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendInvoice(TransferNotificationProvinceDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userDto = (UserDto)Session[Constant.USER_SESSION];
            if (userDto != null)
            {
                result = tfProvinceService.SendInvoice(dto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
