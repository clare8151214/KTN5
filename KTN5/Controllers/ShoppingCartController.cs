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
        public ActionResult Index()
        {
            return View();
        }
    }
}