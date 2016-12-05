using OrderManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderManagement.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public class queryParam
	{
		public int limit{get;set;}
		public int offset{get;set;}
        public string departmentname { get; set; }
		public string status{get;set;}
		public string sortName{get;set;}
		public string sortOrder{get;set;}
	}

        /// <summary>
        /// 用于渲染订单页面
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public JsonResult GetOrder(queryParam queryParams)
        {
            OrderManageDbContext db = new OrderManageDbContext();
            List<Order> data = db.Orders.ToList();
            var total = data.Count;
            var rows = data.Skip(queryParams.offset).Take(queryParams.limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用于渲染账户管理页面
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public JsonResult GetUserInfo(queryParam queryParams)
        {
            OrderManageDbContext db = new OrderManageDbContext();
            List<User> data = db.Users.Where(u => u.Id > 0).ToList();
            //var total = data.Count;
           // var rows = data.Skip(queryParams.offset).Take(queryParams.limit).ToList();
            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}