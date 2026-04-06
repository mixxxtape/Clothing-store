using System.Security.Claims;
using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ClothingStoreContext _context;

        public ReviewsController(ClothingStoreContext context)
        {
            _context = context;
        }

        private async Task<ClothingStoreMVC.Domain.Entities.UserAggregates.User?> GetCurrentUserAsync()
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
        }

        // POST: /Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewViewModel vm)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid review data";
                return RedirectToAction("Details", "Catalog", new { id = vm.ProductId });
            }

            // Перевірити чи є доставлене замовлення
            var hasDeliveredOrder = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                .AnyAsync(o =>
                    o.Items.Any(i => i.ProductId == vm.ProductId) &&
                    o.StatusHistory.OrderByDescending(s => s.ChangedAt)
                        .First().Status == "Delivered");

            if (!hasDeliveredOrder)
            {
                TempData["Error"] = "You can only review products you have received";
                return RedirectToAction("Details", "Catalog", new { id = vm.ProductId });
            }

            // Перевірити чи вже залишив відгук
            var alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.UserId == user.Id && r.ProductId == vm.ProductId);

            if (alreadyReviewed)
            {
                TempData["Error"] = "You have already reviewed this product";
                return RedirectToAction("Details", "Catalog", new { id = vm.ProductId });
            }

            _context.Reviews.Add(new Review
            {
                UserId = user.Id,
                ProductId = vm.ProductId,
                Rating = vm.Rating,
                Comment = vm.Comment
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Review added";
            return RedirectToAction("Details", "Catalog", new { id = vm.ProductId });
        }

        // POST: /Reviews/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int rating, string? comment, int productId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);

            if (review == null) return NotFound();

            review.Rating = rating;
            review.Comment = comment;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Review updated";
            return RedirectToAction("Details", "Catalog", new { id = productId });
        }

        // POST: /Reviews/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);

            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Review deleted";
            return RedirectToAction("Details", "Catalog", new { id = productId });
        }
    }
}