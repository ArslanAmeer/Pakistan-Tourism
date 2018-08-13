using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary.FeedbackManagement;
using BookPakistanTourClasslibrary.TourManagement;
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

        public ActionResult ToursByCompany(string id)
        {
            throw new NotImplementedException();
        }

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

    }
}