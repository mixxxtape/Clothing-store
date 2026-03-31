using System.Security.Claims;
using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly ClothingStoreContext _context;

        public WishlistsController(ClothingStoreContext context)
        {
            _context = context;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
        }

        private async Task<Wishlist> GetOrCreateWishlistAsync(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userId };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return wishlist;
        }

        // GET: /Wishlists
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                    .ThenInclude(p => p.Style)
                .FirstOrDefaultAsync(w => w.UserId == user.Id);

            var vm = new WishlistViewModel();

            if (wishlist != null)
            {
                vm.Items = wishlist.Products.Select(p => new WishlistItemViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    StyleName = p.Style?.Name ?? "—"
                }).ToList();
            }

            return View(vm);
        }

        // POST: /Wishlists/Toggle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int productId, string? returnUrl)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var wishlist = await GetOrCreateWishlistAsync(user.Id);

            // Reload with products
            var wishlistWithProducts = await _context.Wishlists
                .Include(w => w.Products)
                .FirstAsync(w => w.Id == wishlist.Id);

            var product = wishlistWithProducts.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                wishlistWithProducts.Products.Remove(product);
                TempData["Info"] = "Removed from wishlist";
            }
            else
            {
                var prod = await _context.Products.FindAsync(productId);
                if (prod != null)
                {
                    wishlistWithProducts.Products.Add(prod);
                    TempData["Success"] = "Added to wishlist";
                }
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Wishlists/MoveToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            // Знайти перший доступний розмір
            var productSize = await _context.ProductSizes
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.Quantity > 0);

            if (productSize == null)
            {
                TempData["Error"] = "No sizes available";
                return RedirectToAction(nameof(Index));
            }

            // Додати в корзину
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserId = user.Id };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existing = cart.Items.FirstOrDefault(i => i.ProductSizeId == productSize.Id);
            if (existing != null)
                existing.Quantity++;
            else
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    ProductSizeId = productSize.Id,
                    Quantity = 1
                });

            // Видалити з вішліста
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == user.Id);

            if (wishlist != null)
            {
                var prod = wishlist.Products.FirstOrDefault(p => p.Id == productId);
                if (prod != null) wishlist.Products.Remove(prod);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Moved to cart";
            return RedirectToAction(nameof(Index));
        }
    }
}