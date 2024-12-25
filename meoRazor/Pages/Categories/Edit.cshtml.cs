using meoRazor.Data;
using meoRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace meoRazor.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public Category? Category { get; set; }
        public void OnGet(int? id)
        {
            if (id.HasValue && id != 0)
            {
                Category = _db.categories.Find(id);
            }

        }

        // Handle the delete action
        public IActionResult OnPost()
        {
           
            if (ModelState.IsValid)
            {
                _db.categories.Update(Category);
                _db.SaveChanges();
                TempData["Success"] = "Edit Success !";
                return RedirectToPage("Index");
            }
            return NotFound();
        }
    }
}
