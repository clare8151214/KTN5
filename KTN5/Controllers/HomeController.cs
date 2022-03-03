using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;
using Object = KTN5.Models.Object;

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
                
            }
            List<FundsView> funds = new List<FundsView>();
            var query = (from f in db.Fund
                         join c in db.Charity_Member on f.cId equals c.cId
                         select new
                         {
                             fId = f.fId,
                             fName = f.fName,
                             cName = c.c_name,
                             targetMoney = f.targetMoney,
                             accMoney = f.accMoney,
                             startTime = f.startTime,
                             endTime = f.endTime,
                             fPhoto = f.fPhoto,

                         }).Take(6);

            foreach (var item in query)
            {
                funds.Add(new FundsView()
                {
                    fId = item.fId,
                    fName = item.fName,
                    cName = item.cName,
                    accMoney = Convert.ToInt32(item.accMoney),
                    startTime = item.startTime,
                    endTime = item.endTime,
                    countDown = (item.endTime - item.startTime),
                    progress = (item.accMoney / item.targetMoney) * 100,                   
                    fPhoto = item.fPhoto,
                });
            }


            return View(funds);
        }

        public ActionResult Admin()
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();

            if (user != null)
            {
                
                ViewBag.Name = user.name;
                ViewBag.Role = user.role;
                ViewBag.photo = user.photo;

            }
            if(user.role == "管理員")
            {
                CVMMember cvm = new CVMMember()
                {
                    gmembers = db.User.ToList(),
                    charities = db.Charity_Member.ToList(),
                    restaurants = db.Restaurant.ToList(),
                    objects = db.Object.ToList()
                };
                ViewBag.msg = TempData["msg"];
                return View(cvm);
            }
            return RedirectToAction("Index");
        }

        public JsonResult getRestaurants()
        {
            return Json(db.Restaurant.Select(m => new
            {
                m.rId,
                m.rName,
                m.rAddress,
                m.rPhone,
                m.startTime,
                m.endTime
            }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getGenMembers()
        {
            return Json(db.User.Select(m => new
            {
                m.uId,
                m.name,
                m.account,
                m.password,
                m.phone,
                m.address,
                m.role,
                m.created_at
                //created_at = m.created_at.HasValue ? m.created_at.Value.ToString("dd/MM/yyyy"):string.Empty
                //MftDate = item.MftDate == DateTime.MinValue ? "" : item.MftDate.Value.ToString("dd/MM/yyyy"),
                //MftDate = item.MftDate.HasValue ? item.MftDate.Value.ToString("dd/MM HH:mm:ss") :
            }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRestById(int rId)
        {
            return Json(db.Restaurant.Where(m => m.rId == rId).FirstOrDefault());
        }

        //[HttpPost]
        public JsonResult getCharitiesById(int cId)
        {
            return Json(db.Charity_Member.Where(m => m.cId == cId).FirstOrDefault());
        }

        public JsonResult getGenMembersById(int mId)
        {
            return Json(db.User.Where(m => m.uId == mId).FirstOrDefault());
        }

        public JsonResult getRestsByKeyword(string searchOption, string searchText)
        {
            List<Restaurant> restList = new List<Restaurant>();
            switch (searchOption)
            {
                case "餐廳編號":
                    int intsearchText = Convert.ToInt32(searchText);
                    restList = db.Restaurant.Where(m => m.rId == intsearchText).OrderBy(m => m.rId).ToList();
                    break;
                case "名稱":
                    restList = db.Restaurant.Where(m => m.rName.Contains(searchText)).OrderBy(m => m.rId).ToList();
                    break;
                case "地區":
                    restList = db.Restaurant.Where(m => m.rAddress.Contains(searchText)).OrderBy(m => m.rId).ToList();
                    break;
                case "電話":
                    restList = db.Restaurant.Where(m => m.rPhone.Contains(searchText)).OrderBy(m => m.rId).ToList();
                    break;
            }
            return Json(restList);
        }

        public JsonResult getCharitiesByKeyword(string searchOption, string searchText)
        {
            List<Charity_Member> charityList = new List<Charity_Member>();
            switch (searchOption)
            {
                case "cId":
                    int intsearchText = Convert.ToInt32(searchText);
                    charityList = db.Charity_Member.Where(m => m.cId == intsearchText).OrderBy(m => m.cId).ToList();
                    break;
                case "c_name":
                    charityList = db.Charity_Member.Where(m => m.c_name.Contains(searchText)).OrderBy(m => m.cId).ToList();
                    break;
                case "c_address":
                    charityList = db.Charity_Member.Where(m => m.c_address.Contains(searchText)).OrderBy(m => m.cId).ToList();
                    break;
                case "c_phone":
                    charityList = db.Charity_Member.Where(m => m.c_phone.Contains(searchText)).OrderBy(m => m.cId).ToList();
                    break;
                case "heart_code":
                    int intsearchText_code = Convert.ToInt32(searchText);
                    charityList = db.Charity_Member.Where(m => m.heart_code == intsearchText_code).OrderBy(m => m.cId).ToList();
                    break;
            }
            return Json(charityList);
        }

        [HttpPost]
        public JsonResult getMembersByKeyword(string searchOption, string searchText)
        {
            List<User> memberList = new List<User>();

            switch (searchOption)
            {
                case "uId":
                    int intsearchText = Convert.ToInt32(searchText);
                    memberList = db.User.Where(m => m.uId == intsearchText).OrderBy(m => m.uId).ToList();
                    break;
                case "name":
                    memberList = db.User.Where(m => m.name.Contains(searchText)).OrderBy(m => m.uId).ToList();
                    break;
                case "account":
                    memberList = db.User.Where(m => m.account.Contains(searchText)).OrderBy(m => m.uId).ToList();
                    break;
                case "password":
                    memberList = db.User.Where(m => m.password.Contains(searchText)).OrderBy(m => m.uId).ToList();
                    break;
                case "phone":
                    memberList = db.User.Where(m => m.phone.Contains(searchText)).OrderBy(m => m.uId).ToList();
                    break;
                case "address":
                    memberList = db.User.Where(m => m.address.Contains(searchText)).OrderBy(m => m.uId).ToList();
                    break;
            }
            return Json(memberList);
        }

        public JsonResult getCharities()
        {
            return Json(db.Charity_Member.Select(m => new {
                m.cId,
                m.c_name,
                m.c_address,
                m.c_phone,
                m.photo,
                m.heart_code,
                m.created_at
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditMember(User user)
        {
            //var path = UploadImage(avatarImg);
            User result = db.User.Where(m => m.uId == user.uId).FirstOrDefault();
            if (result != null)
            {
                if (user.photoFile != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/images"), photoName);
                    user.photoFile.SaveAs(path);
                    result.photo = photoName;
                }
                result.name = user.name;
                result.account = user.account;
                result.password = user.password;
                result.phone = user.phone;
                result.address = user.address;
                result.role = user.role;
                result.created_at = user.created_at;
                db.SaveChanges();
                TempData["msg"] = "恭喜!資料更新成功!";
            }
            return RedirectToAction("Admin");
        }

        public ActionResult EditCharity(Charity_Member charity)
        {
            Charity_Member result = db.Charity_Member.Where(m => m.cId == charity.cId).FirstOrDefault();

            if (result != null)
            {
                if (charity.logoFile != null)
                {
                    string logoName = charity.c_name + Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/images"), logoName);
                    charity.logoFile.SaveAs(path);
                    result.photo = logoName;  //更改資料庫logo檔名
                }
                result.c_name = charity.c_name;
                result.c_address = charity.c_address;
                result.c_phone = charity.c_phone;
                result.heart_code = charity.heart_code;
                result.created_at = charity.created_at;
                db.SaveChanges();
                TempData["msg"] = "恭喜!資料更新成功!";
            }
            return RedirectToAction("Admin");
        }

        public ActionResult EditRest(Restaurant rest)
        {
            Restaurant result = db.Restaurant.Where(m => m.rId == rest.rId).FirstOrDefault();
            if (result != null)
            {
                result.rName = rest.rName;
                result.rAddress = rest.rAddress;
                result.rPhone = rest.rPhone;
                result.startTime = rest.startTime;
                result.endTime = rest.endTime;
                db.SaveChanges();
                TempData["msg"] = "恭喜!資料更新成功!";
            }
            return RedirectToAction("Admin");
        }

        public ActionResult DeleteMember(int uId)
        {
            User result = db.User.Where(m => m.uId == uId).FirstOrDefault();
            if (result != null)
            {
                db.User.Remove(result);
                db.SaveChanges();
                TempData["msg"] = "資料已成功刪除!";
            }
            return RedirectToAction("Admin");
        }

        public ActionResult DeleteCharity(int cId)
        {
            Charity_Member result = db.Charity_Member.Where(m => m.cId == cId).FirstOrDefault();
            if (result != null)
            {
                db.Charity_Member.Remove(result);
                db.SaveChanges();
                TempData["msg"] = "資料已成功刪除!";
            }
            return RedirectToAction("Admin");
        }

        public ActionResult DeleteRest(int rId)
        {
            Restaurant result = db.Restaurant.Where(m => m.rId == rId).FirstOrDefault();
            if (result != null)
            {
                db.Restaurant.Remove(result);
                db.SaveChanges();
                TempData["msg"] = "資料已成功刪除!";
            }
            return RedirectToAction("Admin");
        }

        public JsonResult getObjById(int oId)
        {
            return Json(db.Object.Where(m => m.oId == oId).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditObj(Object obj)
        {
            Object result = db.Object.Where(m => m.oId == obj.oId).FirstOrDefault();
            if (result != null)
            {
                if (obj.photo != null)
                {
                    string ophotoName = obj.oName + Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/images"), ophotoName);
                    obj.photo.SaveAs(path);
                    result.oPhoto = ophotoName;
                }
                result.oName = obj.oName;
                result.oNumber = obj.oNumber;
                result.oIntro = obj.oIntro;
                db.SaveChanges();
                TempData["msg"] = "恭喜!資料更新成功!";
            }
            return RedirectToAction("Admin");
        }

        public JsonResult getObjects()
        {
            return Json(
                from c in db.Charity_Member
                join o in db.Object on c.cId equals o.cId
                select new { c.c_name, o.oId, o.oName, o.oPhoto, o.oNumber, o.oIntro },
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult getObjectsByKeyword(string searchOption, string searchText)
        {
            //List<Object> objList = new List<Object>();
            switch (searchOption)
            {
                case "oId":
                    int intsearchText = Convert.ToInt32(searchText);
                    var objList = (from c in db.Charity_Member
                                   join o in db.Object on c.cId equals o.cId
                                   where o.oId == intsearchText
                                   select new { c.c_name, o.oId, o.oName, o.oNumber, o.oIntro }).ToList();
                    return Json(objList);
                case "oName":
                    objList = (from c in db.Charity_Member
                               join o in db.Object on c.cId equals o.cId
                               where o.oName.Contains(searchText)
                               select new { c.c_name, o.oId, o.oName, o.oNumber, o.oIntro }).ToList();
                    return Json(objList);
                case "cId":
                    objList = (from c in db.Charity_Member
                               join o in db.Object on c.cId equals o.cId
                               where c.c_name.Contains(searchText)
                               select new { c.c_name, o.oId, o.oName, o.oNumber, o.oIntro }).ToList();
                    return Json(objList);
                case "oNumber":
                    objList = (from c in db.Charity_Member
                               join o in db.Object on c.cId equals o.cId
                               where o.oNumber == searchText
                               select new { c.c_name, o.oId, o.oName, o.oNumber, o.oIntro }).ToList();
                    return Json(objList);
                case "oIntro":
                    objList = (from c in db.Charity_Member
                               join o in db.Object on c.cId equals o.cId
                               where o.oIntro.Contains(searchText)
                               select new { c.c_name, o.oId, o.oName, o.oNumber, o.oIntro }).ToList();
                    return Json(objList);
            }
            return Json(null);
        }

        public ActionResult DeleteObj(int oId)
        {
            Object result = db.Object.Where(m => m.oId == oId).FirstOrDefault();
            if (result != null)
            {
                db.Object.Remove(result);
                db.SaveChanges();
                TempData["msg"] = "資料已成功刪除!";
            }
            return RedirectToAction("Admin");
        }




    }
}