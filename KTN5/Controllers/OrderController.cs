using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class OrderController : Controller
    {
        dbktnEntities db = new dbktnEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List()
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            var order = db.Order.Where(m => m.uId == result.uId).OrderByDescending(m => m.orderId).ToList();            
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Order(string hName, string hPhone, string hEmail , string hCarrier)
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            Order order = new Order()
            {
                uId = result.uId,
                hName = hName,
                hPhone = hPhone,
                hEamil = hEmail,
                hCarrier = hCarrier,
                created_at = DateTime.Now
            };
            db.Order.Add(order);
            db.SaveChanges();


            int orderid = db.Order.OrderByDescending(m => m.orderId).FirstOrDefault().orderId;
            var shoppingCart = db.ShoppingCart.Where(m => m.uId == result.uId).ToList();
            OrderDetail[] orderDetails = new OrderDetail[shoppingCart.Count];

            for(int i = 0; i < orderDetails.Length; i++)
            {
                orderDetails[i] = new OrderDetail();
                orderDetails[i].orderId = orderid;
                orderDetails[i].oName = shoppingCart[i].oName;
                orderDetails[i].oQty = shoppingCart[i].oQty;
            }
            db.OrderDetail.AddRange(orderDetails);
            db.ShoppingCart.RemoveRange(shoppingCart);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult OrderDetails(int orderid)
        {
            var order = db.OrderDetail.Where(m => m.orderId == orderid).ToList();
            return View(order);
        }


    }
}