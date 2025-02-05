using meo.DataAccess.Data;
using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace meo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
      

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<ApplicationUser> userobj = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var RoleUser = _db.UserRoles.ToList();
            var Role = _db.Roles.ToList();
            foreach (var obj in userobj)
            {


                var roleId = RoleUser.FirstOrDefault(u => u.UserId == obj.Id).RoleId;
                obj.Role = Role.FirstOrDefault(u => u.Id == roleId).Name;


                if (obj.Company == null)
                {
                    obj.Company = new Company()
                    {
                        CompanyName = ""
                    };
                }
            }
            return Json(new { data = userobj });
        }

       
        public IActionResult LockUnLock([FromBody] string id) {

            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true , message = "Operation success !"});

        }
        #endregion
    }
}
