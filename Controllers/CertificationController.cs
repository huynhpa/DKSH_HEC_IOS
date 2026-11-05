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
    public class CertificationController : Controller
    {
        private CDIService cdiService = null;
        private UserService userService = null;

        public CertificationController() : this (new CDIServiceImpl(), new UserServiceImpl()) { }
        public CertificationController(CDIService cdiService, UserService userService)
        {
            this.cdiService = cdiService;
            this.userService = userService;
        }

        /// <summary>
        /// Init Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                /* update certification whether valid */
                int count = cdiService.IsCertification();
                IEnumerable<UserDto> userTeamCDI = userService.GetUserByTeamCDI();
                ViewBag.UserTeamCDI = userTeamCDI;
                ViewBag.Count = count;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search Certification
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchCertification(CertificateParam ctfDto)
        {
            ResultCertificateDto rsCertificate = new ResultCertificateDto();
            rsCertificate.certificateList = cdiService.SearchCertification(ctfDto);
            rsCertificate.certificateInvalid = cdiService.GetCertificateExpired();
            var jsonResult =  Json(rsCertificate, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Update certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateCertification(CertificationDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (Session[Constant.USER_SESSION] != null)
            {
                var userSession = (UserDto)Session[Constant.USER_SESSION];
                dto.UserId = userSession.UserId;
                result = cdiService.UpdateCertification(dto);
            }
            else
            {
                result.code = Constant.USER_SESSION;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Insert Certification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertCertification(CertificationDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (Session[Constant.USER_SESSION] != null)
            {
                var userSession = (UserDto)Session[Constant.USER_SESSION];
                dto.UserId = userSession.UserId;
                dto.CreateBy = userSession.UserId;
                result = cdiService.InsertCertification(dto);
            }
            else
            {
                result.code = Constant.USER_SESSION;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Get Certificate Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCertificateDetail(CertificateParam dto)
        {
            CertificationDto obj = cdiService.GetCertificateDetail(dto);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCustomer(CertificateParam param)
        {
            CDIDto dto = new CDIDto();
            dto.CustCode = param.CustCode;
            ResultMessageDto result = cdiService.UnusedCDI(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// get is certification
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetIsCertification(CertificateParam param)
        {
            IEnumerable<CertificationDto> lst = cdiService.GetIsCertification(param);
            var jsonRs = Json(lst, JsonRequestBehavior.AllowGet);
            jsonRs.MaxJsonLength = int.MaxValue;
            return jsonRs;
        }

        /// <summary>
        /// SearchCertificateInvalid
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchCertificateInvalid(CertificateParam jsonParam)
        {
            IEnumerable<CertificationDto> lst = cdiService.SearchCertificateInvalid(jsonParam);
            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    }
}
