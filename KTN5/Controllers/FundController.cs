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
                ViewBag.Role = user.role;
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
                        accMoney = Convert.ToInt32 (item.accMoney),
                        //startTime = item.startTime,
                        //endTime = item.endTime,
                        countDown = (item.endTime - item.startTime),
                        progress = (item.accMoney / item.targetMoney) * 100,
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
                        accMoney = Convert.ToInt32(item.accMoney),
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
                ViewBag.Role = user.role;
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
                ViewBag.Role = user.role;
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

        [Authorize]
        public ActionResult CreateSolution()
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.Role = user.role;
            }
            if (user.role == "公益單位" || user.role == "管理員")
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateSolution(Solution newSol, HttpPostedFileBase photo)
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.Role = user.role;
            }
            if (user.role == "公益單位" || user.role == "管理員")
            {
                var charity = db.Charity_Member.Where(m => m.uId == user.uId).FirstOrDefault();
                var fund = db.Fund.Where(m => m.cId == charity.cId);
               
                if (photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    photo.SaveAs(Server.MapPath("~/FundPhotos/" + photoName));
                    newSol.sPhoto = photoName;
                }
                db.Solution.Add(newSol);
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
                ViewBag.Role = user.role;
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
                    targetMoney = query.targetMoney,
                    accMoney = query.accMoney,
                    startTime = query.startTime,
                    endTime = query.endTime,
                    countDown = (query.endTime - query.startTime),
                    progress = (query.accMoney / query.targetMoney) * 100,
                    fPhoto = query.fPhoto,
                };
            
            return View(fund);
        }

        [Authorize]
        public ActionResult Sponsor(int id)
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.Role = user.role;
            }

            var query = (from f in db.Fund
                         join c in db.Charity_Member on f.cId equals c.cId
                         join s in db.Solution on f.fId equals s.fId
                         where s.sId == id
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
                             sId = s.sId,
                             sMoney = s.sMoney,
                             intro = s.intro,
                             sName = s.sName,
                             sPhoto = s.sPhoto,
                         }).FirstOrDefault();

            SponsorView sponsor = new SponsorView()
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
                sId = query.sId,
                sMoney = query.sMoney,
                intro = query.intro,
                sName = query.sName,
                sPhoto = query.sPhoto,
                targetMoney = query.targetMoney,
            };

            return View(sponsor);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Sponsor(Sponsor sponsor)
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.Role = user.role;
            }
            sponsor.uId = user.uId;
            db.Sponsor.Add(sponsor);
            db.SaveChanges();
            

            return RedirectToAction("PaySuccess");
        }
        [Authorize]
        public ActionResult PaySuccess()
        {
            string uid = User.Identity.Name;
            var user = db.User.Where(m => m.account == uid).FirstOrDefault();
            if (user != null)
            {
                ViewBag.photo = user.photo;
                ViewBag.Role = user.role;
            }
            return View();
        }


    }
}