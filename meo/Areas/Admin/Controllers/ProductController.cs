
using meo.DataAccess.Data;
using meo.DataAccess.Repository;
using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Models.ViewModels;
using meo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace meo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
      
        // for saving file image and pass in wwwroot
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objCategory = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
           
            return View(objCategory);
        }
        public IActionResult UpSert(int ? id)
        {
            IEnumerable<SelectListItem> obj = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Value = u.CategoryID.ToString(),
                Text = u.CategoryName
            });

            ProductVM vm = new ProductVM()
            {
                CategoryList = obj,
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                return View(vm);
            }
            vm.Product = _UnitOfWork.Product.Get(u => u.Id == id);
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpSert(ProductVM obj, IFormFile ? file)
        {

            // Handle ImageUrl validation
            if (file == null && string.IsNullOrEmpty(obj.Product.ImageUrl))
            {
                ModelState.AddModelError("Product.ImageUrl", "Please upload an image.");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    //---------------------------------------------------------------------

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //----------------------------------------------------------------------

                    using(var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName;
                  
                }

                if(obj.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(obj.Product);
                    TempData["Success"] = "Add Successfully !";
                }
                else
                {
                    _UnitOfWork.Product.Update(obj.Product);
                    TempData["Success"] = "Update Successfully !";
                }

               
                _UnitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                obj.CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Value = u.CategoryID.ToString(),
                    Text = u.CategoryName

                });

                return View(obj);
            }

            
        }

  

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(int ? id)
        //{
        //    if(id != 0 && id.HasValue)
        //    {
        //        Product Product = _UnitOfWork.Product.Get(u => u.Id == id);

        //        if (Product != null)
        //        {
        //            _UnitOfWork.Product.Remove(Product);
        //            _UnitOfWork.Save();
        //            TempData["Success"] = "Delete Success!";
        //            return RedirectToAction("Index", "Product");
        //        }
        //    }
          
        //    return NotFound();
            
        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }


       
        public IActionResult Delete(int ? id)
        {
            var productTobeDeleted = _UnitOfWork.Product.Get(u => u.Id == id);
            if(productTobeDeleted == null)
            {
                return Json(new { success = false , message = "Error while deleting "});
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,productTobeDeleted.ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _UnitOfWork.Product.Remove(productTobeDeleted);
            TempData["Success"] = "Delete Successful !";
            _UnitOfWork.Save();

            return Json(new { success = true, message = "Delete successful !" });
        }

        #endregion

    }
}
