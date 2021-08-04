using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yased_Api.Models;

namespace Yased_Api.Controllers
{
    public class AdminController : Controller
    {
        YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var login = db.Users.Where(x => x.Email == user.Email).SingleOrDefault();

            if (login != null)
            {
                if (login.Email == user.Email && login.Password == user.Password)
                {
                    Session["AdminId"] = login.id;
                    Session["AdminEmail"] = login.Email;
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {

            Session["AdminId"] = null;
            Session["AdminEmail"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }


    }
}