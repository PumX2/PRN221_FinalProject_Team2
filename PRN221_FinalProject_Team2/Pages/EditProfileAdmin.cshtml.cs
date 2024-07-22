using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages
{
    public class EditProfileAdminModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public EditProfileAdminModel(PRN221DBContext db)
        {
            _db = db;
            Customer = new Customer();
        }
        [BindProperty]
        public Account Account { get; set; }
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }


        public IActionResult OnGet()
        {
            //string? email = HttpContext.Session.GetString("email");
            string? customer = HttpContext.Session.GetString("customer");
            string? admin = HttpContext.Session.GetString("admin");

            if (customer != null || admin == null)
            {
                //CRUD Account

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
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account.Employee.FirstName == null || Account.Employee.LastName == null)
            {
                TempData["err"] = "Please insert FirstName and LastName!!!";
                return Page();
            }
            _db.Attach(Account).State = EntityState.Modified;
            _db.Attach(Account.Employee).State = EntityState.Modified;

            await _db.SaveChangesAsync();
            TempData["msg"] = "You update profile success!";
            return Page();
        }
    }
}