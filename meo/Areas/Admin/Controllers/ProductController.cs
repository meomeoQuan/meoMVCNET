
using meo.DataAccess.Data;
using meo.DataAccess.Repository;
using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace meo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
           _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objCategory = _UnitOfWork.Product.GetAll().ToList();
           
            return View(objCategory);
        }
        public IActionResult UpSert(int ? id)
        {
            IEnumerable<SelectListItem> obj = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryID.ToString(),
                Value = u.CategoryName

            });

            ProductVM vm = new()
            {
                CategoryList = obj,
                Product = new Product()
            };

            if (id == null || id == 0)
            {

                return View(vm);
            }
            vm.Product = _UnitOfWork.Product.Get(u => u.Id == id);
            return View(vm);


        }

        [HttpPost]
        public IActionResult UpSert(ProductVM obj, IFormFile ? file)
        {

           
            if (ModelState.IsValid)
            {
                _UnitOfWork.Product.Add(obj.Product);
                _UnitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                // for return back if encounter if modelstate is invalid

                obj.CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Value = u.CategoryID.ToString(),
                    Text = u.CategoryName
                });
                return View(obj);
                
                
            }
          

        }

  

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int ? id)
        {
            if(id != 0 && id.HasValue)
            {
                Product Product = _UnitOfWork.Product.Get(u => u.Id == id);

                if (Product != null)
                {
                    _UnitOfWork.Product.Remove(Product);
                    _UnitOfWork.Save();
                    TempData["Success"] = "Delete Success!";
                    return RedirectToAction("Index", "Product");
                }
            }
          
            return NotFound();
            
        }

    }
}
