using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.Project
{
    public class ProjectController : Controller
    {

        private ProjectService projectService;
        public ProjectController() : this (new ProjectServiceImpl())
        {

        }
        public ProjectController(ProjectService projectService)
        {
            this.projectService = projectService;
        }


        //
        // GET: /Project/
        public ActionResult Index()
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search Project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchProject(ProjectDto dto)
        {
            IEnumerable<ProjectDto> rs = projectService.SearchProject(dto);
            var js = Json(rs, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// get project detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetProjectDetail(ProjectDto dto)
        {
            ProjectDto rs = projectService.GetProjectDetail(dto);
            var js = Json(rs, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// insert project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertProject(ProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.CreatedBy = userSession.UserName;
                dto.UpdatedBy = userSession.UserName;
                result = projectService.InsertProject(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }

            var js = Json(result, JsonRequestBehavior.AllowGet);
            return js;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateProject(ProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UpdatedBy = userSession.UserName;
                result = projectService.UpdateProject(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }

            var js = Json(result, JsonRequestBehavior.AllowGet);
            return js;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteProject(ProjectDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UpdatedBy = userSession.UserName;
                result = projectService.DeleteProject(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }

            var js = Json(result, JsonRequestBehavior.AllowGet);
            return js;
        }

    }
}
