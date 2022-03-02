using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;


namespace KTN5.Controllers
{
    public class CharityController : Controller
    {
        dbktnEntities db = new dbktnEntities();
        // GET: Charity
        [Authorize]
        public ActionResult Index() //公益單位畫面
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            ViewBag.photo = result.photo;
            ViewBag.Role = result.role;
            if (result.role == "公益單位")
            {
                var charity = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();
                return View(charity);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [Authorize]
        public ActionResult Edit() //編輯公益單位畫面
        {
            string uid = User.Identity.Name;
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();
            ViewBag.photo = result.photo;
            ViewBag.Role = result.role;
            if (result.role == "公益單位")
            {
                var charity = db.Charity_Member.Where(m => m.uId == result.uId).FirstOrDefault();
                return View(charity);
            }
            return RedirectToAction("Index","Home");
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Charity_Member charity , HttpPostedFileBase photo) //編輯公益單位
        {
            string uid = User.Identity.Name; 
            var result = db.User.Where(m => m.account == uid).FirstOrDefault();//抓該使用者
            if (result.role == "公益單位")
            {
                var cm = db.Charity_Member.Where(m => m.uId == result.uId).FirstOrDefault();//抓該使用者的公益單位
                //cm.c_name = charity.c_name; //單位名不能更改
                cm.c_phone = charity.c_phone;
                cm.c_pname = charity.c_pname;
                cm.c_address = charity.c_address;
                cm.heart_code = charity.heart_code;
                if (photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UserPhotos"), photoName);
                    photo.SaveAs(path);
                    cm.photo = photoName;
                }
                db.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
        }

        //[Authorize]
        //public ActionResult ObjectManage()
        //{
        //    string uid = User.Identity.Name;
        //    var result = db.User.Where(m => m.account == uid).FirstOrDefault();
        //    if (result.role == "公益單位")
        //    {
        //        var charity = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();
        //        IEnumerable<Models.Object> objects = null;
        //        string keyword = Request.Form["txtKeyword"];
        //        if (string.IsNullOrEmpty(keyword))
        //            objects = from o in (new dbktnEntities()).Object
        //                      select o;
        //        else
        //            objects = from o in (new dbktnEntities()).Object
        //                      where o.oName.Contains(keyword)
        //                      select o;
        //        return View(objects);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //[Authorize]
        //public ActionResult ObjectCreate() //提出物資需求
        //{
        //    string uid = User.Identity.Name;
        //    var result = db.User.Where(m => m.account == uid).FirstOrDefault();
        //    if (result.role == "公益單位")
        //    {
        //        //var charity = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();
        //        return View();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ObjectCreate(Models.Object obj) //提出物資需求
        //{
        //    string uid = User.Identity.Name;
        //    var result = db.User.Where(m => m.account == uid).FirstOrDefault();
        //    if (result.role == "公益單位")
        //    {
        //        var charity = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();
        //        obj.cId = charity.cId;
        //        db.Object.Add(obj);
        //        return View();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //[Authorize]
        //public ActionResult ObjectEdit() //修改物資需求
        //{
        //    string uid = User.Identity.Name;
        //    var result = db.User.Where(m => m.account == uid).FirstOrDefault();
        //    if (result.role == "公益單位")
        //    {
        //        var charity = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();
        //        var object1 = db.Object.Where(m => m.cId == charity.cId).FirstOrDefault();

        //        return View(object1);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

    }
}