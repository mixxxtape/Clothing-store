using System.Security.Claims;
using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ClothingStoreContext _context;
        private readonly IdentityContext _identityContext;

        public OrdersController(ClothingStoreContext context, IdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }


        private async Task<User?> GetCurrentUserAsync()
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductSize)
                        .ThenInclude(ps => ps.Size)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var vms = orders.Select(o => new OrderViewModel
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                DeliveryAddress = o.DeliveryAddress,
                CurrentStatus = o.StatusHistory
                    .OrderByDescending(s => s.ChangedAt)
                    .FirstOrDefault()?.Status ?? "Pending",
                Items = o.Items.Select(i => new OrderItemViewModel
                {
                    ProductName = i.ProductName,
                    SizeName = i.ProductSize?.Size?.Name ?? "—",
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            }).ToList();

            return View(vms);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Where(o => o.Id == id && o.UserId == user.Id)
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductSize)
                        .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync();

            if (order == null) return NotFound();

            var vm = new OrderViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                DeliveryAddress = order.DeliveryAddress,
                CurrentStatus = order.StatusHistory
                    .OrderByDescending(s => s.ChangedAt)
                    .FirstOrDefault()?.Status ?? "Pending",
                Items = order.Items.Select(i => new OrderItemViewModel
                {
                    ProductName = i.ProductName,
                    SizeName = i.ProductSize?.Size?.Name ?? "—",
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            return View(vm);
        }

        // GET: /Orders/Checkout
        public async Task<IActionResult> Checkout()
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

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Carts");
            }

            var identityUser = await _identityContext.Set<AppUser>()
                .FirstOrDefaultAsync(u => u.Id == user.IdentityUserId);

            var vm = new CheckoutViewModel
            {
                DeliveryAddress = identityUser?.DefaultAddress ?? "",
                Items = cart.Items.Select(i => new CartItemViewModel
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    SizeName = i.ProductSize?.Size?.Name ?? "—",
                    ProductSizeId = i.ProductSizeId,
                    Quantity = i.Quantity
                }).ToList(),
                Total = cart.Items.Sum(i => i.Product.Price * i.Quantity)
            };

            return View(vm);
        }

        // POST: /Orders/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel vm)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .Include(c => c.Items)
                    .ThenInclude(i => i.ProductSize)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Carts");
            }

            if (!ModelState.IsValid)
            {
                vm.Items = cart.Items.Select(i => new CartItemViewModel
                {
                    CartItemId = i.Id,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    SizeName = i.ProductSize?.Size?.Name ?? "—",
                    Quantity = i.Quantity
                }).ToList();
                vm.Total = vm.Items.Sum(i => i.Subtotal);
                return View(vm);
            }

            foreach (var item in cart.Items)
            {
                if (item.ProductSize.Quantity < item.Quantity)
                {
                    TempData["Error"] = $"Not enough stock for {item.Product.Name}";
                    return RedirectToAction("Index", "Carts");
                }
            }

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                DeliveryAddress = vm.DeliveryAddress,
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,   
                    ProductSizeId = i.ProductSizeId,
                    Quantity = i.Quantity
                }).ToList(),
                StatusHistory = new List<OrderStatus>
                {
                    new OrderStatus { Status = "Pending", ChangedAt = DateTime.UtcNow }
                }
            };

            foreach (var item in cart.Items)
                item.ProductSize.Quantity -= item.Quantity;

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        // POST: /Orders/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductSize)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == user.Id);

            if (order == null) return NotFound();

            var currentStatus = order.StatusHistory
                .OrderByDescending(s => s.ChangedAt)
                .FirstOrDefault()?.Status;

            if (currentStatus != "Pending")
            {
                TempData["Error"] = "Cannot cancel this order";
                return RedirectToAction(nameof(Details), new { id });
            }

            foreach (var item in order.Items)
                item.ProductSize.Quantity += item.Quantity;

            order.StatusHistory.Add(new OrderStatus
            {
                Status = "Cancelled",
                ChangedAt = DateTime.UtcNow,
                ChangeReason = "Cancelled by user"
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Order cancelled";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Orders/Manage
        public async Task<IActionResult> Manage()
        {
            if (!User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");

            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductSize)
                        .ThenInclude(ps => ps.Size)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var identityIds = orders.Select(o => o.User.IdentityUserId).Distinct().ToList();
            var appUsers = await _identityContext.Set<AppUser>()
                .Where(u => identityIds.Contains(u.Id))
                .ToListAsync();

            var vms = orders.Select(o =>
            {
                var appUser = appUsers.FirstOrDefault(u => u.Id == o.User.IdentityUserId);
                return new AdminOrderViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    DeliveryAddress = o.DeliveryAddress,
                    UserName = appUser != null
                        ? $"{appUser.FirstName} {appUser.LastName} ({appUser.Email})"
                        : "Unknown",
                    CurrentStatus = o.StatusHistory
                        .OrderByDescending(s => s.ChangedAt)
                        .FirstOrDefault()?.Status ?? "Pending",
                    Items = o.Items.Select(i => new OrderItemViewModel
                    {
                        ProductName = i.ProductName,
                        SizeName = i.ProductSize?.Size?.Name ?? "—",
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                };
            }).ToList();

            return View(vms);
        }

        // POST: /Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string status, string? reason)
        {
            if (!User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");

            var order = await _context.Orders
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductSize)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            if (status == "Cancelled")
            {
                var currentStatus = order.StatusHistory
                    .OrderByDescending(s => s.ChangedAt)
                    .FirstOrDefault()?.Status;

                if (currentStatus != "Cancelled")
                {
                    foreach (var item in order.Items)
                        item.ProductSize.Quantity += item.Quantity;
                }
            }

            order.StatusHistory.Add(new OrderStatus
            {
                Status = status,
                ChangedAt = DateTime.UtcNow,
                ChangeReason = reason
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Status updated to {status}";
            return RedirectToAction(nameof(Manage));
        }

        // POST: /Orders/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");

            var order = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            _context.OrderStatuses.RemoveRange(order.StatusHistory);
            _context.OrderItems.RemoveRange(order.Items);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Order #{id} deleted";
            return RedirectToAction(nameof(Manage));
        }
    }
}