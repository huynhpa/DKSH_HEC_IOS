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
    public class AssignmentController : Controller
    {
        public AssignmentService assignmentService;
        public ServiceTypeService serviceTypeService;
        private ProjectService projectService;

        public AssignmentController() : this (new AssignmentServiceImpl(), new ServiceTypeServiceImpl(), new ProjectServiceImpl())
        { }
        public AssignmentController(AssignmentService assignmentService, ServiceTypeService serviceTypeService, ProjectService projectService)
        {
            this.assignmentService = assignmentService;
            this.serviceTypeService = serviceTypeService;
            this.projectService = projectService;
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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ViewAssignment(AssignmentDto dto)
        {
            IEnumerable<AssignmentDto> result = assignmentService.ViewAssignment(dto);
            var js = Json(result, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = int.MaxValue;
            return js;
        }

        /// <summary>
        /// Remove Assignment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RemoveAssignment(AssignmentDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            result = assignmentService.RemoveAssignment(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Project By Window User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProjectByWindowUser()
        {
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            ProjectDto project = new ProjectDto();
            if (user != null)
            {
                project.ID = user.GroupID;
                project = projectService.GetProjectDetail(project);
            }
            List<ProjectDto> result = assignmentService.GetProjectByWindowUser(project);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}
