using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages
{
    public class DetailModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public DetailModel(PRN221DBContext db)
        {
            _db = db;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (_db.Products == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FirstOrDefaultAsync(a => a.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnGetAddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Account") == null)
            {
                return RedirectToPage("Login");
            }
            else
            {
                var cartJson = HttpContext.Session.GetString("cart");
                var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                var product = _db.Products.FirstOrDefault(x => x.ProductId == id);
                if (cart.FirstOrDefault(x => x.ProductId == id) == null)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = id,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        Quantity = 1
                    });

                    TempData["cartmsg"] = "Add success " + product.ProductName + " to cart!";
                }
                else
                {
                    cart.FirstOrDefault(x => x.ProductId == id).Quantity++;
                    TempData["cartmsg"] = "Add success one more " + product.ProductName + " to cart!";
                }
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            }
            return Redirect("/Detail?id=" + id);
        }
    }
}
