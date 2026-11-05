using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.CampaignManagement
{
    public class ImportPhoneNumberController : Controller
    {
        private ImportPhoneNumberService phonenumberService;
        public ImportPhoneNumberController() : this (new ImportPhoneNumberServiceImpl())
        {

        }
        public ImportPhoneNumberController(ImportPhoneNumberService phonenumberService)
        {
            this.phonenumberService = phonenumberService;
        }


        //
        // GET: /ImportPhoneNumber/

        public ActionResult Index()
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
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
        /// SearchPhoneNumber
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchPhoneNumber(PhoneNumberDto dto)
        {
            IEnumerable<PhoneNumberDto> rs = phonenumberService.SearchPhoneNumber(dto);
            var js = Json(rs, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SummaryPhoneNumber(PhoneNumberDto dto)
        {
            IEnumerable<SummaryPhoneDto> rs = phonenumberService.SummaryPhoneNumber(dto);
            var js = Json(rs, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// UploadPhoneNumber
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadPhoneNumber(HttpPostedFileBase aFile, PhoneNumberDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null && aFile != null)
            {
                dto.CreatedBy = userSession.UserName;
                dto.ProjectId = userSession.GroupID;
                dto.WindowsUser = userSession.UserLogin;
                dto.Extension = userSession.AgentNumber;
                result = phonenumberService.UploadPhoneNumber(aFile, dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ImportPhoneNumberTemplate
        /// </summary>
        [HttpGet]
        public void PhoneNumberTemplate()
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + "PhoneNumberTemplate.xlsx";
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
