using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.CompanyManagement;
using BookPakistanTourClasslibrary.LocationManagement;
using BookPakistanTourClasslibrary.UserManagement;
using FYProject1.Models;

namespace BookPakistanTour.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult CompanyManagment()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }
            List<Company> companies = new CompanyHandler().GetAllCompanies();
            return View(companies);
        }

        // GET: Company/Details/
        public ActionResult CompanyDetails(int id)
        {
            Company company = new CompanyHandler().GetCompanybyId(id);
            return View(company);
        }

        // GET: Company/Create
        public ActionResult AddCompany()
        {
            LocationHandler lh = new LocationHandler();
            ViewBag.CountryList = ModelHelper.ToSelectItemList(lh.GetCountries());
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult AddCompany(FormCollection fdata)
        {
            try
            {
                Company c = new Company
                {
                    Name = fdata["Name"],
                    Description = fdata["Description"],
                    FacebookPageUrl = fdata["FacebookPageUrl"],
                    City = new City { Id = Convert.ToInt32(fdata["CityList"]) }
                };
                long numb = DateTime.Now.Ticks;
                int count = 0;
                foreach (string fname in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fname];
                    if (!string.IsNullOrEmpty(file?.FileName))
                    {
                        string url = "/ImagesData/CompanyImages/" + numb + "_" + ++count + file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        c.ImageUrl = url;
                    }
                    else
                    {
                        string url = "/ImagesData/CompanyImages/noimage2.jpg";
                        c.ImageUrl = url;
                    }
                }
                new CompanyHandler().AddCompany(c);
                return RedirectToAction("CompanyManagment");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Edit/
        public ActionResult EditCompany(int id)
        {
            Company company = new CompanyHandler().GetCompanybyId(id);
            return View(company);
        }

        // POST: Company/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditCompany(Company company)
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
                    new CompanyHandler().UpdateCompany(company);
                    return RedirectToAction("CompanyManagment");
                }
                return View(company);
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult DeleteCompany(int id)
        {
            Company company = new CompanyHandler().GetCompanybyId(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("DeleteCompany")]
        public ActionResult DeleteCompanyConfirmed(int id)
        {
            try
            {
                Company company = new CompanyHandler().GetCompanybyId(id);
                new CompanyHandler().DeleteCompany(company);
                return RedirectToAction("CompanyManagment");
            }
            catch
            {
                return View();
            }
        }

        public int GetCompanyCount()
        {
            return new CompanyHandler().GetCompanyCount();
        }
    }
}
