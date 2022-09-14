using Product_Stock_and_Order_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_Stock_and_Order_Tracking.Controllers
{
    public class UserController : Controller
    {

        DBStokTakipEntities3 db = new DBStokTakipEntities3();


        [Authorize]
        public ActionResult UserList()
        {
          //  var users = db.Users.ToList();


            return View(db.Users.ToList());
        }

    }
}