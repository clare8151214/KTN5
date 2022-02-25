using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class FundController : Controller
    {
        dbktnEntities db = new dbktnEntities();
        // GET: Fund

        public ActionResult Index()
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if(user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.role = user.role;
            }
            List<FundsView> funds = new List<FundsView>(); 
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(keyword))
            {
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

                             }).ToList();

                foreach (var item in query)
                {
                    funds.Add(new FundsView()
                    {
                        fId = item.fId,
                        fName = item.fName,
                        cName = item.cName,
                        accMoney = item.accMoney,
                        //startTime = item.startTime,
                        //endTime = item.endTime,
                        countDown = (item.endTime - item.startTime),
                        progress = (item.accMoney / item.targetMoney),
                        fPhoto = item.fPhoto,
                    });
                }
            }              
            else
            {
                var query = (from f in db.Fund
                             join c in db.Charity_Member on f.cId equals c.cId
                             where f.trueName.Contains(keyword)
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

                             }).ToList();

                foreach (var item in query)
                {
                    funds.Add(new FundsView()
                    {
                        fId = item.fId,
                        fName = item.fName,
                        cName = item.cName,
                        accMoney = item.accMoney,
                        //startTime = item.startTime,
                        //endTime = item.endTime,
                        countDown = (item.endTime - item.startTime),
                        progress =  (item.accMoney / item.targetMoney),
                        fPhoto = item.fPhoto,
                    });
                }
            }
            return View(funds);
        }

        [Authorize]
        public ActionResult Create()
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.role = user.role;
            }
            if(user.role == "公益單位" || user.role == "管理員")
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Fund newFund,HttpPostedFileBase photo)
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.role = user.role;
            }
            if (user.role == "公益單位" || user.role == "管理員")
            {
                var charity = db.Charity_Member.Where(m => m.uId == user.uId).FirstOrDefault();
                newFund.cId = charity.cId;
                newFund.accMoney = 0;
                if (photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    photo.SaveAs(Server.MapPath("~/FundPhotos/" + photoName));
                    newFund.fPhoto = photoName;
                }
                db.Fund.Add(newFund);
                db.SaveChanges();                
                // return View();
            }
            return RedirectToAction("Index");
        }

        public ActionResult FundDetails(int id)
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.role = user.role;
            }
            
            var query = (from f in db.Fund
                         join c in db.Charity_Member on f.cId equals c.cId
                         where f.fId == id
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
                             fText = f.fText,

                         }).FirstOrDefault();
                FundDetailsView fund = new FundDetailsView()
                {
                    fId = query.fId,
                    fName = query.fName,
                    cName = query.cName,
                    accMoney = query.accMoney,
                    startTime = query.startTime,
                    endTime = query.endTime,
                    countDown = (query.endTime - query.startTime),
                    progress = (query.accMoney / query.targetMoney),
                    fPhoto = query.fPhoto,
                };
            
            return View(fund);
        }

        //public IEnumerable<Solution> GetSolutions(int id)
        //{
        //    var solutions = db.Solution.Where(m => m.fId == id).ToList();           
        //    return solutions;
        //}
    }
}