using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.CampaignManagement
{
    public class ServiceTypeController : Controller
    {
        public ServiceTypeService serviceTypeService;

        public ServiceTypeController() : this (new ServiceTypeServiceImpl())
        { }
        public ServiceTypeController(ServiceTypeService serviceTypeService)
        {
            this.serviceTypeService = serviceTypeService;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }            
        }

        /// <summary>
        /// GetActiveServiceType
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetActiveServiceType(ServiceTypeDto dto)
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.WindowUser = userSession.UserName;
            }
            IEnumerable<ServiceTypeDto> result = serviceTypeService.GetActiveServiceType(dto);                            
            var js = Json(result, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

    }
}
