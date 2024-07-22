using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Users
{
    public class CreateModel : PageModel
    {
        private readonly PRN221_FinalProject_Team2.Models.PRN221DBContext _db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        
        public CreateModel(PRN221_FinalProject_Team2.Models.PRN221DBContext db,Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _db= db;
            _environment= environment;
        }


        public IActionResult OnGet() 
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Account.Role = 2;
            _db.Accounts.Add(Account);
            await _db.SaveChangesAsync();
            return RedirectToPage("./Index");

            

        }
    }
}
