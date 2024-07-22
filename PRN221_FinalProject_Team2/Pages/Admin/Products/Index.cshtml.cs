using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Products
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _db;
        [BindProperty]
        public string OrderId { get; set; }

        public IndexModel(PRN221DBContext db)
        {
            _db = db;
        }

        public List<Product> Products { get; set; }

        public int Page { get; set; }

        public int NumberPage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? keyword)
        {
            ViewData["Search"] = keyword;
            if(HttpContext.Session.GetString("admin") != null)
            {
				int pageSize = 6;

				pageIndex = pageIndex ?? 1;

				Page = pageIndex.Value;

                if(string.IsNullOrEmpty(keyword))
                {
					int total = _db.Products.Include(c => c.Category).Count();
					NumberPage = (int)Math.Ceiling((double)total / (double)pageSize);
					Products = await _db.Products.Include(c => c.Category).Skip((pageIndex.Value - 1) * pageSize).Take(pageSize).ToListAsync();
				}
                else
                {
					int total = _db.Products.Include(c => c.Category).Where(p => p.ProductName.Contains(keyword)).Count();
					NumberPage = (int)Math.Ceiling((double)total / (double)pageSize);
					Products = await _db.Products.Include(c => c.Category).Where(p => p.ProductName.Contains(keyword)).Skip((pageIndex.Value - 1) * pageSize).Take(pageSize).ToListAsync();
				}

				return Page();
			}
            else
            {
				return RedirectToPage("/Error");
			}
        }

        public async Task<IActionResult> OnGetDelete(int id)
        {
			var checkExist = _db.OrderDetails.FirstOrDefault(od => od.ProductId == id);
            if(checkExist != null)
            {
                TempData["Exist"] = "Product is currently being used, can't delete the product!";
                return RedirectToPage("Index");
            }
            var product = await _db.Products.FindAsync(id);
            if(product != null)
            {
				TempData["Success"] = "Delete successfully!";
				_db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
