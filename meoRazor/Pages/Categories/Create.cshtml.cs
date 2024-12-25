using meoRazor.Data;
using meoRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace meoRazor.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        
        public Category category {  get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void  OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            _db.categories.Add(category);
            _db.SaveChanges();
            TempData["Success"] = "Create Success !";
            return RedirectToPage("Index");
        }
    }
}
