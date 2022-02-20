using System;
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

        public ActionResult List()
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
        public ActionResult Delete(int id)
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
        public ActionResult Edit(int id)
        {
            dbktnEntities db = new dbktnEntities();
            Object obj = db.Object.FirstOrDefault(o => o.oId == id);
            if (obj == null)
                return RedirectToAction("List");
            return View(obj);
        }
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
        public ActionResult Create()
        {
            return View();
        }
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