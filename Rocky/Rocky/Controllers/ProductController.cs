using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment WebHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = WebHostEnvironment;
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

        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productViewModel.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = filename + extension;
                    _db.Product.Add(productViewModel.Product);
                }
                else
                {
                    //Editing 
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(m => m.Id == productViewModel.Product.Id);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.Image = filename + extension;
                    }
                    else
                    {
                        productViewModel.Product.Image = objFromDb.Image;
                    }

                    _db.Product.Update(productViewModel.Product);
                }
                _db.SaveChanges();   
                return RedirectToAction("Index");
            }
        
            return View(productViewModel);
        }
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
