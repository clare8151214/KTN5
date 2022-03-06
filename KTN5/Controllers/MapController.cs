using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class MapController : Controller
    {
        dbktnEntities db = new dbktnEntities();
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
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

            }
            ViewBag.lat = 22.628026;
            ViewBag.lng = 120.293009;
            IEnumerable<SelectListItem> objMemberSelectListItem =
                (from p in db.BloodMap
                 where p.bName != null
                 select p).ToList()
                 .Select(p => new SelectListItem { Value = p.bId.ToString(), Text = p.bName });
            ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");

            var selected = from p in db.BloodMap select p;
            return View(selected);

        }
        [HttpPost]
        public ActionResult List(TimeSpan? txtKeyword)
        {
            IEnumerable<SelectListItem> objMemberSelectListItem =
                (from p in db.BloodMap
                 where p.bName != null
                 select p).ToList()
                 .Select(p => new SelectListItem { Value = p.bId.ToString(), Text = p.bName });
            ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");

            IEnumerable<BloodMap> themap =
                (from p in db.BloodMap
                 select p).ToList();

            if (txtKeyword == null)
            {
                themap = from p in db.BloodMap select p;
                return View(themap);
            }
            else
            {
                themap = from p in db.BloodMap
                         where p.startTime <=  txtKeyword
                         && p.endTime >= txtKeyword
                         select p;
                return View(themap);
            }
        }
        public ActionResult Detail(int id)
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

            }
            BloodMap mapEdit = db.BloodMap.FirstOrDefault(p => p.bId == id);
            ViewBag.lat = mapEdit.lat;
            ViewBag.lng = mapEdit.lng;
            if (mapEdit == null)
                return RedirectToAction("List");
            return View(mapEdit);
        }
        
        public ActionResult queryByDate(TimeSpan dt)
        {

            BloodMap betweendate = db.BloodMap.FirstOrDefault(p => p.startTime <= dt);
            return View(betweendate);
        }
        public JsonResult GetAllLocation()
        {
            var data = db.BloodMap.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLocation(TimeSpan? txtKeyword)
        {
            var data = (from p in db.BloodMap select p).ToList();
            if (txtKeyword == null)
            {
                data = (from p in db.BloodMap select p).ToList();                
            }
            else
            {
                data = (from p in db.BloodMap
                        where p.startTime <= txtKeyword
                        && p.endTime >= txtKeyword
                        select p).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLocation(string country)
        {
            var data = db.BloodMap.ToList();
            if(country == "all")
            {
                data = db.BloodMap.ToList();
            }
            else
            {
                data = db.BloodMap.Where(m => m.bAddress.Contains(country)).ToList();
            }          
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}