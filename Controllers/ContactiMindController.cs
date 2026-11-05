using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class ContactiMindController : Controller
    {

        private ContactiMindService contactiMindService;

        /// <summary>
        /// 
        /// </summary>
        public ContactiMindController() : this(new ContactiMindServiceImpl())
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactiMindService"></param>
        public ContactiMindController(ContactiMindService contactiMindService)
        {
            this.contactiMindService = contactiMindService;
        }
        //
        // GET: /ContactiMind/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult SearchContactiMind(contact_phone_obs json)
        {
            IEnumerable<contact_phone_obs> result = contactiMindService.SearchContactiMind(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ContactPhoneNumber(contact_phone_obs json)
        {
            IEnumerable<contact_phone_obs> result = contactiMindService.ContactPhoneNumber(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// DeletePhoneNumber
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeletePhoneNumber(contact_phone_obs json)
        {
            ResultMessageDto result = contactiMindService.DeletePhoneNumber(json);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}
