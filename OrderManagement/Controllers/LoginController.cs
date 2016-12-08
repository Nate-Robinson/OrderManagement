using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrderManagement.Common;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult StartLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartLogin(FormCollection form, string returnUrl)
        {
            if (!string.IsNullOrEmpty(form["user"]))
            {
                //FormsAuthentication.SetAuthCookie(uname,true);
                string useraccount = form["user"];
                string password = form["password"];
                OrderManageDbContext db = new OrderManageDbContext();
               // List<User> data = db.Users.Where(u => u.UserName == form["user"]).ToList();

                bool userValid = db.Users.Any(user => user.Account == useraccount && user.PassWord == password);

                //if (data.Count == 1)
                //{
                //    string RoleInfo = data[0].UserLevel == 1 ? "管理员": "普通用户"; 
                //    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                //    (
                //        1,                               // 版本号？？
                //        form["user"],                    // 存储用户名
                //        DateTime.Now,                    // 持续开始时间
                //        DateTime.Now.AddMinutes(20),     // 持续结束时间
                //        true,                            // 是否持久的
                //        RoleInfo,                        // 登录用户信息，如：权限等级
                //        "/"                              // 保存cookie的路径
                //    );
                //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                //    cookie.HttpOnly = true;
                //    HttpContext.Response.Cookies.Add(cookie);
                //    Response.Redirect(Request["ReturnUrl"]); // 重定向到用户申请的初始页面
                //}
                if (userValid)
                {
                    FormsAuthentication.SetAuthCookie(useraccount, false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }



            }
            return View();
            //if (form["user"].IsNullOrEmpty() || form["password"].IsNullOrEmpty())
            //{
            //    //Response.Redirect("~");
            //}
            //if (form["user"] == null || form["password"] == null)
            //{
               
            //}
            //else
            //{
            //    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            //        1,
            //        form[0],
            //        DateTime.Now,
            //        DateTime.Now.AddMinutes(30),
            //        false,
            //        "admins"
            //        );

            //    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            //    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
            //    Response.Redirect("~/home/index");

            //}

        }
	}
}