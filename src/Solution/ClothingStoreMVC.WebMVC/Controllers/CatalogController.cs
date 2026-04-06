using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ClothingStoreContext _context;
        private readonly IdentityContext _identityContext;

        public CatalogController(ClothingStoreContext context, IdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string? search,
            string? category,
            string? style,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var allProducts = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Style)
                .Include(p => p.Sizes)
                    .ThenInclude(ps => ps.Size)
                .ToListAsync();

            var absoluteMin = allProducts.Any() ? allProducts.Min(p => p.Price) : 0;
            var absoluteMax = allProducts.Any() ? allProducts.Max(p => p.Price) : 10000;

            var filtered = allProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(category))
                filtered = filtered.Where(p => p.Category.Name == category);

            if (!string.IsNullOrWhiteSpace(style))
                filtered = filtered.Where(p => p.Style.Name == style);

            if (minPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.ToLower().Trim();
                filtered = filtered.Where(p =>
                    FuzzyMatch(p.Name.ToLower(), q) ||
                    FuzzyMatch(p.Description.ToLower(), q) ||
                    FuzzyMatch(p.Category.Name.ToLower(), q) ||
                    FuzzyMatch(p.Style.Name.ToLower(), q));
            }

            var vm = new CatalogFilterViewModel
            {
                Products = filtered.Select(p => new ProductListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    StyleName = p.Style.Name,
                    Sizes = string.Join(", ", p.Sizes.Select(s => s.Size.Name))
                }).ToList(),
                Categories = allProducts.Select(p => p.Category.Name).Distinct().OrderBy(x => x).ToList(),
                Styles = allProducts.Select(p => p.Style.Name).Distinct().OrderBy(x => x).ToList(),
                SearchQuery = search,
                SelectedCategory = category,
                SelectedStyle = style,
                MinPrice = minPrice ?? absoluteMin,
                MaxPrice = maxPrice ?? absoluteMax,
                AbsoluteMinPrice = absoluteMin,
                AbsoluteMaxPrice = absoluteMax
            };

            return View(vm);
        }

        private bool FuzzyMatch(string text, string query)
        {
            if (text.Contains(query)) return true;

            var queryWords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var textWords = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var qWord in queryWords)
            {
                foreach (var tWord in textWords)
                {
                    if (LevenshteinDistance(qWord, tWord) <= Math.Max(1, qWord.Length / 4))
                        return true;
                }
            }
            return false;
        }

        private int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b?.Length ?? 0;
            if (string.IsNullOrEmpty(b)) return a.Length;

            var dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost);
                }
            return dp[a.Length, b.Length];
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Style)
                .Include(p => p.Sizes)
                    .ThenInclude(ps => ps.Size)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = identityId != null
                ? await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId)
                : null;

            bool canReview = false;
            bool alreadyReviewed = false;

            if (currentUser != null)
            {
                var hasDeliveredOrder = await _context.Orders
                    .Where(o => o.UserId == currentUser.Id)
                    .Include(o => o.StatusHistory)
                    .Include(o => o.Items)
                    .AnyAsync(o =>
                        o.Items.Any(i => i.ProductId == id) &&
                        o.StatusHistory.OrderByDescending(s => s.ChangedAt)
                            .First().Status == "Delivered");

                canReview = hasDeliveredOrder;
                alreadyReviewed = product.Reviews.Any(r => r.UserId == currentUser.Id);
            }

            var userIds = product.Reviews.Select(r => r.User.IdentityUserId).Distinct().ToList();
            var appUsers = await _identityContext.Set<AppUser>()
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var vm = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name,
                StyleName = product.Style.Name,
                Sizes = string.Join(", ", product.Sizes.Select(s => s.Size.Name)),
                SizeOptions = product.Sizes
                    .Where(ps => ps.Quantity > 0)
                    .Select(ps => new SizeOptionViewModel
                    {
                        ProductSizeId = ps.Id,
                        SizeName = ps.Size.Name,
                        Quantity = ps.Quantity
                    }).ToList(),
                AverageRating = product.Reviews.Any()
                    ? Math.Round(product.Reviews.Average(r => r.Rating), 1)
                    : 0,
                CanReview = canReview,
                AlreadyReviewed = alreadyReviewed,
                Reviews = product.Reviews.Select(r =>
                {
                    var appUser = appUsers.FirstOrDefault(u => u.Id == r.User.IdentityUserId);
                    return new ReviewViewModel
                    {
                        Id = r.Id,
                        AuthorName = appUser != null
                            ? $"{appUser.FirstName} {appUser.LastName}"
                            : "User",
                        Rating = r.Rating,
                        Comment = r.Comment,
                        IsOwner = currentUser != null && r.UserId == currentUser.Id
                    };
                }).ToList()
            };

            return View(vm);
        }


    }
}