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
    public class MedicalHospitalController : Controller
    {
        private MedicalDeviceService medicalDeviceService;

        public MedicalHospitalController() : this(new MedicalDeviceServiceImpl())
        {
        }
        public MedicalHospitalController(MedicalDeviceService medicalDeviceService)
        {
            this.medicalDeviceService = medicalDeviceService;
        }
        //
        // GET: /MedicalHospital/
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
        /// Selling Quota
        /// </summary>
        /// <returns></returns>
        public ActionResult SellingQuota()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                return View("SellingQuota");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Upload Selling Quota
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadSellingQuota(HttpPostedFileBase aFile, string UpdateDate)
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto usersession = (UserDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                string userId = usersession.UserId;
                if (aFile != null)
                {
                    result = medicalDeviceService.UploadSellingQuota(aFile, userId, UpdateDate);
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
        /// 
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchSellingQuota(SellingQuotaDto jsonParam)
        {
            IEnumerable<SellingQuotaDto> SellingQuotaList = medicalDeviceService.SearchSellingQuota(jsonParam);
            var jsonResult = Json(SellingQuotaList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonParam"></param>
        [HttpGet]
        public void ExportResult(SellingQuotaDto jsonParam)
        {
            try
            {
                string fileName = Constant.SELLING_QUOTA + "-" + DateTime.Now.ToString(Constant.DDMYYYY);
                IWorkbook workbook = medicalDeviceService.ExportResult(jsonParam);
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
        /// Fetch Email
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FetchingEmail(ReadMailDto rm)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = medicalDeviceService.FetchingEmail(rm);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
