using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Users
{
    public class EditModel : PageModel
    {
        private readonly PRN221_FinalProject_Team2.Models.PRN221DBContext _db;

        public EditModel(PRN221_FinalProject_Team2.Models.PRN221DBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Account Account { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts.FirstOrDefaultAsync(m => m.AccountId == id);

            if (account == null)
            {
                return NotFound();
            }
            else
            {
                Account = account;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            _db.Attach(Account).State= EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if(!AccountExists(Account.AccountId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }
        private bool AccountExists(int id)
        {
            return (_db.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
