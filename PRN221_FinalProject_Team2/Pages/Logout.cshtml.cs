using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRN221_FinalProject_Team2.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("admin");
            HttpContext.Session.Remove("customer");
            HttpContext.Session.Remove("Account");
			HttpContext.Session.Remove("Cart");

			return RedirectToPage("/Index");
        }
    }
}
