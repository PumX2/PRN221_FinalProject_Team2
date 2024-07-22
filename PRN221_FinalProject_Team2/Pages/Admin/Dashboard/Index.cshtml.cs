using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Globalization;

namespace PRN221_FinalProject_Team2.Pages.Admin.Dashboard
{
    public class QuantityCount
    {
        public string? Name { get; set; }
        public int Count { get; set; }
    }

    public class PriceCount
    {
        public string? Name { get; set; }
        public decimal Total { get; set; }
    }

    public class Revenue
    {
        public string? Month { get; set; }
        public decimal Total { get; set; }
    }

    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public IndexModel(PRN221DBContext db)
        {
            _db = db;
        }

        public List<QuantityCount> listQuantity { get; set; }

        public List<PriceCount> listPrice { get; set; }

        public List<Revenue> listRevenue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? year)
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
                ViewData["Year"] = year;

                //Get top 10 by quantity
                List<QuantityCount> productsInQuantity = new List<QuantityCount>();

                var top10Quanity = (from od in _db.OrderDetails
                                     join p in _db.Products on od.ProductId equals p.ProductId
                                     group od by new { od.ProductId, p.ProductName, od.UnitPrice } into odGroup
                                     orderby odGroup.Sum(od => od.Quantity) descending
                                     select new
                                     {
                                         ProductId = odGroup.Key.ProductId,
                                         ProductName = odGroup.Key.ProductName,
                                         TotalQuantity = odGroup.Sum(od => od.Quantity),
                                         TotalPrice = odGroup.Sum(od => od.Quantity) * odGroup.Key.UnitPrice
                                     }).Take(10);

                foreach (var item in top10Quanity)
                {
                    productsInQuantity.Add(new QuantityCount()
                    {
                        Name = item.ProductName,
                        Count = item.TotalQuantity
                    });
                }

                listQuantity = productsInQuantity;


                //Get top 10 by price
                List<PriceCount> productsInPrice = new List<PriceCount>();

                var top10Price = (from od in _db.OrderDetails
                                     join p in _db.Products on od.ProductId equals p.ProductId
                                     group od by new { od.ProductId, p.ProductName, od.UnitPrice } into odGroup
                                     orderby (odGroup.Sum(od => od.Quantity) * odGroup.Key.UnitPrice) descending
                                     select new
                                     {
                                         ProductId = odGroup.Key.ProductId,
                                         ProductName = odGroup.Key.ProductName,
                                         TotalQuantity = odGroup.Sum(od => od.Quantity),
                                         TotalPrice = odGroup.Sum(od => od.Quantity) * odGroup.Key.UnitPrice
                                     }).Take(10);

                foreach(var item in top10Price)
                {
                    productsInPrice.Add(new PriceCount()
                    {
                        Name = item.ProductName,
                        Total = item.TotalPrice
                    });
                }

                listPrice = productsInPrice;

                //Revenue
                List<Revenue> productsInRevenue = new List<Revenue>();

                var revenue = from m in Enumerable.Range(1, 12)
                              join sales in (
                                  from o in _db.Orders
                                  where o.OrderDate.HasValue && o.OrderDate.Value.Year == (year == null ? DateTime.Now.Year : year) // Change to the specific year you want to query
                                  from od in o.OrderDetails
                                  select new { Month = o.OrderDate.Value.Month, Total = od.UnitPrice * od.Quantity }
                              ) on m equals sales.Month into g
                              from sales in g.DefaultIfEmpty()
                              group sales by m into monthlySales
                              orderby monthlySales.Key ascending
                              select new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthlySales.Key), Total = monthlySales.Sum(x => x?.Total ?? 0) };

                foreach(var item in revenue)
                {
                    productsInRevenue.Add(new Revenue()
                    {
                        Month = item.Month,
                        Total = item.Total
                    });
                }

                listRevenue = productsInRevenue;

                return Page();
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
    }
}