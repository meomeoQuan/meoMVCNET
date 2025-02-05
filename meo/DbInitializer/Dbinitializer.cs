using meo.DataAccess.Data;
using meo.Models;
using meo.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace meo.DbInitializer
{
    public class Dbinitializer : IDbInitializer
    {
        public readonly ApplicationDbContext _db;
        public readonly UserManager<IdentityUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public Dbinitializer(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            // migration if they are not applied

            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


            }

            // create roles if they not created 

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();



                _userManager.CreateAsync(new ApplicationUser
                {
                    name = "Admin",
                    Email = "QUANNADE180924@fpt.edu.vn",
                    UserName = "quan",
                    PhoneNumber = "0913494261",
                    streetAddress = "No.20, 17 My An street, Ngu Hanh Son district, Danang city",
                    state = "Meo",
                    city = "Da Nang",
                    portalCode = "CB001",
                    userImage = "",
                    EmailConfirmed = true

                }, "Neko@123"


               ).GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "QUANNADE180924@fpt.edu.vn");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }


            // create admin  if not created any roles
            return;
           
        }
    }
}