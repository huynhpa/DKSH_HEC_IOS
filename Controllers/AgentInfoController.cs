using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class AgentInfoController : Controller
    {
        private AgentService agentService;
        public AgentInfoController() : this (new AgentServiceImpl())
        {

        }
        public AgentInfoController(AgentService agentService)
        {
            this.agentService = agentService;
        }
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangePassword(MemberShipDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.UserId = usersession.UserId;
                dto.UserName = usersession.UserName;
                result = agentService.ChangePassword(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// update user profile
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateProfile(MemberShipDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.UserId = usersession.UserId;
                dto.UserName = usersession.UserName;
                result = agentService.UpdateProfile(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAvatar(FileDto fileDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                fileDto.userId = usersession.UserName;
                if (fileDto.files != null)
                {
                    HttpPostedFileBase file = fileDto.files.First();
                    string ext = Path.GetExtension(Path.GetFileName(file.FileName));
                    string fileName = fileDto.userId + ext;
                    var ServerSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Images/MemberPictures/")) + fileName;
                    if (System.IO.File.Exists(ServerSavePath))
                    {
                        System.IO.File.Delete(ServerSavePath);
                    }
                    file.SaveAs(ServerSavePath);
                    // UPDATE IMAGE TO USER
                    MemberShipDto obj = new MemberShipDto();
                    obj.UserId = usersession.UserId;
                    obj.ImagePath = fileName;
                    result = agentService.UpdateImagePath(obj);
                    ViewBag.Message = string.Format(Constant.FILE_UPLOAD, usersession.UserId);
                }
            }
            return RedirectToAction("Index", "AgentInfo");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserProfile(MemberShipDto userProfile)
        {
            MemberShipDto profile = agentService.GetUserProfile(userProfile);
            return Json(profile, JsonRequestBehavior.AllowGet);
        }
    }
}
