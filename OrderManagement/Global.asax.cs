using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Security;
using System.Security.Principal;
using OrderManagement.Models;

namespace OrderManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //    var app = sender as HttpApplication;

        //    if (app.Context.User != null)
        //    {
        //        var user = app.Context.User;
        //        var identity = user.Identity as FormsIdentity;

        //        // We could explicitly construct an Principal object with roles info using System.Security.Principal.GenericPrincipal
        //        var principalWithRoles = new GenericPrincipal(identity, identity.Ticket.UserData.Split(','));

        //        // Replace the user object
        //        app.Context.User = principalWithRoles;

        //    }
        //}

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;


                        using (OrderManageDbContext db = new OrderManageDbContext())
                        {
                            User user = db.Users.SingleOrDefault(u => u.UserName == username);
                            roles = user.UserLevel == 1? "管理员" : "普通用户";
                        }
                        //using (userDbEntities entities = new userDbEntities())
                        //{
                        //    User user = entities.Users.SingleOrDefault(u => u.username == username);

                        //    roles = user.Roles;
                        //}
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }


       // protected void Application_AuthorizeRequest(object sender, System.EventArgs e)
        //{

         //   var id = Context.User.Identity as FormsIdentity;
         //   if (id != null && id.IsAuthenticated)
         //   {
         //       var roles = id.Ticket.UserData.Split(',');
         //       Context.User = new GenericPrincipal(id, roles);
         //   }

            //HttpApplication App = (HttpApplication)senderDefault1;
            //HttpContext Ctx = App.Context; //获取本次Http请求相关的HttpContext对象

            //if (Ctx.Request.IsAuthenticated == false) //验证过的用户才进行role的处理
            //{
            //    FormsIdentity Id = (FormsIdentity)Ctx.User.Identity;
            //    FormsAuthenticationTicket Ticket = Id.Ticket; //取得身份验证票
            //    string[] Roles = Ticket.UserData.Split(','); //将身份验证票中的role数据转成字符串数组
            //    Ctx.User = new GenericPrincipal(Id, Roles); //将原有的Identity加上角色信息新建一个GenericPrincipal表示当前用户,这样当前用户就拥有了role信息
            //}
      //  }

        //public MvcApplication()
        //{
        //    this.AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        //}

        //void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        //{
        //    var id = Context.User.Identity as FormsIdentity;
        //    if (id != null && id.IsAuthenticated)
        //    {
        //        var roles = id.Ticket.UserData.Split(',');
        //        Context.User = new GenericPrincipal(id, roles);
        //    }
        //} 



        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
