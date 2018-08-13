using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary.FeedbackManagement;
using BookPakistanTourClasslibrary.TourManagement;
using BookPakistanTourClasslibrary.UserManagement;
using FYProject1.Models;

namespace BookPakistanTour.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.indexTours = ModelHelper.ToTourSummaryList(new TourHandler().GetLatestTours(8));
            return View();
        }

        public ActionResult TourDetail(int id)
        {
            Tour tour = new TourHandler().GetTourById(id);
            if (tour != null)
            {
                ViewBag.feedbacks = new FeedbackHandler().GetAllFeedbackByTourId(tour.Id);
            }
            else
            {
                ViewBag.feedbacks = null;
            }
            return View(tour);
        }

        public ActionResult ToursByCompany(string name)
        {
            ViewBag.indexTours = ModelHelper.ToTourSummaryList(new TourHandler().GetToursByCompanyName(name));
            return View();
        }

        [HttpPost]
        public ActionResult SaveFeedback(FormCollection fdata, int id)
        {
            try
            {
                Feedback feedback = new Feedback
                {
                    Name = fdata["Name"],
                    Message = fdata["Message"],
                    DateEntered = Convert.ToString(DateTime.Now),
                    Tour = new TourHandler().GetTourById(id)
                };

                new FeedbackHandler().AddFeedback(feedback);

                return RedirectToAction("TourDetail", new { Id = id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(FormCollection fdata)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(fdata["Email"]);
                message.To.Add("pakistantourism.2018@gmail.com");
                message.Subject = "Contact Us From Email: " + message.From;
                message.IsBodyHtml = true;
                message.Body = fdata["Msg"] + "  <br/><br/> From: " + fdata["Name"] + " <br/> Email: " + message.From;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;

                smtp.Credentials = new System.Net.NetworkCredential
                    ("pakistantourism.2018@gmail.com", "pakistan1947");

                smtp.EnableSsl = true;
                smtp.Send(message);
                ViewBag.SuccessMessage = "Thank you for Contacting us ";
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ViewBag.ErrorMessage = $" Sorry we are facing Problem here {ex.Message}";
            }
            return View();
        }

    }
}