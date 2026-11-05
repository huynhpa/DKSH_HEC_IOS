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
    public class CallingTimeController : Controller
    {
        private CallingTimeService callingTimeService;

        /// <summary>
        /// 
        /// </summary>
        public CallingTimeController() : this (new CallingTimeServiceImpl())
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callingTimeService"></param>
        public CallingTimeController(CallingTimeService callingTimeService)
        {
            this.callingTimeService = callingTimeService;
        }


        /// <summary>
        /// Index page
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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchCallingTime(CallingTimeDto dto)
        {
            IEnumerable<CallingTimeDto> result = callingTimeService.SearchCallingTime(dto);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SetCallingTime(CallingTimeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = callingTimeService.SetCallingTime(dto);
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    }
}
