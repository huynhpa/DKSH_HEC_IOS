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
    public class ProductInformationController : Controller
    {
        private ProductInformationService mdPendingOrderService;

        public ProductInformationController() : this(new ProductInformationServiceImpl()) {
        }
        public ProductInformationController(ProductInformationService mdPendingOrderService)
        {
            this.mdPendingOrderService = mdPendingOrderService;
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
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Search Pending Order
        /// </summary>
        /// <returns></returns>
        public ActionResult PendingOrder()
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View("PendingOrder");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Product Information
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductInformation()
        {
            MemberShipDto userSession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                return View("ProductInformation");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

   
        /// <summary>
        /// Get Suppliers
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSuppliers(ProductInformationDto jsonParam)
        {
            IEnumerable<ProductInformationDto> result = mdPendingOrderService.GetSuppliers(jsonParam);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Search Products
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchProduct(ProductInformationDto jsonParam)
        {
            IEnumerable<ProductInformationDto> result = mdPendingOrderService.SearchProduct(jsonParam);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }     
    }
}
