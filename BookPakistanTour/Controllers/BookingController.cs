using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.BookingManagment;
using BookPakistanTourClasslibrary.TourManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTour.Controllers
{
    public class BookingController : Controller
    {
        private readonly DbContextClass db = new DbContextClass();
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
                UserId = user.Id
            };

            new BookingHandler().AddBooking(booking);
            ViewBag.HideSlider = true;
            return View();
        }

        public ActionResult BookingList()
        {
            List<Booking> bookings = new BookingHandler().GetAllBookings();
            return View(bookings);
        }

        public ActionResult Delete(int? id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Booking booking = new BookingHandler().GetBookingById(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: History/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            Booking booking = new BookingHandler().GetBookingById(id);
            db.Entry(booking.Tour).State = EntityState.Unchanged;
            db.Entry(booking.User).State = EntityState.Unchanged;
            db.Entry(booking).State = EntityState.Deleted;
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("BookingList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int GetBokingCount()
        {
            return (new BookingHandler().BookingCount());
        }
    }
}