using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.CompanyManagement;
using BookPakistanTourClasslibrary.TourManagement;
using BookPakistanTourClasslibrary.UserManagement;
using FYProject1.Models;

namespace BookPakistanTour.Controllers
{
    public class TourController : Controller
    {
        // GET: Tour
        public ActionResult TourManagment()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }
            List<Tour> tours = new TourHandler().GetAllTours();
            ViewBag.message = TempData["message"];
            return View(tours);
        }

        public ActionResult TourDetails(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            Tour tour = new TourHandler().GetTourById(id);
            return View(tour);
        }

        [HttpGet]
        public ActionResult AddTour()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            ViewBag.companies = ModelHelper.ToSelectItemList(new CompanyHandler().GetAllCompanies());
            return View();
        }

        [HttpPost]
        public ActionResult AddTour(FormCollection fdata)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Product", act = "AddProduct" });
            }
            try
            {
                Tour t = new Tour
                {
                    Title = fdata["Title"],
                    Description = fdata["Description"],
                    Price = Convert.ToSingle(fdata["Price"]),
                    Sale = Convert.ToSingle(fdata["Sale"]),
                    Company = new Company { Id = Convert.ToInt32(fdata["CompanyList"]) },
                    DepartureDate = fdata["DepartureDate"]
                };
                long numb = DateTime.Now.Ticks;
                int count = 0;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        string name = file.FileName;
                        string url = "/ImagesData/TourImages/" + numb + "_" + ++count +
                                     file.FileName.Substring(file.FileName.LastIndexOf("."));
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        t.TourImages.Add(new TourImages() { Caption = name, ImageUrl = url });
                    }
                    else
                    {
                        string name = "No Image";
                        string url = "/ImagesData/TourImages/noimage2.jpg";
                        t.TourImages.Add(new TourImages { Caption = name, ImageUrl = url });
                    }
                }

                new TourHandler().AddTour(t);
                return RedirectToAction("TourManagment");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult EditTour(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            Tour tour = new TourHandler().GetTourById(id);
            ViewBag.companies = ModelHelper.ToSelectItemList(new CompanyHandler().GetAllCompanies());
            return View(tour);
        }

        // POST: Company/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditTour(Tour tour, FormCollection fdata)
        {
            try
            {
                User u = (User)Session[WebUtil.CURRENT_USER];
                if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
                {
                    return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
                }

                if (ModelState.IsValid)
                {
                    tour.Company = new Company { Id = Convert.ToInt32(fdata["CompanyList"]) };
                    new TourHandler().UpdateTour(tour);
                    return RedirectToAction("TourManagment");
                }
                return View(tour);
            }
            catch
            {
                return View();
            }
        }


        public ActionResult DeleteTour(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            Tour tour = new TourHandler().GetTourById(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tour/Delete/
        [HttpPost, ActionName("DeleteTour")]
        public ActionResult DeleteTourConfirmed(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            try
            {
                Tour tour = new TourHandler().GetTourById(id);
                new TourHandler().DeleteTour(tour);


                return RedirectToAction("TourManagment");
            }
            catch
            {
                ViewBag.message = "Cannot Delete. This Tour Is Assigned To Some Booking. Delete That Booking First";
                TempData["message"] = ViewBag.message;
                return RedirectToAction("TourManagment");
            }
        }

        public int GetTourCount()
        {
            return new TourHandler().GetTourCount();
        }
    }
}