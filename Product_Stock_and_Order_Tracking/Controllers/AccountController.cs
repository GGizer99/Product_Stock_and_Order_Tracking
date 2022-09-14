using Product_Stock_and_Order_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Product_Stock_and_Order_Tracking.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        DBStokTakipEntities3 db = new DBStokTakipEntities3();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(Users p)
        {
            var bilgiler = db.Users.FirstOrDefault(x => x.Email == p.Email && x.Password == p.Password);
            if (bilgiler!=null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.Email, false);
                Session["BayiADI"] = bilgiler.BayiAdi.ToString();
                Session["Mail"] = bilgiler.Email.ToString();
                Session["Name"] = bilgiler.Name.ToString();
                Session["Surname"] = bilgiler.Surname.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.hata = "KUllanıcı Adı veya Şifre Hatalı";
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Users data)
        {
            db.Users.Add(data);
            data.Role = "U";
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
        }
    }
}