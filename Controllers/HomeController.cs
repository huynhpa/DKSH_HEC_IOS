using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using MP.Core.Library;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class HomeController : Controller
    {
        private HomeService homeService = null;
        private RoleService roleService = null;
        public HomeController() : this(new HomeServiceImpl(), new RoleServiceImpl()) { }

        public HomeController(HomeService homeService, RoleService roleService)
        {
            this.homeService = homeService;
            this.roleService = roleService;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                if (!string.IsNullOrEmpty(usersession.EmpLevels) && !usersession.EmpLevels.Equals("AGENT")) 
                {
                    ViewBag.LEADER_EXTENSION = usersession.AgentNumber.Trim();
                }                
                else
                {
                    ViewBag.AGENT_EXTENSION = usersession.AgentNumber.Trim();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Dashboard Summary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DashboardSummary(OBSDashboardDto dto)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            OBSDashboardDto result = new OBSDashboardDto();
            if (usersession != null)
            {
                result = homeService.OBSDashboardSummary(dto);                
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        /// <summary>
        /// Summary call performance by Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SummaryByCampaign(OBSDashboardDto dto)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.Agent = usersession.WindowsUser.Substring(0, 3);
            }
            IEnumerable<SummaryByCampaignDto> result = homeService.SummaryByCampaign(dto);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpGet]
        public JsonResult SupsList()
        {
            IEnumerable<AgentByLeaderDto> result = homeService.SupsList();
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SaleRepBySup(AgentByLeaderDto dto)
        {
            IEnumerable<AgentByLeaderDto> result = homeService.SaleRepBySup(dto);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        /// <summary>
        /// ExportSummaryByCampaign
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void ExportSummaryByCampaign(OBSDashboardDto json)
        {
            try
            {
                string fileNm = "SummaryByCampaign-" + json.Month + ".xlsx";
                IEnumerable<SummaryByCampaignDto> result = homeService.SummaryByCampaign(json);
                IWorkbook workbook = homeService.ExportSummaryByCampaign(result);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileNm));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SummaryByAgent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SummaryByAgent(OBSDashboardDto dto)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.Agent = usersession.WindowsUser.Substring(0, 3);
            }
            IEnumerable<SummaryByAgentDto> result = homeService.SummaryByAgent(dto);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RefreshPerformanceData(OBSDashboardDto dto)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.Agent = usersession.WindowsUser.Substring(0, 3);
            }
            ResultMessageDto result = homeService.RefreshPerformanceData(dto);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        /// <summary>
        /// ExportSummaryByAgent
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void ExportSummaryByAgent(OBSDashboardDto json)
        {
            try
            {
                string fileNm = "SummaryByAgent.xlsx";                
                IEnumerable<SummaryByAgentDto> result = homeService.SummaryByAgent(json);
                IWorkbook workbook = homeService.ExportSummaryByAgent(result);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileNm));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sign Out
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignOut()
        {
            Session[Constant.USER_SESSION] = null;
            return RedirectToAction("Index", "Login");
        }
        
        

    }
}
