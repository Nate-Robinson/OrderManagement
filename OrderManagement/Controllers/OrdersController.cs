﻿using System;
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
        private OrderManagementContext db = new OrderManagementContext();

        // GET: api/Orders
        //public List<Order> GetOrders()
        //{
        //    var orders = from o in db.Orders
        //                 //where o
        //                 select o;

        //    return db.Orders.ToList();
        //}

        public HttpResponseMessage GetOrders(queryParam user)
        {
            //var da = from u in db.Orders
            //         select u;
            //List<Order> data = da.ToList();
            //var tempTotal = data.Count;
            //var tempRows = data.Skip(user.offset).Take(user.limit).ToList();
            //ResultData result =  new ResultData()
            //{ 
            //    total = tempTotal,
            //    rows = tempRows 
            //};

            OrderManagementContext db = new OrderManagementContext();
            List<Order> data = db.Orders.Where(u => u.Id > 0).ToList();

            var tempTotal = data.Count;
            var tempRows = data.ToList();
            ResultData result = new ResultData()
            {
                total = tempTotal,
                rows = tempRows
            };
            return Utity.toJson(result);
        }

        public class ResultData
        {
            public int total { get; set; }
            public List<Order> rows { get; set; }
        }

        public class queryParam
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
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

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}