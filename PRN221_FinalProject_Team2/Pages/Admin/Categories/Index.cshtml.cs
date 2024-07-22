using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Categories
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public IndexModel(PRN221DBContext db)
        {
            _db = db;
        }

        public List<Category> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if(HttpContext.Session.GetString("admin") != null)
            {
				Categories = await _db.Categories.ToListAsync();
				return Page();
			}
            else
            {
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnGetDelete(int id)
        {
            var checkExist = _db.Products.FirstOrDefault(p => p.CategoryId == id);
            if(checkExist != null)
            {
				TempData["Exist"] = "Category is currently being used, can't delete the category!";
                return RedirectToPage("Index");
			}
            var category = await _db.Categories.FindAsync(id);
            if(category != null)
            {
                TempData["Success"] = "Delete successfully!";
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
