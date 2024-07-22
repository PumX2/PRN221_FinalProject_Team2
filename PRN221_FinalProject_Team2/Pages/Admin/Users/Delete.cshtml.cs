using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Users
{
    public class DeleteModel : PageModel
    {
        private readonly PRN221_FinalProject_Team2.Models.PRN221DBContext _db;

        public DeleteModel(PRN221_FinalProject_Team2.Models.PRN221DBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Account Account { get; set; }
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
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }
            var account = await _db.Accounts.FindAsync(id);

            if (account != null)
            {
                Account = account;
                _db.Accounts.Remove(Account);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
