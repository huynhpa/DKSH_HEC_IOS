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
    public class UserController : Controller
    {
        private UserService userService = null;
        private RoleService roleService = null;

        public UserController() : this(new UserServiceImpl(), new RoleServiceImpl()) { }

        public UserController(UserService userService, RoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }
        /// <summary>
        /// Init page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                var RoleList = roleService.GetRoleIdList();
                ViewBag.RoleList = RoleList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchUser(UserDto param) {
            IEnumerable<UserDto> listUser = userService.SearchUser(param);
            var jsonResult = Json(listUser, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertUser(UserDto param)
        {
            ResultMessageDto rs = new ResultMessageDto();
            if (Session[Constant.USER_SESSION] != null)
            {
                var userSession = (UserDto)Session[Constant.USER_SESSION];
                param.CreateBy = userSession.CreateBy;
                param.UpdateBy = userSession.UpdateBy;
            }
            rs = userService.InsertUser(param);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get User Detail
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserDetail(UserDto user)
        {
            UserDto dto = userService.GetUserDetail(user);
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUser(UserDto dto)
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                var userSession = (UserDto)Session[Constant.USER_SESSION];
                dto.UpdateBy = userSession.UserId;
            }
            ResultMessageDto rs = userService.UpdateUser(dto);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteUser(int Id)
        {
            ResultMessageDto rs = new ResultMessageDto();
            rs = userService.DeleteUser(Id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <returns></returns>
        public JsonResult ForgotPassword(ForgotPasswordDto dto)
        {
            ForgotPasswordDto rs = new ForgotPasswordDto();
            rs = userService.ForgotPassword(dto);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public JsonResult ResetPassword(ForgotPasswordDto jsonParam)
        {
            ResultMessageDto dto = new ResultMessageDto();
            dto = userService.ResetPassword(jsonParam);
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// View Profile
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ActionResult ViewProfile(UserDto dto)
        {
            dto = userService.GetUserDetail(dto);
            return RedirectToAction("Index", "Profile", dto);
        }

        /// <summary>
        /// ActivedUser
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivedUser(UserDto json)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = userService.ActivedUser(json);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check Session Timeout
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckSessionTimeout()
        {
            ResultMessageDto result = new ResultMessageDto();
            UserDto userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                result.code = Constant.SESSION_OK;
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
