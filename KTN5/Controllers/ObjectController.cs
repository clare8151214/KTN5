﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;
using Object = KTN5.Models.Object;

namespace KTN5.Controllers
{
    public class ObjectController : Controller
    {
        dbktnEntities db = new dbktnEntities();
        [NonAction]
        bool isAdmin()
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (result.role == "管理員" || result.role == "公益單位")
                return true;
            return false;
        }
        // GET: Object
        public ActionResult Index()
        {
            IEnumerable<Object> objects = null;
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(keyword))
                objects = from o in (new dbktnEntities()).Object
                          select o;
            else
                objects = from o in (new dbktnEntities()).Object
                          where o.oName.Contains(keyword)
                          select o;
            return View(objects);
        }

        [Authorize]
        public ActionResult List()
        {
            if (isAdmin())
            {
                IEnumerable<Object> objects = null;
                string keyword = Request.Form["txtKeyword"];
                if (string.IsNullOrEmpty(keyword))
                    objects = from o in (new dbktnEntities()).Object
                              select o;
                else
                    objects = from o in (new dbktnEntities()).Object
                              where o.oName.Contains(keyword)
                              select o;
                return View(objects);
            }
            return RedirectToAction("Index");
            
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (isAdmin())
            {
                dbktnEntities db = new dbktnEntities();
                Object obj = db.Object.FirstOrDefault(o => o.oId == id);
                if (obj != null)
                {
                    db.Object.Remove(obj);
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (isAdmin())
            {
                dbktnEntities db = new dbktnEntities();
                Object obj = db.Object.FirstOrDefault(o => o.oId == id);
                if (obj == null)
                    return RedirectToAction("List");
                return View(obj);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Object o)
        {
            dbktnEntities db = new dbktnEntities();
            Object obj = db.Object.FirstOrDefault(t => t.oId == o.oId);
            if (obj != null)
            {
                obj.oName = o.oName;
                if (o.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    o.photo.SaveAs(Server.MapPath("~/Content/oPhoto/" + photoName));
                    obj.oPhoto = photoName;
                }
                obj.oNumber = o.oNumber;
                obj.oIntro = o.oIntro;
                obj.type = o.type;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        [Authorize]
        public ActionResult Create()
        {
            if (isAdmin())
            {
                return View();
            }
            return RedirectToAction("Index");   
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Object o, HttpPostedFileBase photo)
        {
            dbktnEntities db = new dbktnEntities();
            db.Object.Add(o);
            db.SaveChanges();
            Object obj = db.Object.FirstOrDefault(c => c.oId == o.oId);
            if (obj != null)
            {
                obj.oName = o.oName;
                if (o.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    o.photo.SaveAs(Server.MapPath("~/Content/oPhoto/" + photoName));
                    obj.oPhoto = photoName;
                }
                obj.oNumber = o.oNumber;
                obj.oIntro = o.oIntro;
                obj.type = o.type;
                db.SaveChanges();
            }
            //string picName = null;
            //var folderPath = HttpContext.Server.MapPath("~/Content/images/oPhoto/"); // 存放路徑
            //if (!System.IO.Directory.Exists(folderPath)) // 路徑不存在先新增一個
            //{
            //    System.IO.Directory.CreateDirectory(folderPath);
            //}
            //if (photo != null && photo.ContentLength > 0)
            //{
            //    string ext = System.IO.Path.GetExtension(photo.FileName); // 取得副檔名
            //    picName = "img_" + o.oId + ext; // 重新命名圖片
            //    var path = System.IO.Path.Combine(folderPath, picName);
            //    photo.SaveAs(path);
            //}
            return RedirectToAction("List");
        }
    }
}