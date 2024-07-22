using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages.Member.Orders
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _context;


        public List<Customer> listc;
        public IndexModel(PRN221DBContext dbcontext)
        {
            _context = dbcontext;
            listc = _context.Customers.ToList();


        }
        [BindProperty]
        public int Page { get; set; }
        [BindProperty]
        public int NumberPage { get; set; }
        public Account acc { get; set; }

        [BindProperty]
        public string StartDate { get; set; }
        [BindProperty]
        public string EndDate { get; set; }

        [BindProperty]
        public List<Order> orders { get; set; }
        [BindProperty]
        public List<OrderDetail> orderdetail { get; set; }
        public int unitpageSize = 6;
        public async Task<IActionResult> OnGetAsync(int? pageIndex, string d1, string d2)
        {

          


            StartDate = d1;
            EndDate = d2;

            string data = HttpContext.Session.GetString("Account");
            pageIndex = pageIndex ?? 1;
            Page = pageIndex.Value;
            if (data != null)
            {
                Account acount = JsonSerializer.Deserialize<Account>(data);
                if (acount != null)
                {
                    if (acount.CustomerId != null)
                    {

                        acc = acount;
                        var oderdetails = await _context.OrderDetails.Include(x => x.Order).Include(x => x.Product).
                            Where(x => x.Order.CustomerId == acount.CustomerId).
                            OrderByDescending(x => x.Order.OrderDate).ToListAsync();

                        orderdetail = oderdetails;
                        if (string.IsNullOrEmpty(d1) && string.IsNullOrEmpty(d2))
                        {
                            var orderSort = await _context.Orders
                           .Where(x => x.CustomerId == acount.CustomerId)
                           .OrderByDescending(x => x.OrderDate)
                            .ToListAsync();
                            int total = orderSort.Count;
                            NumberPage = (int)Math.Ceiling((double)total / (double)unitpageSize);

                            orders = await _context.Orders
                           .Where(x => x.CustomerId == acount.CustomerId)
                           .OrderByDescending(x => x.OrderDate).Skip((pageIndex.Value - 1) * unitpageSize).Take(unitpageSize)
                            .ToListAsync();

                           
                        }
                        if (string.IsNullOrEmpty(d1) && !string.IsNullOrEmpty(d2))
                        {

                         
                            
                            var orderSort = await _context.Orders
                            .Where(x => x.CustomerId == acount.CustomerId && x.OrderDate <= DateTime.Parse(d2))
                            .OrderByDescending(x => x.OrderDate)
                            .ToListAsync();
                            int total = orderSort.Count;
                            NumberPage = (int)Math.Ceiling((double)total / (double)unitpageSize);
                            
                            orders = await _context.Orders
                           .Where(x => x.CustomerId == acount.CustomerId && x.OrderDate <= DateTime.Parse(d2))
                           .OrderByDescending(x => x.OrderDate).Skip((pageIndex.Value - 1) * unitpageSize).Take(unitpageSize)
                            .ToListAsync();

                        }
                        if (!string.IsNullOrEmpty(d1) && string.IsNullOrEmpty(d2))
                        {
                            
                            var orderSort = await _context.Orders
                            .Where(x => x.CustomerId == acount.CustomerId && x.OrderDate >= DateTime.Parse(StartDate))
                            .OrderByDescending(x => x.OrderDate)
                            .ToListAsync();
                            int total = orderSort.Count;
                            NumberPage = (int)Math.Ceiling((double)total / (double)unitpageSize);

                            orders = await _context.Orders
                           .Where(x => x.CustomerId == acount.CustomerId && x.OrderDate >= DateTime.Parse(StartDate))
                           .OrderByDescending(x => x.OrderDate).Skip((pageIndex.Value - 1) * unitpageSize).Take(unitpageSize)
                            .ToListAsync();
                        }
                        if (!string.IsNullOrEmpty(d1) && !string.IsNullOrEmpty(d2))
                        {
                            

                            var orderSort = await _context.Orders
                            .Where(x => x.CustomerId == acount.CustomerId && x.OrderDate <= DateTime.Parse(EndDate) && x.OrderDate >= DateTime.Parse(StartDate))
                            .OrderByDescending(x => x.OrderDate)
                            .ToListAsync();
                            int total = orderSort.Count;
                            NumberPage = (int)Math.Ceiling((double)total / (double)unitpageSize);

                            orders = await _context.Orders
                           .Where(x => x.CustomerId == acount.CustomerId && x.OrderDate <= DateTime.Parse(EndDate) && x.OrderDate >= DateTime.Parse(StartDate))
                           .OrderByDescending(x => x.OrderDate).Skip((pageIndex.Value - 1) * unitpageSize).Take(unitpageSize)
                            .ToListAsync();

                        }
                       
                        return Page();

                    }
                    
                }




            }

            return RedirectToPage("/Error");

        }

    }

}
