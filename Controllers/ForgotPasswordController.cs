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
    public class ForgotPasswordController : Controller
    {
        private UserService userService = null;

        public ForgotPasswordController() : this(new UserServiceImpl()) { }

        public ForgotPasswordController(UserService userService)
        {
            this.userService = userService;
        }
        //
        // GET: /ForgotPassword/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResetPassword(ForgotPasswordDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.Email = userSession.Email;
                result = userService.ResetPassword(dto);
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
