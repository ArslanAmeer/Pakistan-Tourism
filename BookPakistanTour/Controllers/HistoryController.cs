using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.HistoryManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTour.Controllers
{
    public class HistoryController : Controller
    {
        private DbContextClass db = new DbContextClass();

        // GET: History
        public ActionResult Index()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            return View(db.Histories.ToList());
        }

        // GET: History/Details/5
        public ActionResult Details(int? id)
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
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // GET: History/Create
        public ActionResult Create()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            return View();
        }

        // POST: History/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ImageUrl")] History history, FormCollection fdata)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }
            if (ModelState.IsValid)
            {
                long numb = DateTime.Now.Ticks;
                int count = 0;
                foreach (string fname in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fname];
                    if (!string.IsNullOrEmpty(file?.FileName))
                    {
                        string url = "/ImagesData/HistoryImages/" + numb + "_" + ++count + file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        history.ImageUrl = url;
                    }
                    else
                    {
                        string url = "/ImagesData/HistoryImages/noimage2.jpg";
                        history.ImageUrl = url;
                    }
                }

                db.Histories.Add(history);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(history);
        }

        // GET: History/Edit/5
        public ActionResult Edit(int? id)
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
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: History/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ImageUrl")] History history, FormCollection fdata)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Admin", act = "AdminPanel" });
            }

            foreach (string fname in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fname];
                if (!string.IsNullOrEmpty(file?.FileName))
                {

                    if (history.ImageUrl != null)
                    {
                        string oldpth = Request.MapPath(history.ImageUrl);
                        if (System.IO.File.Exists(oldpth))
                        {
                            System.IO.File.Delete(oldpth);
                        }
                    }

                    string url = history.ImageUrl;
                    string path = Request.MapPath(url);
                    file.SaveAs(path);
                }
            }

            db.Entry(history).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: History/Delete/5
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
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
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
            History history = db.Histories.Find(id);
            db.Histories.Remove(history);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int GetHistoryCount()
        {
            return db.Histories.Count();
        }
    }
}
