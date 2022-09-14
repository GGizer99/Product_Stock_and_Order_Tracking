using Product_Stock_and_Order_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Product_Stock_and_Order_Tracking.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        DBStokTakipEntities3 db = new DBStokTakipEntities3();
        public ActionResult Index(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = db.Users.FirstOrDefault(x => x.Email == userName);
                var model = db.Orders.Where(x => x.UserId == user.Id).ToList().ToPagedList(page , 5);
                return View(model);
            }
            return HttpNotFound();


        }

        public ActionResult GetOrder(int id)
        {
            var model = db.Basket.FirstOrDefault(x => x.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOrders(int id)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var model = db.Basket.FirstOrDefault(x => x.Id == id);
                    var order = new Orders
                    {
                        UserId = model.UserId,
                        ProductId = model.ProductId,
                        Count = model.Count,
                        Picture = model.Picture,
                        Price = model.Price,
                        Date = model.Date,
                    };
                    db.Basket.Remove(model);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    ViewBag.action = "Siparişleriniz Ana Merkeze BAŞARILI Bir Şekilde Ulaşmıştır";
                }

            }
            catch (Exception)
            {

                ViewBag.action = "Sipariş Oluşturma İşleminiz BAŞARISIZ";
            }
            
            return View("action");
        }

        public ActionResult GetAllOrder(decimal?Amount)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = db.Users.FirstOrDefault(x => x.Email == userName);
                var model = db.Basket.Where(x => x.UserId == user.Id).ToList();
                var userID = db.Basket.FirstOrDefault(x => x.UserId == user.Id);

                if (model != null)
                {
                    if (userID == null)
                    {
                        ViewBag.Amount = "Sipariş için Sepetinizde Ürün Yoktur.";
                    }
                    else if (userID != null)
                    {
                        Amount = db.Basket.Where(x => x.UserId == userID.UserId).Sum(x => x.Products.Price * x.Count);
                        ViewBag.Amount = "Toplam Sipariş Tutarı = " + Amount + " TL";
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult GetAllOrders()
        {
            var userName = User.Identity.Name;
            var user = db.Users.FirstOrDefault(x => x.Email == userName);
            var model = db.Basket.Where(x => x.UserId == user.Id).ToList();
            int line = 0;

            foreach (var p in model)
            {
                var order = new Orders
                {
                    UserId = model[line].UserId,
                    ProductId = model[line].ProductId,
                    Count = model[line].Count,
                    Picture = model[line].Picture,
                    Price = model[line].Price,
                    Date = DateTime.Now

                };
                db.Orders.Add(order);
                db.SaveChanges();
                line++;
            }
            db.Basket.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Basket");
        }

        [Authorize(Roles = "A")]
        public ActionResult OrderAllList(int page = 1)
        {
            //  var users = db.Users.ToList();


            return View(db.Orders.ToList().ToPagedList(page, 5));

        }
    }
}