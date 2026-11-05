using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers.Dashboard
{
    public class DashboardTelemarketingController : Controller
    {
        private DashboardTLMService dashboardTLMService = null;
        //
        // GET: /DashboardTLM/
        public DashboardTelemarketingController(): this(new DashboardTLMServiceImpl()){ }

        public DashboardTelemarketingController(DashboardTLMService dashboardTLMService)
        {
            this.dashboardTLMService = dashboardTLMService;
        }
        public ActionResult Index()
        {
            return View();
        }


        
    }
}
