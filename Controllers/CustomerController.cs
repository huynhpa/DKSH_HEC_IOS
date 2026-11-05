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
    public class CustomerController : Controller
    {
        private CustomerService customerService;

        public CustomerController()
            : this(new CustomerServiceImpl())
        {
        }
        public CustomerController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// INIT
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
        /// Search Customer
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchCustomer(CustomerDto jsonParam)
        {
            IEnumerable<CustomerDto> customerList = customerService.SearchCustomer(jsonParam);
            var jsonResult = Json(customerList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public void Template()
        {
            try
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/") + "CustomerInformation.xlsx";
                using (MemoryStream exportData = new MemoryStream())
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = Constant.XLSX_CONTENT_TYPE;
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.TransmitFile(fileName);
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
