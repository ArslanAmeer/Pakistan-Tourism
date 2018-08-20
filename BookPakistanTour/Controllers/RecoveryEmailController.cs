using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BookPakistanTour.Models;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTour.Controllers
{
    public class RecoveryEmailController : Controller
    {
        // GET: RecoveryEmail
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.HideSlider = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(recoveryEmail data)
        {
            if (ModelState.IsValid)
            {
                User user = new UserHandler().GetUserByEmail(data.Email);
                if (user == null)
                {
                    ViewBag.error = "Email Not Registered. Please Enter Registered Email Address";
                    return View();
                }

                try
                {
                    string randomnumb = Path.GetRandomFileName().Replace(".", "");
                    var message = new MailMessage();
                    message.From = new MailAddress("pakistantourism.2018@gmail.com");
                    message.To.Add(data.Email);
                    message.Subject = "-No-Reply- Password Recovery Email by PAKISTAN TOURISM";
                    message.IsBodyHtml = true;
                    message.Body = "Please use this password: <b><u>" + randomnumb +
                                   "</u></b> , Next Time You Login! And dont forget to change your password";

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                        ("pakistantourism.2018@gmail.com", "pakistan1947");

                    smtp.EnableSsl = true;
                    smtp.Send(message);
                    user.Password = randomnumb;
                    new UserHandler().UpdateUser(user);
                    ViewBag.success = "Email Has been sent to  " + data.Email;

                    ViewBag.HideSlider = true;
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Error Sending Mail. Please Try Again Later!";
                    ViewBag.HideSlider = true;
                    return View();
                }
            }
            return View();
        }
    }
}

