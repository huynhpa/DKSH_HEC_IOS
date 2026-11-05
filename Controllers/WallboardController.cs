using DKSHApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.Service;
using System.Web.Mvc;
using DKSHApplication.DTO;

namespace DKSHApplication.Controllers
{
    public class WallboardController : Controller
    {
        private WallboardService wallboardService = null;

        public WallboardController() : this(new WallboardServiceImpl()) { }

        public WallboardController(WallboardService wallboardService)
        {
            this.wallboardService = wallboardService;
        }


        // GET: /Wallboard/
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
        /// Initialize Wallboard
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult InitializeWallboard()
        {
            WallboardDto result = wallboardService.InitializeWallboard();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Save Records
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveRecords()
        {
            ResultMessageDto result = new ResultMessageDto();
            result = wallboardService.SaveRecords();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveVASCampaign()
        {
            ResultMessageDto result = new ResultMessageDto();
            result = wallboardService.SaveVASCampaign();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
