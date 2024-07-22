using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
		private readonly PRN221DBContext _db;

		public CreateModel(PRN221DBContext db)
		{
			_db = db;
		}

		public Category Category { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			if(HttpContext.Session.GetString("admin") != null)
			{
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
				return Page();
			}
			TempData["Success"] = "Add successfully!";
			await _db.Categories.AddAsync(Category);
			await _db.SaveChangesAsync();
			return RedirectToPage("Index");
		}
	}
}
