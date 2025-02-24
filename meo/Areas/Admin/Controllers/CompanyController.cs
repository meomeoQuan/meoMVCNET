using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace meo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }

        public ActionResult Create()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id.HasValue && id != 0)
            {
                company = _unitOfWork.Company.Get(u => u.CompanyID == id);
            }
           
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // avoid Cross-Site Request Forgery (CSRF) attacks
        public IActionResult Upsert(Company company,IFormFile ? file)
        {
            if (ModelState.IsValid)
            {

              string webRootPath = _webHostEnvironment.WebRootPath;


                if (file != null)
                {
                    // k2h13u5hiu12y3413kj4134.jpg
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string companyPath = Path.Combine(webRootPath, @"images\company");
                    // :c:\wwwroot\images\company
                    if (!string.IsNullOrEmpty(company.companyImage)) {

                        //retrie in dbs the image path then take the path and delete the file if it already exist 

                        string oldPath = Path.Combine(webRootPath, company.companyImage.TrimStart('\\'));
                        if(System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    
                    }

                    // c:\wwwroot\images\company\k2h13u5hiu12y3413kj4134.jpg
                    using (var fileStream = new FileStream(Path.Combine(companyPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    company.companyImage = Path.Combine(@"\images\company\", fileName);
                    // \images\company\k2h13u5hiu12y3413kj4134.jpg -> this is the path that will be saved in the database

                }


                if (company.CompanyID == 0)
                {
                    // Create new company
                    _unitOfWork.Company.Add(company);
                    TempData["Success"] = "Company created successfully.";
                }
                else
                {
                    // Update existing company
                    _unitOfWork.Company.Update(company);
                    TempData["Success"] = "Company updated successfully.";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }

            // Return the view with the current model to show validation errors
            return View(company);
        }










        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Company> companyobj = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = companyobj });
        }

       
        public IActionResult Delete(int ? id) { 
        
            Company company = _unitOfWork.Company.Get(u => u.CompanyID == id);
            if (company == null) { 
            return Json(new { success = false , message = "Deleting fail"});
            }
            if(!string.IsNullOrEmpty(company.companyImage))
            {
                string oldFile = Path.Combine(_webHostEnvironment.WebRootPath, company.companyImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete success !" });
        }
        #endregion
    }
}
