
using meo.DataAccess.Data;

using meo.DataAccess.Repository.IRepository;
using meo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace meo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategory = _unitOfWork.Category.GetAll().ToList();
            return View(objCategory);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {

            if (obj.CategoryName == obj.CategoryOrder.ToString())
            {
                ModelState.AddModelError("CategoryName", "name and order can not the same");

            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                var obj = _unitOfWork.Category.Get(u => u.CategoryID == id);
                if (obj != null)
                {
                    return View(obj);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.CategoryName == obj.CategoryOrder.ToString())
            {
                ModelState.AddModelError("CategoryName", "The name and order cannot be the same.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Category.Update(obj);
                    _unitOfWork.Save();
                    TempData["Success"] = "Edit Success";
                    return RedirectToAction("Index", "Category");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes: " + ex.Message);
                }
            }

            return View(obj);
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id.HasValue && id.Value != 0)
        //    {
        //        var obj = _unitOfWork.Category.Get(u => u.CategoryID == id);
        //        if (obj != null)
        //        {
        //            return View(obj);
        //        }
        //    }
        //    return NotFound();
        //}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int ? id)
        {
            if(id != 0 && id.HasValue)
            {
                Category Category = _unitOfWork.Category.Get(u => u.CategoryID == id);

                if (Category != null)
                {
                    _unitOfWork.Category.Remove(Category);
                    _unitOfWork.Save();
                    TempData["Success"] = "Delete Success!";
                    return RedirectToAction("Index", "Category");
                }
            }
          
            return NotFound();
            
        }

    }
}
