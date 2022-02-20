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
            if(result.role == "公益單位")
            {
                var charity = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();
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
                var cm = db.Charity_Member.Where(m => m.cId == result.cId).FirstOrDefault();//抓該使用者的公益單位
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

        [Authorize]
        public ActionResult RaiseFunds() //提出募資
        {
            return View();
        }

    }
}