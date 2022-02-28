using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class HomeController : Controller
    {
        dbktnEntities db = new dbktnEntities();

        // GET: Home     
        public ActionResult Index()
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            
            if (user != null)
            {
                var getCart = db.ShoppingCart.Where(m => m.uId == user.uId).ToList();
                int? quantity = 0;
                foreach (var item in getCart)
                {
                    quantity += item.oQty;
                }
                Session["Cart"] = quantity;
                Session.Timeout = 50;
                ViewBag.Name = user.name;
                ViewBag.Role = user.role;
                ViewBag.photo = user.photo;
                Session["role"] = user.role;
            }
            return View();
        }
    }
}