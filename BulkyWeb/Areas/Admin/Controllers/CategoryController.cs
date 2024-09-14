
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
       

        
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            /*List<Category> objcategoryList = _unitofwork.Category.Categories.ToList();*/
            List<Category> objcategoryList = _unitofwork.Category.GetAll().ToList();
            return View(objcategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name.Length < 2)
            {
                ModelState.AddModelError("Name", "Name should not be a Single Alphabet");
            }
            if (ModelState.IsValid)
            {
                /*_unitofwork.Category.Categories.Add(obj);
                _unitofwork.Category.SaveChanges();*/
                _unitofwork.Category.Add(obj);
                _unitofwork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            /* Category? categoryFromdb = _unitofwork.Category.Categories.FirstOrDefault(c => c.Id == id);*/
            Category? categoryFromdb = _unitofwork.Category.Get(c => c.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                /*_unitofwork.Category.Categories.Update(obj);
                _unitofwork.Category.SaveChanges();*/
                _unitofwork.Category.Update(obj);
                _unitofwork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromdb = _unitofwork.Category.Get(c => c.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitofwork.Category.Get(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();

            }
            _unitofwork.Category.Remove(obj);
            _unitofwork.Save(); ;
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");


        }

    }
}
