using Product_Stock_and_Order_Tracking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Product_Stock_and_Order_Tracking.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        DBStokTakipEntities3 db = new DBStokTakipEntities3();


        [Authorize]
        public ActionResult Index(string search)
        {
            var list = db.Products.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).ToList();
            }
            return View(list);
        }

        /*--------------------------------------------------------------------------------*/

        [Authorize(Roles ="A")]
        public ActionResult AddProduct()
        {

            List<SelectListItem> selectListItems = (from x in db.Categories.ToList()


                                                    select new SelectListItem
                                                    {
                                                        Text = x.Name, Value = x.Id.ToString()

                                                    }).ToList();
            ViewBag.category = selectListItems;
            return View();
        }

        /*--------------------------------------------------------------------------------*/

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult AddProduct(Products pr,HttpPostedFileBase file)
        {
            string path = Path.Combine("~/Content/Image" + file.FileName);
            file.SaveAs(Server.MapPath(path));
            pr.Picture = file.FileName.ToString();
            db.Products.Add(pr);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*--------------------------------------------------------------------------------*/

        [Authorize(Roles = "A")]
        public ActionResult DeleteProduct(int id)
        {
            var product = db.Products.Where(x => x.Id == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /*--------------------------------------------------------------------------------*/


        [Authorize(Roles = "A")]
        public ActionResult UpdateProduct(int id)
        {
            var product = db.Products.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> selectListItems = (from x in db.Categories.ToList()


                                                    select new SelectListItem
                                                    {
                                                        Text = x.Name,
                                                        Value = x.Id.ToString()

                                                    }).ToList();
            ViewBag.category = selectListItems;

            return View(product);
                
        }

        /*--------------------------------------------------------------------------------*/

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult UpdateProduct(Products pr, HttpPostedFileBase file)
        {
            var product = db.Products.Find(pr.Id);

            if (file == null)
            {
                product.Name = pr.Name;
                product.Description = pr.Description;
                
                product.CategoriId = pr.CategoriId;
                product.Price = pr.Price;
                product.Stock = pr.Stock;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                product.Name = pr.Name;
                product.Description = pr.Description;
                product.CategoriId = pr.CategoriId;
                product.Price = pr.Price;
                product.Stock = pr.Stock;   
                product.Picture = file.FileName.ToString();

                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        /*--------------------------------------------------------------------------------*/

        [Authorize(Roles = "A")]
        public ActionResult StockTrack()
        {
            var track = db.Products.Where(x => x.Stock <= 20).ToList();
            

            return View(track);
        }

        public PartialViewResult StockCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                var counter = db.Products.Where(x => x.Stock <= 20).Count();
                ViewBag.counter = counter;
            }


            return PartialView();
        }
        /*--------------------------------------------------------------------------------*/
        public ActionResult Graph()
        {
            ArrayList pr1 = new ArrayList();
            ArrayList pr2 = new ArrayList();

            var datas = db.Products.ToList();
            datas.ToList().ForEach(x => pr1.Add(x.Name));
            datas.ToList().ForEach(x => pr2.Add(x.Stock));
            var graph = new Chart(width: 500, height: 500).AddTitle("Ürün Stok Grafiği").AddSeries(chartType: "Column", name: "Name", xValue: pr1, yValues: pr2);
            return File(graph.ToWebImage().GetBytes(), "image/jpeg");
        }


    }
}