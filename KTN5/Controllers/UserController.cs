using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KTN5.Models;

namespace KTN5.Controllers
{
    public class UserController : Controller
    {
        // Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        // Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(string charityName , [Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string Message = "";
            //
            //Model Validation
            if (ModelState.IsValid)
            {
                #region //驗證帳號是否存在
                var isExist = IsEmailExist(user.account);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "帳號已存在");
                    return View(user);
                }
                #endregion

                #region//產生驗證碼
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region//密碼加密
                user.password = Crypto.Hash(user.ConfirmPassword);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                user.IsEmailVerified = false;
                user.created_at = DateTime.Now;
                #region//存入資料庫
                using (dbktnEntities dc = new dbktnEntities())
                {
                    dc.User.Add(user);
                    
                    if(user.role == "公益單位") //辨識會員類型
                    {
                        string cName = charityName;
                        var isCharityExit = dc.Charity_Member.Where(m => m.c_name == cName).FirstOrDefault(); //檢查是否已有這個公益單位                        
                        if(isCharityExit == null)
                        {
                            Charity_Member charity = new Charity_Member
                            {
                                c_name = cName,
                                c_pname = user.name,
                                created_at = DateTime.Now
                            };
                            dc.Charity_Member.Add(charity);
                        }
                        else
                        {
                            ModelState.AddModelError("CharityExist", "公益單位已被註冊，請改註冊一般會員或聯絡網站管理員");
                            return View(user);
                        }
                    }
                    dc.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(user.account, user.ActivationCode.ToString());
                    Message = "OK" + user.account;
                    Status = true;
                }
                #endregion
            }
            else
            {
                Message = "Invalid Request";
            }

            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }

        

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (dbktnEntities dc = new dbktnEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;
                var v = dc.User.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = true;
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (dbktnEntities dc = new dbktnEntities())
            {
                var v = dc.User.Where(a => a.account == login.account).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.password), v.password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.account, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid";
                    }
                }
                else
                {
                    message = "Invalid";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //AdminLogin
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (dbktnEntities dc = new dbktnEntities())
            {
                var v = dc.User.Where(a => a.account == login.account && a.role == "管理員").FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.password), v.password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.account, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid";
                    }
                }
                else
                {
                    message = "Invalid";
                }
            }
            ViewBag.Message = message;
            return View();
        }


        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [NonAction]
        public bool IsEmailExist(string account)
        {
            using (dbktnEntities db = new dbktnEntities())
            {
                var v = db.User.Where(a => a.account == account).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]//"miniwei1011@gmail.com","Passw0rd-iii"
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("miniwei1011@gmail.com", "WEIWEI");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Passw0rd-iii";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "OK";
                body = "<br/><br/>aa" + "<br/><br/><a href='" + link + "'>" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "reset Password";
                body = "<br/><br/>reset Password" + "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string account)
        {
            //Verify Email ID
            //Generate Reset password link
            //Send Email
            string message = "";
            bool status = false;

            using (dbktnEntities dc = new dbktnEntities())
            {
                var Account = dc.User.Where(a => a.account == account).FirstOrDefault();
                if (Account != null)
                {
                    //Send Email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(Account.account, resetCode, "ResetPassword");
                    Account.ResetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "密碼確認信已寄出";
                }
                else
                {
                    message = "帳號不存在";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            using (dbktnEntities dc = new dbktnEntities())
            {
                var user = dc.User.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (dbktnEntities dc = new dbktnEntities())
                {
                    var user = dc.User.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "新密碼已成功更新";
                    }
                }
            }
            else
            {
                message = "未成功更新密碼";
            }
            ViewBag.Message = message;
            return View(model);
        }
        
        //編輯個人檔案
        [Authorize]
        public ActionResult EditProfile()
        {
            string uid = User.Identity.Name;
            dbktnEntities db = new dbktnEntities();
            var personalProfile = db.User.Where(m => m.account == uid).FirstOrDefault();
            return View(personalProfile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(string name, string phone, string address, HttpPostedFileBase photo)
        {
            string uid = User.Identity.Name;
            dbktnEntities db = new dbktnEntities();
            var personalProfile = db.User.Where(m => m.account == uid).FirstOrDefault();
            personalProfile.name = name;
            personalProfile.phone = phone;
            personalProfile.address = address;
            personalProfile.ConfirmPassword = personalProfile.password; //模型驗證出錯
            if (photo != null)
            {
                string photoName = Guid.NewGuid().ToString() + Path.GetFileName(photo.FileName);
                var path = Path.Combine(Server.MapPath("~/UserPhotos"), photoName);
                photo.SaveAs(path);
                personalProfile.photo = photoName;
            }
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw;
            }
            
            return RedirectToAction("Index","Home");
        }


        //編輯密碼
        [Authorize]
        public ActionResult EditPassword()
        {
            string uid = User.Identity.Name;
            dbktnEntities db = new dbktnEntities();
            var personalProfile = db.User.Where(m => m.account == uid).FirstOrDefault();
            return View(personalProfile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(User user)
        {
            string uid = User.Identity.Name;
            dbktnEntities db = new dbktnEntities();
            var personalProfile = db.User.Where(m => m.account == uid).FirstOrDefault();
            user.password = Crypto.Hash(user.ConfirmPassword);
            user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
            personalProfile.password = user.password;
            personalProfile.ConfirmPassword = user.ConfirmPassword;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("Index", "Home");
        }


    }

}