using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;
            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            }
            return View(objList);
        }
        
        
        //Get - Upsert - Insert and Update
        public IActionResult Upsert(int? id)
        {
            ProductViewModel ProductVM = new ProductViewModel
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //This is for Create
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _db.Product.Find(id);
                if (ProductVM.Product == null)
                {
                    return NotFound();
                }
                return View(ProductVM);
            }
        }

        // //POST - Upsert
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Upsert(Category obj)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _db.Category.Add(obj); 
        //         _db.SaveChanges();   
        //         return RedirectToAction("Index");
        //     }
        //
        //     return View(obj);
        // }
        //
        // //Get - Delete
        // public IActionResult Delete(int id)
        // {
        //     if (id == null || id == 0)
        //     {
        //         return NotFound();
        //     }
        //     var obj = _db.Category.Find(id);
        //     if (obj == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(obj);
        // }

        //POST - Delete method for controller
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj); 
            _db.SaveChanges();   
            return RedirectToAction("Index");
        }
    }
}
