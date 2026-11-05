using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class RoleController : Controller
    {
        private RoleService roleService = null;

        public RoleController() : this(new RoleServiceImpl()) { }

        public RoleController(RoleService roleService)
        {
            this.roleService = roleService;
        }
        /// <summary>
        /// INIT PAGE
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
        /// All Role
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public JsonResult GetRoleList()
        {
            IEnumerable<RoleDto> result = roleService.GetRoleList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchRole(RoleDto dto)
        {
            IEnumerable<RoleDto> result = roleService.SearchRole(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoleDetail(RoleDto dto)
        {
            RoleDto result = roleService.GetRoleDetail(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertRole(RoleDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = roleService.InsertRole(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateRole(RoleDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = roleService.UpdateRole(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelRole(RoleDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                result = roleService.DelRole(dto);
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
