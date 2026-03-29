using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ClothingStoreContext _context;

        public CatalogController(ClothingStoreContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Style)
            .Include(p => p.Sizes)
            .ThenInclude(ps => ps.Size)
            .Select(p => new ProductListViewModel
            {
                 Id = p.Id,
                 Name = p.Name,
                 Price = p.Price,
                 CategoryName = p.Category.Name,
                 StyleName = p.Style.Name,
                 Sizes = string.Join(", ", p.Sizes.Select(s => s.Size.Name))
             })
            .ToListAsync();

            return View(products);
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
    .Select(p => new ProductDetailsViewModel
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        CategoryName = p.Category.Name,
        StyleName = p.Style.Name,
        Sizes = string.Join(", ", p.Sizes.Select(s => s.Size.Name))
    })
    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [Authorize(Roles = "user")]
        public IActionResult AddToCart(int id)
        {
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "user")]
        public IActionResult AddToFavorites(int id)
        {
            return RedirectToAction("Index");
        }
    }
}