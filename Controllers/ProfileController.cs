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
    public class ProfileController : Controller
    {
        private UserService userService = null;

        public ProfileController() : this(new UserServiceImpl()) { }

        public ProfileController(UserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// INIT PAGE
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string Email)
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
        /// Get User Profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserProfile(UserDto userProfile)
        {
            UserDto profile = userService.GetUserDetail(userProfile);
            return Json(profile, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateProfile(UserDto profile)
        {
            ResultMessageDto rs = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                profile.UpdateBy = userSession.UserId;
                rs = userService.UpdateProfile(profile);
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Upload avatar
        /// </summary>
        /// <param name="fileDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAvatar(FileDto fileDto)
        {
            ResultMessageDto result = new ResultMessageDto();            
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                fileDto.userId = userSession.UserId;
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
                    UserDto obj = new UserDto();
                    obj.UserId = userSession.UserId;
                    obj.ImagePath = fileName;
                    result = userService.UpdateImagePath(obj);
                    ViewBag.Message = string.Format(Constant.FILE_UPLOAD, userSession.UserId);
                }
            }
            return RedirectToAction("Index", "Profile", result);
        }
    }
}
