using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages
{
    public class ChangePasswordAdminModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public ChangePasswordAdminModel(PRN221DBContext db)
        {
            _db = db;
            Employee = new Employee();
        }
        [BindProperty]
        public Account Account { get; set; }
        public Employee Employee { get; set; }

        public IActionResult OnGet()
        {
            string? customer = HttpContext.Session.GetString("customer");
            string? admin = HttpContext.Session.GetString("admin");
            if (customer != null || admin == null)
            {
                return RedirectToPage("Error");
            }
            Account acc = JsonSerializer.Deserialize<Account>(admin);
            Account = _db.Accounts.Include(x => x.Employee).FirstOrDefault(x => x.Email == acc.Email);
            if (Account == null)
            {
                return RedirectToPage("Error");
            }

            Employee = Account.Employee;
            return Page();
        }

        public IActionResult OnPost(string old, string New, string confirm)
        {
            ViewData["title"] = "Change password";
            string? admin = HttpContext.Session.GetString("admin");

            Account acc = JsonSerializer.Deserialize<Account>(admin);
            Account? account = _db.Accounts.SingleOrDefault(account => account.EmployeeId == acc.EmployeeId);
            if (old == null || New == null || confirm == null)
            {
                ViewData["message"] = "Invalid password";
            }
            else if (old.Length > 50 || New.Length > 50 || confirm.Length > 50)
            {
                ViewData["message"] = "Password max 50 characters";
            }
            else if (!old.Equals(account.Password))
            {
                ViewData["message"] = "Your old password not correct";
            }
            else if (!confirm.Equals(New))
            {
                ViewData["message"] = "Your confirm password not the same new password";
            }
            else
            {
                account.Password = New;
                _db.Accounts.Update(account);
                int number = _db.SaveChanges();
                if (number > 0)
                {
                    ViewData["mess"] = "Update successful";
                }
            }

            return Page();
        }
    }
}
