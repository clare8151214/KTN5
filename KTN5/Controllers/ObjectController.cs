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
        dbktnEntities db = new dbktnEntities();
        [NonAction]
        bool isAdmin()
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (result.role == "管理員")
                return true;
            return false;
        }
        // GET: Object
        public ActionResult Index(string otype)
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (result != null)
            {
                ViewBag.photo = result.photo;
                ViewBag.Role = result.role;
            }

            List<ObjectIndexView> objects = new List<ObjectIndexView>();
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(otype))
            {
                var query = (from o in db.Object
                             join c in db.Charity_Member on o.cId equals c.cId
                             select new
                             {
                                 oId = o.oId,
                                 oName = o.oName,
                                 c_name = c.c_name,
                                 oPhoto = o.oPhoto
                             }).ToList();
                foreach(var item in query)
                {
                    objects.Add(new ObjectIndexView()
                    {
                        oId = item.oId,
                        oName = item.oName,
                        c_name = item.c_name,
                        oPhoto = item.oPhoto
                    });
                }
            }
            else
            {
                var query = (from o in db.Object
                             join c in db.Charity_Member on o.cId equals c.cId
                             where o.type.Contains(otype)
                             select new
                             {
                                 oId = o.oId,
                                 oName = o.oName,
                                 c_name = c.c_name,
                                 oPhoto = o.oPhoto
                             }).ToList();
                foreach (var item in query)
                {
                    objects.Add(new ObjectIndexView()
                    {
                        oId = item.oId,
                        oName = item.oName,
                        c_name = item.c_name,
                        oPhoto = item.oPhoto
                    });
                }
            }

            return View(objects);
        }

        

        [Authorize]
        public ActionResult List()
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            ViewBag.photo = result.photo;
            ViewBag.Role = result.role;
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
            else if (result.role == "公益單位")
            {
                var charity = db.Charity_Member.Where(m => m.uId == result.uId).FirstOrDefault();
                IEnumerable<Object> objects = null;
                string keyword = Request.Form["txtKeyword"];
                if (string.IsNullOrEmpty(keyword))
                {
                    objects = db.Object.Where(m => m.cId == charity.cId);                   
                }
                else
                {
                    objects = db.Object.Where(m => m.cId == charity.cId && m.oName.Contains(keyword));
                }
                return View(objects);
            }

            return RedirectToAction("Index");
            
        }

        public ActionResult ObjectDetail(int oId)
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (result != null)
            {
                ViewBag.photo = result.photo;
                ViewBag.Role = result.role;
            }
            ObjectIndexView objects = new ObjectIndexView();

            var query = (from o in db.Object
                         join c in db.Charity_Member on o.cId equals c.cId
                         where o.oId == oId
                         select new
                         {
                             oId = o.oId,
                             oName = o.oName,
                             c_name = c.c_name,
                             oPhoto = o.oPhoto,
                             oIntro = o.oIntro,
                             oNumber = o.oNumber,
                             type = o.type
                         }).FirstOrDefault();
            objects.oId = query.oId;
            objects.oName = query.oName;
            objects.c_name = query.c_name;
            objects.oPhoto = query.oPhoto;
            objects.oIntro = query.oIntro;
            objects.oNumber = query.oNumber;
            objects.type = query.type;
            return View(objects);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (isAdmin() || result.role == "公益單位")
            {
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
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            ViewBag.photo = result.photo;
            ViewBag.Role = result.role;
            if (isAdmin() || result.role == "公益單位")
            {             
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
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            ViewBag.photo = result.photo;
            ViewBag.Role = result.role;
            if (isAdmin() || result.role == "公益單位")
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

            string uid = User.Identity.Name; //取得當前帳號
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            ViewBag.photo = result.photo;
            if (result.role == "公益單位")
            {
                var charity = db.Charity_Member.Where(m => m.uId == result.uId).FirstOrDefault();
                o.cId = charity.cId;
            }           
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