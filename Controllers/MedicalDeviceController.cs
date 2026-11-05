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
    public class MedicalDeviceController : Controller
    {

        private MedicalDeviceService medicalDeviceService;

        public MedicalDeviceController() : this(new MedicalDeviceServiceImpl())
        {
        }
        public MedicalDeviceController(MedicalDeviceService medicalDeviceService)
        {
            this.medicalDeviceService = medicalDeviceService;
        }
        //
        // GET: /MedicalDevice/

        public ActionResult Index()
        {
            return View();
        }        

        /// <summary>
        /// Upload Customer
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadVA05(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            //string curDate = DateTime.Now.ToString("yyyymmdd");
            if (aFile != null)
            {
                //string file = aFile.FileName;
                //string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + curDate + "_Customers";
                //string filePath = path + "." + file.Split('.')[1];
                //aFile.SaveAs(filePath);
                result = medicalDeviceService.UploadVA05(aFile);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ExportDataPick()
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                IWorkbook workbook = medicalDeviceService.ExportResult();
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "OrderCount"));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
