using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_FinalProject_Team2.Models.PRN221DBContext _db;

        public IndexModel(PRN221_FinalProject_Team2.Models.PRN221DBContext db)
        {
            _db = db;
        }

        public IList<Account> Account { get; set; }
        public async Task OnGetAsync()
        {
            if (_db.Accounts != null)
            {
                Account = await _db.Accounts.ToListAsync();
            }
        }
    }
}
