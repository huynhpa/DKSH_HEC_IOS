using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.SS.UserModel;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class PharmacyManagementController : Controller
    {
        private TransferNotificationService transferNotificationService = null;

        public PharmacyManagementController() : this(new TransferNotificationServiceImpl()){}
        public PharmacyManagementController(TransferNotificationService transferNotificationService)
        {
            this.transferNotificationService = transferNotificationService;
        }
        //
        // GET: /PharmacyManagement/

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
        /// Auto fill invoice detail by Invoice No ZZ24
        /// </summary>
        /// <returns></returns>
        public ActionResult InvoiceToBank()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("InvoiceToBank");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Upload Data
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAndProcess(HttpPostedFileBase aFile, UploadStatusDto UploadStatus)
        {
            ResultMessageDto result = new ResultMessageDto();
            string curDate = DateTime.Now.ToString("yyyymmddHHmmss");
            UserDto user = (UserDto)Session[Constant.USER_SESSION];            
            if (aFile != null)
            {
                if (Utils.CheckExcelFormat(aFile.FileName.Split('.')[1]))
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/InvoiceToBank/");
                    Directory.CreateDirectory(path);
                    string filePath = path + aFile.FileName + "_" + curDate + "." + aFile.FileName.Split('.')[1];
                    aFile.SaveAs(filePath);
                    if (user != null)
                    {
                        result = transferNotificationService.UploadAndProcess(aFile, user.UserId, UploadStatus);
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
        /// 
        /// </summary>
        [HttpGet]
        public void DownloadTemplate()
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + Constant.TRANSFER_NOTIFICATE_TEMPLATE;               
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

        [HttpGet]
        public JsonResult DownloadInvoices(InvoiceZZ24Dto jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {                
                result = transferNotificationService.DownloadInvoices(jsonParam);
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
        /// Search Invoice
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public JsonResult SearchInvoice(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceZZ24Dto> result = transferNotificationService.TransferNotificationList(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// View Invoice Detail
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public JsonResult ViewInvoiceDetail(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceDetailDto> result = transferNotificationService.ViewInvoiceDetail(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// View Invoice Detail Transfer Log
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ViewInvoiceDetailTransferLog(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceDetailDto> result = transferNotificationService.ViewInvoiceDetailTransferLog(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Export pdf
        /// </summary>
        /// <param name="DeliveryNoList"></param>
        [HttpGet]
        public void ExportPDF(InvoiceZZ24Dto invoice)
        {
            try
            {                
                string fileNm = transferNotificationService.ExportPDF(invoice);
                if (!string.IsNullOrEmpty(fileNm))
                {
                    string pdfFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileNm + Constant.PDF_EXTENSION;
                    Response.Clear();
                    Response.ContentType = Constant.PDF_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", pdfFile));
                    Response.TransmitFile(pdfFile);
                    Response.End();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoice"></param>
        [HttpGet]
        public void ExportPDFIsDelete(InvoiceZZ24Dto invoice)
        {
            try
            {
                string fileNm = transferNotificationService.ExportPDFIsDelete(invoice);
                if (!string.IsNullOrEmpty(fileNm))
                {
                    string pdfFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileNm + Constant.PDF_EXTENSION;
                    Response.Clear();
                    Response.ContentType = Constant.PDF_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", pdfFile));
                    Response.TransmitFile(pdfFile);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Export excel
        /// </summary>
        /// <param name="invoice"></param>
        [HttpGet]
        public JsonResult ExportExcel(InvoiceZZ24Dto invoice)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string fileNm = transferNotificationService.ExportExcel(invoice);
                string xlsxFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileNm + "." + Constant.EXCEL_XLSX;
                if (System.IO.File.Exists(xlsxFile))
                {
                    Response.Clear();
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", xlsxFile));
                    Response.TransmitFile(xlsxFile);
                    Response.End();
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_NOT_EXIST;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ExportExcelTransferLog(InvoiceZZ24Dto invoice)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                string fileNm = transferNotificationService.ExportExcelTransferLog(invoice);
                string xlsxFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + fileNm + "." + Constant.EXCEL_XLSX;
                if (System.IO.File.Exists(xlsxFile))
                {
                    Response.Clear();
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", xlsxFile));
                    Response.TransmitFile(xlsxFile);
                    Response.End();
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.FILE_NOT_EXIST;
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
        /// Share All Delivery
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ShareAllInvoice(InvoiceZZ24Dto invoice)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = transferNotificationService.ShareAllInvoice(invoice);
            return Json(result, JsonRequestBehavior.AllowGet);            
        }

        /// <summary>
        /// Send Invoice
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendInvoice(InvoiceZZ24Dto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userDto = (UserDto)Session[Constant.USER_SESSION];
            if (userDto != null)
            {
                json.UpdateBy = userDto.UserId;
                result = transferNotificationService.SendInvoice(json);
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Invoice
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteInvoice(InvoiceZZ24Dto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userDto = (UserDto)Session[Constant.USER_SESSION];
            if (userDto != null)
            {
                json.UpdateBy = userDto.UserId;
                result = transferNotificationService.DeleteInvoice(json);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Convert PDF
        /// </summary>
        /// <param name="aFile"></param>
        [HttpPost]
        public JsonResult ConvertPDF(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/InvoiceToBank/");
            if (aFile != null)
            {
                string fileNm = aFile.FileName;
                string fileLocation = path + fileNm;
                aFile.SaveAs(fileLocation);
                if (Utils.CheckExcelFormat(aFile.FileName.Split('.')[1]))
                {
                    result = transferNotificationService.ConvertPDF(fileLocation, fileNm.Split('.')[0]);                    
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download PDF Converted
        /// </summary>
        /// <param name="pdfPath"></param>
        [HttpGet]
        public void DownloadPDFConverted(PDFConvertDto pdfConvert)
        {
            try
            {
                string name = Path.GetFileNameWithoutExtension(pdfConvert.FilePath);
                Response.Clear();
                Response.ContentType = Constant.PDF_CONTENT_TYPE;
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", pdfConvert.FileName));
                Response.TransmitFile(pdfConvert.FilePath);
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// View log upload transfer notification
        /// </summary>
        /// <returns></returns>
        public ActionResult TransferLog()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("TransferLog");
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
        [HttpGet]
        public JsonResult TransferNotifiLog(InvoiceZZ24Dto json)
        {
            IEnumerable<InvoiceZZ24Dto> result = transferNotificationService.TransferNotifiLog(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    }
}
