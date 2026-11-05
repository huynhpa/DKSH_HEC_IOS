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

namespace DKSHApplication.Controllers
{
    public class SalesPromotionController : Controller
    {

        private SalesPromotionService salesPromotionService;
        private MDPendingOrderService poService;

        public SalesPromotionController() : this (new SalesPromotionServiceImpl(), new MDPendingOrderServiceImpl()) { }

        public SalesPromotionController(SalesPromotionService salesPromotionService, MDPendingOrderService poService)
        {
            this.salesPromotionService = salesPromotionService;
            this.poService = poService;
        }

        /// <summary>
        /// View page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                ProductInformationDto dto = new ProductInformationDto();
                IEnumerable<ProductInformationDto> Suppliers = poService.GetSuppliers(dto);
                ViewBag.Suppliers = Suppliers;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search SalesPromotion
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchSalesPromotion(SalesPromotionDto param)
        {
            IEnumerable<SalesPromotionDto> rs = salesPromotionService.SearchSalesPromotion(param);
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Save SalesPromotion
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSalesPromotion(HttpPostedFileBase aFile, SalesPromotionDto dto)
        {
            ResultMessageDto rs = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null && aFile != null)
            {
                dto.CreateBy = userSession.UserId;
                rs = salesPromotionService.SaveSalesPromotion(aFile, dto);
            }
            else
            {
                rs.code = Constant.SESSION_EXPIRED_CODE;
                rs.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// get sales promotion details
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSalesPromotionDetail(SalesPromotionDto dto)
        {
            SalesPromotionDto spDto = null;
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.CreateBy = userSession.UserId;
                spDto = salesPromotionService.GetSalesPromotionDetail(dto);
            }
            var jsonResult = Json(spDto, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSalesPromotionId(SalesPromotionDto dto)
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            IEnumerable<SalesPromotionDto> lst = salesPromotionService.SalesPromotionIdList();
            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Set Entity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetEntity(SalesPromotionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession =  (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UpdateBy = userSession.UserId;
                result = salesPromotionService.SetEntity(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Sales Promotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateSalesPromotion(SalesPromotionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UpdateBy = userSession.UserId;
                result = salesPromotionService.UpdateSalesPromotion(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void Export(SalesPromotionDto dto)
        {
            try
            {
                IEnumerable<SalesPromotionDto> search = salesPromotionService.SearchSalesPromotion(dto);
                IWorkbook workbook = salesPromotionService.Export(search);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "SalesPromotionReport"));
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
        /// ExportByEntityApply
        /// </summary>
        /// <param name="dto"></param>
        [HttpGet]
        public void ExportByEntityApply(SalesPromotionDto dto)
        {
            try
            {
                string fileZip = salesPromotionService.ExportByEntityApply(dto);
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = Constant.ZIP_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ExportByEntity"));
                    Response.TransmitFile(fileZip);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Sales Promotion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteSalesPromotion(SalesPromotionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                dto.UpdateBy = userSession.UserId;
                result = salesPromotionService.DeleteSalesPromotion(dto);
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
