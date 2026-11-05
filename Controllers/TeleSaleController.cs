using DKSHApplication.Common;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class TeleSaleController : Controller
    {
        //
        // GET: /TeleSale/

        public ActionResult Index()
        {
            var userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        
    }
}
