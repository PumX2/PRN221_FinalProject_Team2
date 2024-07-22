using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public IndexModel(PRN221DBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<Product> products { get; set; } = new List<Product>();

        [BindProperty]
        public int page { get; set; } = 1;

        [BindProperty]
        public int numberPage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex = 1)
        {
            int pageSize = 8;
            page = pageIndex.Value;
            int total = _db.Products
                .Where(x => x.Discontinued == false)
                .Count();
            numberPage = (int)Math.Ceiling((double)total / (double)pageSize);
            products = _db.Products
                .Include(x => x.Category)
                .Where(x => x.Discontinued == false)
                .Skip((pageIndex.Value - 1) * pageSize)
                .Take(pageSize)                
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetAddToCart(int? pid, int pageindex)
        {
            if (HttpContext.Session.GetString("Account") == null)
            {
                return RedirectToPage("Login");
            }
            else
            {
                var cartJson = HttpContext.Session.GetString("cart");
                var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                var product = _db.Products.FirstOrDefault(x => x.ProductId == pid);
                if (cart.FirstOrDefault(x => x.ProductId == pid) == null)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = pid,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        Quantity = 1
                    });

                    TempData["cartmsg"] = "Add success " + product.ProductName + " to cart!";
                }
                else
                {
                    cart.FirstOrDefault(x => x.ProductId == pid).Quantity++;
                    TempData["cartmsg"] = "Add success one more " + product.ProductName + " to cart!";
                }
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            }

            return Redirect("/Index?pageIndex="+pageindex);
        }
    }
}