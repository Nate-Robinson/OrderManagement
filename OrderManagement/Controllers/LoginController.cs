using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OrderManagement.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public void Login(FormCollection form)
        {
            //object obj = SqlHelper.ExecuteScalar("select UserId from CDBUsers where UserName=@uname and Password=@pwd",
            //     new SqlParameter("@uname", collection[0]),
            //     new SqlParameter("@pwd", Weibo.Models.Myencrypt.myencrypt(collection[1])));


            //if (obj != null)
            //{

            //}
            if (form["user"] == null || form["password"] == null)
            {
                Response.Redirect("~");
            }
            else
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    form[0],
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    "admins"
                    );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                Response.Redirect("~/home/index");

            }

        }
	}
}