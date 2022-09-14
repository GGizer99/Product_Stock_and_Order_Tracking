using Product_Stock_and_Order_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_Stock_and_Order_Tracking.Controllers

    
{
    [Authorize(Roles = "A")]
    public class CategoryController : Controller
    {
        DBStokTakipEntities3 db = new DBStokTakipEntities3();

        
        public ActionResult Index()
        {
            return View(db.Categories.Where(x => x.Status == true).ToList());
        }

        
        public ActionResult AddCategory()
        {

            return View();
        }

        
        [HttpPost]
        public ActionResult AddCategory(Categories ctgr)
        {
            db.Categories.Add(ctgr);
            ctgr.Status = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        public ActionResult UpdateCategory()
        {

            return View();

        }

        [HttpPost]
        public ActionResult UpdateCategory(Categories category)
        {
            var ctgr = db.Categories.Where(x => x.Id == category.Id).FirstOrDefault();

            ctgr.Name = category.Name;
            ctgr.Description = category.Description;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        
        
        public ActionResult DeleteCategory(int id)
        {
            var ctgr = db.Categories.Where(x => x.Id == id).FirstOrDefault();
            
            ctgr.Status = false;    

            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}