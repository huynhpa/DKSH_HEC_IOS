using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.CampaignManagement
{
    public class CampaignController : Controller
    {
        private CampaignService campaignService;

        public CampaignController() : this(new CampaignServiceImpl())
        { }
        public CampaignController(CampaignService campaignService)
        {
            this.campaignService = campaignService;
        }


        /// <summary>
        /// index load page
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
        /// GetCampaign
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCampaign(CampaignDto dto)
        {
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            CampaignDto campaign = new CampaignDto();
            if (user != null)
            {
                campaign = campaignService.GetCampaign(dto);
            }
            var jsonResult = Json(campaign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Search(CampaignDto dto)
        {
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            CampaignDto campaign = new CampaignDto();
            if (user != null)
            {
                campaign.ServiceTypeList = campaignService.Search(dto);
            }
            var jsonResult = Json(campaign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertCampaign(ServiceTypeDto dto)
        {
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (user != null)
            {
                dto.CreatedBy = user.UserName;
                dto.UpdatedBy = user.UserName;
                result = campaignService.InsertCampaign(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateCampaign(ServiceTypeDto dto)
        {
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (user != null)
            {
                dto.UpdatedBy = user.UserName;
                result = campaignService.UpdateCampaign(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Delete Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCampaign(ServiceTypeDto dto)
        {
            MemberShipDto user = (MemberShipDto)Session[Constant.USER_SESSION];
            ResultMessageDto result = new ResultMessageDto();
            if (user != null)
            {
                dto.UpdatedBy = user.UserName;
                result = campaignService.DeleteCampaign(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Campaign Detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CampaignDetail(ServiceTypeDto param)
        {
            ServiceTypeDto result = campaignService.CampaignDetail(param);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        [HttpGet]
        public void ViewAgentInCampaign(SearchCallDto json)
        {
            try
            {
                IWorkbook workbook = campaignService.ViewAgentInCampaign(json);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "AgentInCampaign"));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
