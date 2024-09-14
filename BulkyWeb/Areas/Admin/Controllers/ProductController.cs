using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.ProjectModel;
using System.Collections.Generic;
using System.Text;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
       
        
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
           
            List<Product> objProductList = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
           
            
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
          
            ProductVM productvm = new()
            {
                CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productvm);
            }
            else
            {
                //update
                productvm.Product = _unitofwork.Product.Get(u=>u.Id == id);
                return View(productvm);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM , IFormFile? file)
        {
           if (ModelState.IsValid)
            {
              
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath,@"Images\Product" );
                        
                        

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
                            file.CopyTo(fileStream);
                        }



                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;

                    

                }

                if (productVM.Product.Id == 0)
                {
                    _unitofwork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitofwork.Product.Update(productVM.Product);
                }


                
             
                _unitofwork.Save();
                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
       
        
        
        
        
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitofwork.Product.Get(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();

            }
            _unitofwork.Product.Remove(obj);
            _unitofwork.Save(); ;
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");


        }


         
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitofwork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"Images\Product" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitofwork.Product.Remove(productToBeDeleted);
            _unitofwork.Save();

            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }



    }
}
