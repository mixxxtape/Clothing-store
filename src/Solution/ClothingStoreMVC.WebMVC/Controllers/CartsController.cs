using System.Security.Claims;
using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{

    public class CartsController : Controller
    {
        private readonly ClothingStoreContext _context;

        public CartsController(ClothingStoreContext context)
        {
            _context = context;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // GET: /Carts
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .Include(c => c.Items)
                    .ThenInclude(i => i.ProductSize)
                        .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            var vm = new CartViewModel { CartId = cart?.Id ?? 0 };

            if (cart != null)
            {
                vm.Items = cart.Items.Select(i => new CartItemViewModel
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    SizeName = i.ProductSize.Size.Name,
                    ProductSizeId = i.ProductSizeId,
                    Quantity = i.Quantity
                }).ToList();
            }

            return View(vm);
        }

        // POST: /Carts/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int? productSizeId, int quantity = 1)
        {
            if (productSizeId == null || productSizeId == 0)
            {
                TempData["Error"] = "Please select a size before adding to cart";
                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer))
                    return Redirect(referer);
                return RedirectToAction("Index", "Catalog");
            }

            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var productSize = await _context.ProductSizes.FindAsync(productSizeId);
            if (productSize == null) return NotFound();

            if (productSize.Quantity < quantity)
            {
                TempData["Error"] = "Not enough stock";
                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer))
                    return Redirect(referer);
                return RedirectToAction("Index", "Catalog");
            }

            var cart = await GetOrCreateCartAsync(user.Id);

            var existing = cart.Items.FirstOrDefault(i => i.ProductSizeId == productSizeId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productSize.ProductId,
                    ProductSizeId = productSize.Id,
                    Quantity = quantity
                });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Added to cart";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Carts/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item == null) return NotFound();

            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: /Carts/RemoveItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Carts/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}