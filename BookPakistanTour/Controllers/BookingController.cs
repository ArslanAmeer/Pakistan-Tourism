using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.BookingManagment;
using BookPakistanTourClasslibrary.TourManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTour.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BookingController : Controller
    {
        // GET: Booking
        public ActionResult BookTrip(int id)
        {
            User user = (User)Session[WebUtil.CURRENT_USER];
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            Tour tour = new TourHandler().GetTourById(id);
            ViewBag.HideSlider = true;
            return View(tour);
        }

        public ActionResult BookingConfirmed(int id)
        {
            User user = (User)Session[WebUtil.CURRENT_USER];
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }
            Tour tour = new TourHandler().GetTourById(id);

            Booking booking = new Booking
            {
                Tour = tour,
                User = user
            };

            new BookingHandler().AddBooking(booking);
            ViewBag.HideSlider = true;
            return View();
        }
    }
}