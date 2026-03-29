using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly ClothingStoreContext _context;

        public ProductsController(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .IgnoreQueryFilters()
                .Include(p => p.Category)
                .Include(p => p.Style)
                .Include(p => p.Sizes)
                .ThenInclude(ps => ps.Size)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Style)
                .Include(p => p.Sizes)
                    .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name");
            ViewData["Sizes"] = new MultiSelectList(_context.Sizes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,CategoryId,StyleId,IsDeleted")] Product product, int[] selectedSizes)
        {
            if (ModelState.IsValid)
            {
                foreach (var sizeId in selectedSizes)
                {
                    product.Sizes.Add(new ProductSize { SizeId = sizeId, Quantity = 0 });
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name", product.StyleId);
            ViewData["Sizes"] = new MultiSelectList(_context.Sizes, "Id", "Name", selectedSizes);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Sizes)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name", product.StyleId);
            ViewData["Sizes"] = new MultiSelectList(_context.Sizes, "Id", "Name", product.Sizes.Select(s => s.SizeId));
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId,StyleId,IsDeleted")] Product product, int[] selectedSizes)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var productToUpdate = await _context.Products
                    .Include(p => p.Sizes)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (productToUpdate == null) return NotFound();

                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.Price = product.Price;
                productToUpdate.CategoryId = product.CategoryId;
                productToUpdate.StyleId = product.StyleId;
                productToUpdate.IsDeleted = product.IsDeleted;

                productToUpdate.Sizes.Clear();
                foreach (var sizeId in selectedSizes)
                {
                    productToUpdate.Sizes.Add(new ProductSize { SizeId = sizeId, Quantity = 0 });
                }

                try
                {
                    _context.Update(productToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name", product.StyleId);
            ViewData["Sizes"] = new MultiSelectList(_context.Sizes, "Id", "Name", selectedSizes);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
    .Include(p => p.Category)
    .Include(p => p.Style)
    .Include(p => p.Sizes)
        .ThenInclude(ps => ps.Size)
    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}