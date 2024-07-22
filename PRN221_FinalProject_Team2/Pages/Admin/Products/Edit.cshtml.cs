using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Products
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public EditModel(PRN221DBContext db)
        {
            _db = db;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
				ViewData["Category"] = new SelectList(_db.Categories, "CategoryId", "CategoryName");
				Product = await _db.Products.FindAsync(id);
				return Page();
			}
            else
            {
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
				ViewData["Category"] = new SelectList(_db.Categories, "CategoryId", "CategoryName");
                return Page();
			}
            TempData["Success"] = "Update successfully!";
            _db.Products.Update(Product);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
