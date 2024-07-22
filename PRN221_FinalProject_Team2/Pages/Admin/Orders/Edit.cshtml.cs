using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {

        private readonly PRN221_FinalProject_Team2.Models.PRN221DBContext _db;

        public EditModel(PRN221_FinalProject_Team2.Models.PRN221DBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
          if(HttpContext.Session.GetString("admin")!= null)
            {
                if (id == null || _db.Orders == null)
                {
                    return NotFound();
                }
                IList<Customer> customerList = await _db.Customers.ToListAsync();
                var order = await _db.Orders.FirstOrDefaultAsync(m => m.OrderId == id);


                if (order == null)
                {
                    return NotFound();
                }
                else
                {
                    for (int j = 0; j < customerList.Count; j++)
                    {
                        if (order.CustomerId == customerList[j].CustomerId)
                        {
                            order.Customer = customerList[j];
                        }
                    }
                    Order = order;
                }
                return Page();
            }
            return RedirectToPage("/Error");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userData = HttpContext.Session.GetString("Account");
            var acc = JsonSerializer.Deserialize<Account>(userData);
            Order.EmployeeId = acc.EmployeeId;
            _db.Attach(Order).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.OrderId))
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


        private bool OrderExists(int id)
        {
            return (_db.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
      
    }
}
