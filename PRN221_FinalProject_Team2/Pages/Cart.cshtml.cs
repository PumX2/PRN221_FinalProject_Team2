using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_FinalProject_Team2.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace PRN221_FinalProject_Team2.Pages
{
    public class CartModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public CartModel(PRN221DBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
        [BindProperty]
        public List<Product> products { get; set; } = default!;
        [BindProperty]
        public decimal totalPrice { get; set; } = 0;
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("Account") == null)
            {
                return RedirectToPage("Login");
            }

            var cartJson = HttpContext.Session.GetString("cart");


            cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson).OrderBy(x => x.ProductId).ToList();
            foreach (var i in cartItems)
            {
                totalPrice += (decimal)(i.Quantity * i.UnitPrice);
            }
            List<int> productIds = cartItems.Select(item => item.ProductId).ToList<int?>().ConvertAll<int>(x => x.GetValueOrDefault());
            List<Product> product = _db.Products.Where(product => productIds.Contains(product.ProductId)).ToList();
            products = product;
            return Page();
        }


        public async Task<IActionResult> OnGetDelete(int? id)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            if (cartJson != null)
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson).OrderBy(x => x.ProductId).ToList();
                CartItem a = cartItems.Find(x => x.ProductId == id);
                cartItems.Remove(a);
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartItems));

                List<int> productIds = cartItems.Select(item => item.ProductId).ToList<int?>().ConvertAll<int>(x => x.GetValueOrDefault());
                List<Product> product = _db.Products.Where(product => productIds.Contains(product.ProductId)).ToList();
                products = product;
                return RedirectToPage("Cart");
            }
            else
            {
                return RedirectToPage("Cart");
            }



        }
        public async Task<IActionResult> OnGetMinus(int? id)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            if (cartJson != null)
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson).OrderBy(x => x.ProductId).ToList();
                CartItem a = cartItems.Find(x => x.ProductId == id);
                if (a.Quantity > 1)
                {
                    cartItems.FirstOrDefault(x => x.ProductId == id).Quantity--;

                }
                else
                {
                    return RedirectToPage("Cart");
                }

                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartItems));

                List<int> productIds = cartItems.Select(item => item.ProductId).ToList<int?>().ConvertAll<int>(x => x.GetValueOrDefault());
                List<Product> product = _db.Products.Where(product => productIds.Contains(product.ProductId)).ToList();
                products = product;
                return RedirectToPage("Cart");
            }
            else
            {
                return RedirectToPage("Cart");
            }



        }
        public async Task<IActionResult> OnGetAdd(int? id)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            if (cartJson != null)
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson).OrderBy(x => x.ProductId).ToList();
                CartItem a = cartItems.Find(x => x.ProductId == id);

                var productrr = _db.Products.FirstOrDefault(x => x.ProductId == id);
                if (a.Quantity + 1 > productrr.UnitsInStock)
                {
                    TempData["msg"] = "The quantity your buy  is more than in stock";
                    return RedirectToPage("Cart");
                }
                cartItems.FirstOrDefault(x => x.ProductId == id).Quantity++;



                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartItems));

                List<int> productIds = cartItems.Select(item => item.ProductId).ToList<int?>().ConvertAll<int>(x => x.GetValueOrDefault());
                List<Product> product = _db.Products.Where(product => productIds.Contains(product.ProductId)).ToList();
                products = product;
                return RedirectToPage("Cart");
            }
            else
            {
                return RedirectToPage("Cart");
            }



        }
    }
}
