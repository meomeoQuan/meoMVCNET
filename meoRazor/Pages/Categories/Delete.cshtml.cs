using meoRazor.Data;
using meoRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace meoRazor.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // Load the category to be deleted
        public void OnGet(int? id)
        {
            if(id.HasValue && id != 0)
            {
                Category = _db.categories.Find(id);
            }

        }

        // Handle the delete action
        public IActionResult OnPost()
        {
          //Category ? obj = _db.categories.Find(Category.categoryID);
            if (Category != null)
            {
                _db.categories.Remove(Category);
                _db.SaveChanges();
                TempData["Success"] = "Delete Success !";
                return RedirectToPage("Index");
            }
          return NotFound();    
        }
    }
}
