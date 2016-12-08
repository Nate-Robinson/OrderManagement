using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OrderManagement.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.Web;
using OrderManagement.Common;

namespace OrderManagement.Controllers
{
    public static class Utity
    {
        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }

    public class OrdersController : ApiController
    {
        private OrderManageDbContext db = new OrderManageDbContext();

        private const float ShoePrice = 199;


        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 订单列表获取数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public HttpResponseMessage GetOrders()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return Utity.toJson(userHostAddress);
            }
            return Utity.toJson("");

            //OrderManageDbContext db = new OrderManageDbContext();
            //List<Order> data = db.Orders.Where(u => u.Id > 0).ToList();

            //var tempTotal = data.Count;

            //// 分页查询
            //var tempRows = data.Skip(offset).Take(limit).ToList();
            //ResultData result = new ResultData()
            //{
            //    total = tempTotal,
            //    rows = tempRows
            //};
            //return Utity.toJson(result);
        }

        public class ResultData
        {
            public int total { get; set; }
            public List<Order> rows { get; set; }
        }

        public class QueryParam
        {
            public int limit { get; set; }
            public int offset { get; set; }
            public string departmentname { get; set; }
            public string status { get; set; }
            public string sortName { get; set; }
            public string sortOrder { get; set; }
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult PutOrder(/*string id,*/ Order order)
        {
            if (order.Id.IsNullOrEmpty() || order.Status.IsNullOrEmpty())
            {
                return BadRequest();
            }

            Order dbOrder = db.Orders.Find(order.Id);
            DbEntityEntry entry = db.Entry<Order>(dbOrder);
            entry.State = EntityState.Modified;            
            int effect  = db.SaveChanges();            
            if (effect<1)
            {
                return InternalServerError();
            }
            
            return Ok(order);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public HttpResponseMessage PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                var a= ModelState.Values.SelectMany(v => v.Errors).ToList();
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            bool succeed = false;
            if (OrderExists(order.Id))
            {
                succeed = UpdateOrder(order);
            }
            else
            {
                succeed = AddOrder(order);
            }

            if (succeed)
            {
                HttpResponseMessage response = Utity.toJson(order);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "POST");
                response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with,content-type");
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        private bool UpdateOrder(Order order)
        {
            if (order.Id.IsNullOrEmpty())
            {
                return false;
            }

            Order dbOrder = db.Orders.Where(item => item.Id == order.Id).FirstOrDefault();
            if (dbOrder == null)
            {
                return false;
            }

            #region 更新传入order的不为空的字段
            if (!order.Address.IsNullOrEmpty())
            {
                dbOrder.Address = order.Address;
            }

            if (!order.CellPhone.IsNullOrEmpty())
            {
                dbOrder.CellPhone = order.CellPhone;
            }
            if (!order.City.IsNullOrEmpty())
            {
                dbOrder.City = order.City;
            }
            if (!order.Color.IsNullOrEmpty())
            {
                dbOrder.Color = order.Color;
            }
            if (order.CreateTime != null
                &&order.CreateTime == DateTime.MinValue 
                &&order.CreateTime == DateTime.MaxValue)
            {
                dbOrder.CreateTime = order.CreateTime;
            }
            if (!order.CustomerIP.IsNullOrEmpty())
            {
                dbOrder.CustomerIP = order.CustomerIP;
            }
            if (!order.District.IsNullOrEmpty())
            {
                dbOrder.District = order.District;
            }
            if (!order.LogisticsCode.IsNullOrEmpty())
            {
                dbOrder.LogisticsCode = order.LogisticsCode;
            }
            if (!order.LogisticsCompany.IsNullOrEmpty())
            {
                dbOrder.LogisticsCompany = order.LogisticsCompany;
            }
            if (!order.Message.IsNullOrEmpty())
            {
                dbOrder.Message = order.Message;
            }
            if (!order.Name.IsNullOrEmpty())
            {
                dbOrder.Name = order.Name;
            }
            if (order.Price != null)
            {
                dbOrder.Price = order.Price;
            }
            if (!order.Province.IsNullOrEmpty())
            {
                dbOrder.Province = order.Province;
            }
            if (order.Qty >0)
            {
                dbOrder.Qty = order.Qty;
            }
            if (!order.Remark.IsNullOrEmpty())
            {
                dbOrder.Remark = order.Remark;
            }
            if (order.ShoeSize >0)
            {
                dbOrder.ShoeSize = order.ShoeSize;
            }
            if (!order.Status.IsNullOrEmpty())
            {
                dbOrder.Status = order.Status;
            }
            if (order.TotalMoney >=0)
            {
                dbOrder.TotalMoney = order.TotalMoney;
            }

            #endregion

            DbEntityEntry entry = db.Entry<Order>(dbOrder);
            entry.State = EntityState.Modified;
            int effectRows = db.SaveChanges();
            if (effectRows < 1)
            {
                return false;
            }
            return true;
        }

        private bool AddOrder(Order order)
        {
            order.Id = db.GenerateOrderId().FirstOrDefault();
            order.Address = order.Province + order.City + order.District + order.Address;
            order.CustomerIP = GetIP4Address();
            order.Price = ShoePrice;
            order.Status = ((int)OrderStatus.WaitForConfirm).ToString();
            order.CreateTime = DateTime.Now;
            db.Orders.Add(order);
            int effectRows = db.SaveChanges();
            if (effectRows < 1)
            {
                return false;
            }
            return true;
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(string id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(string id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }

        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

    }
}