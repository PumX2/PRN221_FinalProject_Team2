using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
		private readonly PRN221DBContext _db;

		public IndexModel(PRN221DBContext db)
		{
			_db = db;
		}
		public List<Order> Orders { get; set; }
		[BindProperty]
		public string CustomerID { get; set; }

		public int Page { get; set; }

		public int NumberPage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? pageIndex, string? keyword)
		{
            CustomerID = keyword;
			if (HttpContext.Session.GetString("admin") != null)
			{
				int pageSize = 6;
				IList<Customer> customerList = await _db.Customers.ToListAsync();

				pageIndex = pageIndex ?? 1;

				Page = pageIndex.Value;

				if (string.IsNullOrEmpty(CustomerID))
				{
					int total = _db.Orders.Include(c => c.OrderDetails).Count();
					NumberPage = (int)Math.Ceiling((double)total / (double)pageSize);
					Orders = await _db.Orders.Include(c => c.OrderDetails).OrderByDescending(x => x.OrderId).Skip((pageIndex.Value - 1) * pageSize).Take(pageSize).ToListAsync();
					for(int i=0;i<Orders.Count;i++)
					{
						for(int j = 0; j < customerList.Count; j++)
						{
							if (Orders[i].CustomerId == customerList[j].CustomerId)
							{
								Orders[i].Customer = customerList[j];
							}
						}
					}
				}
				else
				{
					int total = _db.Orders.Include(c => c.OrderDetails).Where(C => C.CustomerId== CustomerID).Count();
					NumberPage = (int)Math.Ceiling((double)total / (double)pageSize);
					Orders = await _db.Orders.Include(c => c.OrderDetails).OrderByDescending(x => x.OrderId).Where(C => C.CustomerId == CustomerID).Skip((pageIndex.Value - 1) * pageSize).Take(pageSize).ToListAsync();
				
					for (int i = 0; i < Orders.Count; i++)
					{
						for (int j = 0; j < customerList.Count; j++)
						{
							if (Orders[i].CustomerId == customerList[j].CustomerId)
							{
								Orders[i].Customer = customerList[j];
							}
						}
					}

				}

				return Page();
			}
			else
			{
				return RedirectToPage("/Error");
			}
		}
		
    }
}
