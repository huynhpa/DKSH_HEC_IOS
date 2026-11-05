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
    public class PersonalProfileController : Controller
    {
        //
        // GET: /PersonalProfile/
        private UserService userService = null;

        public PersonalProfileController() : this(new UserServiceImpl()) { }

        public PersonalProfileController(UserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public ActionResult Index(string Email)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                ViewBag.Personal = userService.ViewProfile(Email);
                return View(ViewBag.Personal);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

    }
}
