using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class DataProcessController : Controller
    {
        //
        // GET: /DataProcess/
        private DataProcessService dataProcessService;

        public DataProcessController()
            : this(new DataProcessServiceImpl())
        {
        }
        public DataProcessController(DataProcessService dataProcessService)
        {
            this.dataProcessService = dataProcessService;
        }

        /// <summary>
        /// 
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
        /// UploadDataProcess
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDataProcess(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            string message = string.Empty;
            if (aFile != null)
            {
                string currDate = Utils.Now(Constant.YYYYMMDD);
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + "DataProcess";
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                message = Constant.FILE_UPLOAD_SUCCESS;
                result = dataProcessService.UploadDataProcess(aFile);
            }
            else
            {
                message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UploadCustomerClassSFA
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveClassSFA(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            string message = string.Empty;
            if (aFile != null)
            {
                string currDate = Utils.Now(Constant.YYYYMMDD);
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + "CustomerClassSFA";
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                message = Constant.FILE_UPLOAD_SUCCESS;
                result = dataProcessService.UploadCustomerClassSFA(aFile);
            }
            else
            {
                message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveClient()
        {
            ResultMessageDto result = new ResultMessageDto();
            result = dataProcessService.SaveClient();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Data Sales Inhouse
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDataSalesInhouse(HttpPostedFileBase aFile)
        {
            ResultMessageDto result = new ResultMessageDto();
            string message = string.Empty;
            if (aFile != null)
            {
                string currDate = Utils.Now(Constant.YYYYMMDD);
                string file = aFile.FileName;
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FilesUpload/") + "DataSaleInhouse";
                aFile.SaveAs(path + "." + file.Split('.')[1]);
                message = Constant.FILE_UPLOAD_SUCCESS;
                result = dataProcessService.SaveDataSalesInhouse(aFile);
            }
            else
            {
                message = Constant.NO_FILE_SELECTED;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
