using Product_Stock_and_Order_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_Stock_and_Order_Tracking.Controllers
{
    public class BasketController : Controller
    {
        DBStokTakipEntities3 db = new DBStokTakipEntities3();
        // GET: Basket
        public ActionResult Index(decimal?Amount)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = db.Users.FirstOrDefault(x => x.Email == userName);
                var model = db.Basket.Where(x => x.UserId == user.Id).ToList();
                var userID = db.Basket.FirstOrDefault(x => x.UserId == user.Id);

                if (model!=null)
                {
                    if (userID == null)
                    {
                        ViewBag.Amount = "Sipariş için Sepetinizde Ürün Yoktur.";
                    }
                    else if(userID!=null)
                    {
                        Amount = db.Basket.Where(x => x.UserId == userID.UserId).Sum(x => x.Products.Price * x.Count);
                        ViewBag.Amount = "Toplam Sipariş Tutarı = " + Amount + " TL";
                    }
                    return View(model);
                }
            }
            return HttpNotFound();

        }

        public ActionResult AddToBasket(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var model = db.Users.FirstOrDefault(x => x.Email == userName);
                var product = db.Products.Find(id);
                var basket = db.Basket.FirstOrDefault(x => x.UserId == model.Id && x.ProductId == id);

                if (model!=null)
                {
                    if (basket!=null)
                    {
                        basket.Count++;
                        basket.Price = basket.Count * product.Price;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var b = new Basket
                    {

                        UserId = model.Id,
                        ProductId = product.Id,
                        Count = 1,
                        Price = product.Price,
                        Date = DateTime.Now
                    };
                    db.Basket.Add(b);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            return HttpNotFound();
        }

        public ActionResult Decrease(int id,int pid)
        {
            var model = db.Basket.Find(id);
            var product = db.Products.Find(pid);
            if (model.Count == 1)
            {
                db.Basket.Remove(model);
                db.SaveChanges();
            }
            model.Count--;
            model.Price = model.Count * product.Price;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Increase(int id,int pid)
        {
            var model = db.Basket.Find(id);
            var product = db.Products.Find(pid);
            model.Count++;
            model.Price = model.Count * product.Price;
            db.SaveChanges();
            return  RedirectToAction("Index");
        }

        public ActionResult DeleteProduct(int id)
        {
            var deleteProduct = db.Basket.Find(id);
            db.Basket.Remove(deleteProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult DeleteAllOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var model = db.Users.FirstOrDefault(x => x.Email == userName);
                var delete = db.Basket.Where(x => x.UserId == model.Id);
                db.Basket.RemoveRange(delete); //All Product will delete
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
;
        }



    }
}