using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.BannerManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTour.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminPanel()
        {
            DbContextClass db = new DbContextClass();
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            return View();
        }

        public ActionResult BannerManagment()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User");
            }

            List<MainBanner> banners = new BannerHandler().GetAllBanners();
            return View(banners);

        }

        public ActionResult AddBanner()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddBanner(FormCollection fdata)
        {
            MainBanner b = new MainBanner();
            try
            {
                long numb = DateTime.Now.Ticks;
                int count = 0;
                foreach (string fname in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fname];
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        b.Caption = Convert.ToString(fdata["Caption"]);
                        b.BannerUrl = "/ImagesData/SliderImages/" + file.FileName + numb + "_" + ++count + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                        string path = Request.MapPath(b.BannerUrl);
                        if (file != null)
                        {
                            file.SaveAs(path);
                        }
                        new BannerHandler().AddBanner(b);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("BannerManagment");
        }

        [HttpGet]
        public ActionResult BannerDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MainBanner banner = new BannerHandler().Getbanner(id);

            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        public ActionResult BannerDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MainBanner banner = new BannerHandler().Getbanner(id);

            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        [HttpPost, ActionName("BannerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult BannerDeleteConfirmed(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User");
            }

            //Deleting IMAGE from both database and physical path

            MainBanner banner = new BannerHandler().Getbanner(id);

            string path = Request.MapPath(banner.BannerUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                new BannerHandler().DeleteBanner(id);
            }
            return RedirectToAction("BannerManagment");
        }

        public int GetBannerCount()
        {
            return new BannerHandler().GetBannerCount();
        }

        //Garbage Colector and Disposing off Method
        protected override void Dispose(bool disposing)
        {
            DbContextClass db = new DbContextClass();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}