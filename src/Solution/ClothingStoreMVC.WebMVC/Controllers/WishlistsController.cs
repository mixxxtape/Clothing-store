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
            return RedirectToAction("Details", "Catalog", new { id = productId });
        }
    }
    }
