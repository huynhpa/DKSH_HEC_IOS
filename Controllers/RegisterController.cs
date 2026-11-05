using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class RegisterController : Controller
    {
        private UserService userService = null;
        private RoleService roleService = null;

        public RegisterController() : this(new UserServiceImpl(), new RoleServiceImpl()) { }

        public RegisterController(UserService userService, RoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }
        /// <summary>
        /// Register
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Registration(UserDto userDto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            rs = userService.Registration(userDto);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }



    }
}
