using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PRN221_FinalProject_Team2.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public EditProfileModel(PRN221DBContext db)
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

            if (customer == null || admin != null)
            {
                //CRUD Account

                return RedirectToPage("EditProfileAdmin");
            }
            Account acc = JsonSerializer.Deserialize<Account>(customer);
            Account = _db.Accounts.Include(x => x.Customer).FirstOrDefault(x => x.Email == acc.Email);
            if (Account == null)
            {
                return RedirectToPage("Error");
            }

            Customer = Account.Customer;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account.Customer.CompanyName == null)
            {
                TempData["err"] = "Please insert CompanyName";
                return Page();
            }
            _db.Attach(Account).State = EntityState.Modified;
            _db.Attach(Account.Customer).State = EntityState.Modified;

            await _db.SaveChangesAsync();
            TempData["msg"] = "You update profile success!";
            return Page();
        }
    }
}
