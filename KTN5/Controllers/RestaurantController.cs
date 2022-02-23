using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];
            string area = Request.Form["txtArea"];
            ViewBag.ff = area;
            ViewBag.kk = keyword;
            List<Restaurant> list = null;
            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(area))
                list = (new RestaurantFactory()).queryAll();
            else
            {
                list = (new RestaurantFactory()).queryByKeyword(keyword, area);
                if (list == null)
                    ViewBag.ms = "此地區無愛心待用餐廳";
            }
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Restaurant r)
        {
            (new RestaurantFactory()).create(r);
            return RedirectToAction("List");
        }

        internal List<Restaurant> queryByKeyword(string keyword)
        {
            string sql = "SELECT*FROM Restaurant WHERE rName LIKE '%" + keyword + "%'";
            sql += "OR rPhone LIKE '%" + keyword + "%'";
            sql += "OR rAddress LIKE '%" + keyword + "%'";
            sql += "OR startTime LIKE '%" + keyword + "%'";
            sql += "OR endTime LIKE '%" + keyword + "%'";
            return (new RestaurantFactory()).queryBySql(sql);
        }

        public ActionResult Edit(int? id)
        {
            Restaurant x = null;
            if (id != null)
                x = (new RestaurantFactory()).queryByFid((int)id);
            return View(x);
        }
        [HttpPost]
        public ActionResult Edit(Restaurant x)
        {
            (new RestaurantFactory()).update(x);
            return RedirectToAction("List");
        }


        public ActionResult Delete(int? id)
        {
            if (id != null)
                (new RestaurantFactory()).delete((int)id);
            return RedirectToAction("List");
        }

        public ActionResult Save()
        {
            Restaurant x = new Restaurant();
            x.rName = Request.Form["rName"];
            x.rPhone = Request.Form["rPhone"];
            x.rAddress = Request.Form["rAddress"];
            x.startTime = TimeSpan.Parse(Request.Form["startTime"]);
            x.endTime = TimeSpan.Parse(Request.Form["endTime"]);

            (new RestaurantFactory()).create(x);
            return RedirectToAction("List");
        }
    }
}