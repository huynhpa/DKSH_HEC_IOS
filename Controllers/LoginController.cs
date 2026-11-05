using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using MP.Core.Library;

namespace DKSHApplication.Controllers
{
    public class LoginController : Controller
    {
        private LoginService loginService = null;

        public LoginController() : this (new LoginServiceImpl()) { }

        public LoginController(LoginService loginService)
        {
            this.loginService = loginService;
        }
        //
        // GET: /SignIn/
        /// <summary>
        /// Init page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// SignIn
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [WebMethod(EnableSession = true)]
        public ActionResult SignIn(LoginParams param)
        {
            try
            {
                MemberShipDto memberDto = loginService.Login(param);
                if (memberDto != null)
                {
                    if (memberDto.IsLockedOut == true)
                    {
                        ViewBag.ErrorMessage = Constant.OBS_ACCOUNT_LOCKED;
                        return View("Index");
                    }
                    else
                    {
                        Session.Add(Constant.USER_SESSION, memberDto);
                        Session.Timeout = 1200; // set timeout session 120 minutes
                        var authTicket = new FormsAuthenticationTicket(1,
                                                  memberDto.UserName,  //user id
                                                  DateTime.Now,
                                                  DateTime.Now.AddDays(5),  // expiry
                                                  param.RememberMe,  //true to remember
                                                  "", //roles 
                                                  "/"
                                                );
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Incorrect UserName or Password";
                    return View("Index");
                }
            }
            catch (Exception e) {
                ViewBag.ErrorMessage = "Error : " + e.Message;
                return View("Index");
            }
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            return View("ResetPassword");
        }

    }
}
