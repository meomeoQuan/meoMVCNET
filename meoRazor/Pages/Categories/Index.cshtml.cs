using meoRazor.Data;
using meoRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace meoRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Category> objCategory;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            objCategory = _db.categories.ToList();
        }

        //public IActionResult OnPost(int id)
        //{
        //    var obj = _db.categories.Find(id); // Find the category by id
        //    if (obj != null)
        //    {
        //        _db.categories.Remove(obj); // Delete the category
        //        _db.SaveChanges();
        //    }

        //    return RedirectToPage("Index"); // Redirect to Index after deletion
        //}
    }
}
