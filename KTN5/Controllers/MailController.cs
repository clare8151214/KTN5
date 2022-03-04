using KTN5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace KTN5.Controllers
{
    public class MailController : Controller
    {
        //"charityktn@gmail.com  P@ssw0rd-iii"
        [HttpPost]
        public ActionResult SendEmail(/*string emailID, string emailFor = "VerifyAccount"*/ MailModel model)
        {
           
            var fromEmail = new MailAddress("charityktn@gmail.com", "系統管理員");
            var toEmail = new MailAddress("charityktn@gmail.com");
            var fromEmailPassword = "P@ssw0rd-iii";

            string subject = "";
            string body = "";
            
            subject = "回饋意見";
            body =  "姓名:" + model.Name + "<br/>" + "電子郵件" + model.Email + "<br/>" + model.Message + "<br/>";
            
           
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
            if (model != null)
            {
                return Json("已寄出");
            }
            else
            {
                return Json("發生錯誤沒有寄出");
            }
        }
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
    }
}