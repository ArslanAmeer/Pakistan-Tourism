using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BookPakistanTour.Models;
using BookPakistanTourClasslibrary;
using BookPakistanTourClasslibrary.LocationManagement;
using BookPakistanTourClasslibrary.UserManagement;
using FYProject1.Models;

namespace BookPakistanTour.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {

            HttpCookie myCookie = Request.Cookies["idpas"];
            if (myCookie != null)
            {
                User u = new UserHandler().GetUser(myCookie.Values["lid"], myCookie.Values["psd"]);

                if (u != null)
                {
                    myCookie.Expires = DateTime.Today.AddDays(7);
                    Response.SetCookie(myCookie);

                    Session.Add(WebUtil.CURRENT_USER, u);

                    if (u.IsInRole(WebUtil.ADMIN_ROLE))
                    {
                        return RedirectToAction("AdminPanel", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Controller = Request.QueryString["ctl"];
            ViewBag.Action = Request.QueryString["act"];
            ViewBag.HideSlider = true;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HideSlider = true;
                return View();
            }

            User u = new UserHandler().GetUser(data.Email, data.Password);

            if (u != null)
            {
                if (data.RememberMe)
                {
                    HttpCookie c = new HttpCookie("idpas")
                    {
                        Expires = DateTime.Today.AddDays(7),
                    };
                    c.Values.Add("lid", u.Email);
                    c.Values.Add("psd", u.Password);
                    Response.SetCookie(c);
                }

                Session.Add(WebUtil.CURRENT_USER, u);

                if (u.IsInRole(WebUtil.ADMIN_ROLE))
                {
                    return RedirectToAction("AdminPanel", "Admin");
                }
                else
                {
                    try
                    {
                        var message = new MailMessage { From = new MailAddress(u.Email) };
                        message.To.Add("ADD_YOUR_OWN");
                        message.Subject = "User Login From Email: " + message.From;
                        message.IsBodyHtml = true;
                        message.Body = "A user Just Log In to Your Site  <br/><br/> Name: " + u.FullName + " <br/> Email: " + u.Email;

                        SmtpClient smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            Credentials = new System.Net.NetworkCredential
                                ("ADD_YOUR_OWN", "ADD_YOUR_OWN"),
                            EnableSsl = true
                        };
                        smtp.Send(message);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception)
                    {
                        ViewBag.failed = "You are Required to have an Internet Connection to Login.";
                    }
                }
            }
            else
            {
                ViewBag.failed = "Invalid Username or Password! Please Try Again ";
            }
            ViewBag.HideSlider = true;
            return View();
        }

        public ActionResult Logout()
        {
            ActionResult obj;

            User u = (User)Session[WebUtil.CURRENT_USER];

            if (u != null && u.IsInRole(WebUtil.ADMIN_ROLE))
            {
                obj = RedirectToAction("Login", "User");
            }
            else
            {
                obj = RedirectToAction("Index", "Home");
            }

            Session.Abandon();

            HttpCookie ck = Request.Cookies["idpas"];
            if (ck != null)
            {
                ck.Expires = DateTime.Now;
                Response.SetCookie(ck);
            }
            return obj;
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            ViewBag.HideSlider = true;
            LocationHandler lh = new LocationHandler();
            ViewBag.CountryList = ModelHelper.ToSelectItemList(lh.GetCountries());

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection fdata)
        {
            try
            {
                User u = new User
                {
                    FullName = fdata["FullName"],
                    Email = fdata["Email"],
                    Password = fdata["Password"],
                    FullAddress = fdata["FullAddress"],
                    Phone = Convert.ToInt64(fdata["Phone"]),
                    IsActive = false,
                    City = new City { Id = Convert.ToInt32(fdata["CityList"]) },
                    BirthDate = fdata["BirthDate"]
                };

                string gender = Convert.ToString(fdata["Gender"]);
                if (gender != null && gender == "Male")
                {
                    u.Male = true;
                    u.Female = false;
                }
                else if (gender != null && gender == "Female")
                {
                    u.Male = false;
                    u.Female = true;
                }

                u.Role = new UserHandler().GetRoleById(2);

                long numb = DateTime.Now.Ticks;
                int count = 0;
                foreach (string fname in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fname];
                    if (!string.IsNullOrEmpty(file?.FileName))
                    {
                        string url = "/ImagesData/UserImages/" + numb + "_" + ++count + file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        u.ImageUrl = url;
                    }
                    else
                    {
                        string url = "/ImagesData/UserImages/noimage.jpg";
                        u.ImageUrl = url;
                    }
                }

                new UserHandler().Adduser(u);
                ViewBag.HideSlider = true;
                return RedirectToAction("Login");

            }
            catch (Exception)
            {
                throw;
            }


        }

        [HttpGet]
        public ActionResult CityLists(int id)
        {
            DDViewModel dm = new DDViewModel
            {
                Name = "CityList",
                Label = "- Your City -",
                Values = ModelHelper.ToSelectItemList(new LocationHandler().GetCities(new Country { Id = id }))
            };

            return PartialView("~/Views/Shared/_DDLView.cshtml", dm);
        }

        [HttpGet]
        public ActionResult UserManagment()
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }

            List<User> users = new UserHandler().GetAllUsers();

            ViewBag.roles = ModelHelper.ToSelectItemList(new UserHandler().GetRoles());
            ViewBag.message = TempData["message"];
            return View(users);
        }

        [HttpGet]
        public ActionResult UserDetails(int? id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = new UserHandler().GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult UserGuestUpdate(int? id)
        {
            LocationHandler lh = new LocationHandler();
            ViewBag.CountryList = ModelHelper.ToSelectItemList(lh.GetCountries());

            User u = (User)Session[WebUtil.CURRENT_USER];
            if (u != null && u.IsInRole(WebUtil.ADMIN_ROLE))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("UserEdit", new { id = u.Id });
            }
            else if (u != null && !(u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = new UserHandler().GetUserById(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.HideSlider = true;
                return View(user);
            }
            ViewBag.HideSlider = true;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UserGuestUpdate(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new UserHandler().UpdateUser(user);
                    Session.Add(WebUtil.CURRENT_USER, new UserHandler().GetUserById(user.Id));
                }

                ViewBag.msg = "Update Successfully";
            }
            catch (Exception e)
            {
                ViewBag.msg = "Failed To Update! (" + e.Message + ")";
            }
            ViewBag.HideSlider = true;
            return View(user);
        }

        public ActionResult UserEdit(int? id)
        {
            LocationHandler lh = new LocationHandler();
            ViewBag.CountryList = ModelHelper.ToSelectItemList(lh.GetCountries());
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = new UserHandler().GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit(User user, FormCollection fdata)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }

            if (ModelState.IsValid)
            {
                user.City = new City { Id = Convert.ToInt32(fdata["CityList"]) };
                user.Role = new Role { Id = Convert.ToInt32(fdata["Role.Id"]) };
                new UserHandler().UpdateUserByAdmin(user);
                return RedirectToAction("UserManagment");
            }
            return View(user);
        }

        public ActionResult UserDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = new UserHandler().GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int id)
        {
            User u = (User)Session[WebUtil.CURRENT_USER];
            if (!(u != null && u.IsInRole(WebUtil.ADMIN_ROLE)))
            {
                return RedirectToAction("Login", "User", new { ctl = "Home", act = "Index" });
            }

            try
            {
                User user = new UserHandler().GetUserById(id);
                if (user.ImageUrl != null)
                {
                    string path = Request.MapPath(user.ImageUrl);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                new UserHandler().DeleteUser(id);
                return RedirectToAction("UserManagment");
            }
            catch
            {
                User user = new UserHandler().GetUserById(id);
                ViewBag.message = $"Cannot Delete User. {user.FullName} Has Made Some Booking. Delete That Booking Entry First";
                TempData["message"] = ViewBag.message;
                return RedirectToAction("UserManagment");
            }
        }

        public int UserCount()
        {
            return new UserHandler().GetUserCount();
        }

        protected override void Dispose(bool disposing)
        {
            DbContextClass _db = new DbContextClass();
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
