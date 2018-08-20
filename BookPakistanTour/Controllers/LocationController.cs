using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.LocationManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTour.Controllers
{
    public class LocationController : Controller
    {
        // -- Countries ---
        public ActionResult LocationManagment()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            List<Country> countries = new LocationHandler().GetCountries();
            ViewBag.message = TempData["message"];
            return View(countries);
        }

        public ActionResult AddCountry()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCountry([Bind(Include = "Id,Name,CountryCode,Image_URL")] Country country)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            if (ModelState.IsValid)
            {
                new LocationHandler().AddCountry(country);
                return RedirectToAction("LocationManagment");
            }
            return View(country);
        }

        public ActionResult DeleteCountry(int? id)
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
            Country country = new LocationHandler().GetCountryById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }


        [HttpPost, ActionName("DeleteCountry")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCountryConfirmed(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            try
            {
                Country country = new LocationHandler().GetCountryById(id);
                new LocationHandler().DeleteCountry(country);
                return RedirectToAction("LocationManagment");
            }
            catch
            {
                Country country = new LocationHandler().GetCountryById(id);
                ViewBag.message = $"Cannot Delete Country. Delete All Cities of {country.Name} First";
                TempData["message"] = ViewBag.message;
                return RedirectToAction("LocationManagment");
            }

        }

        //// -- Cities ---

        public ActionResult CityList(int id)
        {
            List<City> cities = new LocationHandler().GetCitiesByCountryId(id);
            ViewBag.CountryID = id;
            ViewBag.CountryName = new LocationHandler().GetCountryById(id).Name;
            return View(cities);
        }

        public ActionResult AddCity(int id, string countryName)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            ViewBag.CountryID = id;
            ViewBag.CountryName = countryName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCity([Bind(Include = "Id,Name")] City city, int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            City c = new City();
            if (ModelState.IsValid)
            {
                c.Name = city.Name;
                c.Country = new LocationHandler().GetCountryById(id);
                new LocationHandler().AddCity(c);
                return RedirectToAction("CityList", new { Id = id });
            }
            return View();
        }

        public ActionResult DeleteCity(int? id, int countryId)
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

            City city = new LocationHandler().GetCityById(id);

            if (city == null)
            {
                return HttpNotFound();
            }
            ViewBag.countryId = countryId;
            TempData["countryId"] = countryId;
            return View(city);
        }

        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCityConfirmed(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            try
            {
                City city = new LocationHandler().GetCityById(id);
                new LocationHandler().DeleteCity(city);
                return RedirectToAction("CityList", new { Id = Convert.ToUInt32(TempData["countryId"]) });
            }
            catch
            {
                City city = new LocationHandler().GetCityById(id);
                ViewBag.message = $"Cannot Delete City. {city.Name} Is Assigned To Some Other Entity (User or Company). Delete That First";
                TempData["message"] = ViewBag.message;
                return RedirectToAction("LocationManagment");
            }
        }

        public int GetCountriesCount()
        {
            return (new DbContextClass().Countries.Count());
        }
    }
}