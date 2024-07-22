using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages
{
    public class LoginModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public LoginModel(PRN221DBContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Account Account { get; set; }
        public IActionResult OnGet()
        {
            if(HttpContext.Session.GetString("admin") != null || HttpContext.Session.GetString("customer") != null)
            {
                return RedirectToPage("./Admin/Categories/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var member = await _db.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Account.Email) && a.Password.Equals(Account.Password));
            if (member != null)
            {
                HttpContext.Session.SetString("Account", JsonSerializer.Serialize(member));
                var cart = new List<CartItem>();
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
                if (member.Role == 1)
                {
                    HttpContext.Session.SetString("admin", JsonSerializer.Serialize(member));
                    return RedirectToPage("./Admin/Orders/Index");
                }
                if (member.Role == 2)
                {
                    HttpContext.Session.SetString("customer", JsonSerializer.Serialize(member));
                    HttpContext.Session.SetString("email", member.Email);
                    return RedirectToPage("/Index");
                }
            }

            TempData["msg"] = "Email or password invalid.";
            return Page();

        }
    }
}
