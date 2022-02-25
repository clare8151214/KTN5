using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KTN5.Models;

namespace KTN5.Controllers
{
    public class ShoppingCartController : Controller
    {
        dbktnEntities db = new dbktnEntities();
        // GET: ShoppingCart
        //[NonAction]
        //public int uid() //取出當前使用者的id
        //{
        //    string user = User.Identity.Name;
        //    var result = db.User.Where(m => m.account == user).FirstOrDefault();
        //    return result.uId;
        //}

        [Authorize]
        public ActionResult Index()
        {
            string user = User.Identity.Name;
            var result = db.User.Where(m => m.account == user).FirstOrDefault();
            ViewBag.photo = result.photo;
            var shoppingCar = db.ShoppingCart.Where(m => m.uId == result.uId).ToList();
            return View(shoppingCar);
        }


        [Authorize]
        public ActionResult AddObjectToShoppingCart(int oId)
        {
            string user = User.Identity.Name;
            var result = db.User.Where(m => m.account == user).FirstOrDefault();
            ViewBag.photo = result.photo;
            var shoppingCar = db.ShoppingCart.Where(m => m.uId == result.uId && m.oId == oId).FirstOrDefault();
            if(shoppingCar != null)
            {
                shoppingCar.oQty += 1;
            }
            else
            {
                var obj = db.Object.Where(m => m.oId == oId).FirstOrDefault();
                ShoppingCart newCart = new ShoppingCart();
                newCart.uId = result.uId;               
                newCart.oId = oId;
                newCart.oName = obj.oName;
                newCart.oIntro = obj.oIntro;
                newCart.oQty = 1;
                db.ShoppingCart.Add(newCart);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ShoppingCartDelete(int cartId)
        {
            var shoppingCar = db.ShoppingCart.Where(m => m.cartId == cartId).FirstOrDefault();
            db.ShoppingCart.Remove(shoppingCar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ShoppingCartAddQty(int cartId)
        {
            var shoppingCar = db.ShoppingCart.Where(m => m.cartId == cartId).FirstOrDefault();
            shoppingCar.oQty += 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ShoppingCartSubQty(int cartId)
        {
            var shoppingCar = db.ShoppingCart.Where(m => m.cartId == cartId).FirstOrDefault();
            shoppingCar.oQty -= 1;
            if(shoppingCar.oQty == 0)
            {
                db.ShoppingCart.Remove(shoppingCar);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}