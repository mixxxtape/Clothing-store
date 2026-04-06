using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
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
                .IgnoreQueryFilters()
                .Include(p => p.Category)
                .Include(p => p.Style)
                .Include(p => p.Sizes)
                    .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            var sizes = await _context.Sizes.ToListAsync();
            var vm = new ProductCreateViewModel
            {
                Sizes = sizes.Select(s => new ProductSizeInputViewModel
                {
                    SizeId = s.Id,
                    SizeName = s.Name,
                    IsSelected = false,
                    Quantity = 0
                }).ToList()
            };

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Price = vm.Price,
                    StyleId = vm.StyleId,
                    CategoryId = vm.CategoryId,
                    IsDeleted = vm.IsDeleted
                };

                foreach (var size in vm.Sizes.Where(s => s.IsSelected))
                {
                    product.Sizes.Add(new ProductSize
                    {
                        SizeId = size.SizeId,
                        Quantity = size.Quantity
                    });
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var sizes = await _context.Sizes.ToListAsync();
            foreach (var s in vm.Sizes)
                s.SizeName = sizes.FirstOrDefault(x => x.Id == s.SizeId)?.Name ?? "";

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", vm.CategoryId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name", vm.StyleId);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .IgnoreQueryFilters()
                .Include(p => p.Sizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var allSizes = await _context.Sizes.ToListAsync();
            var vm = new ProductCreateViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StyleId = product.StyleId,
                CategoryId = product.CategoryId,
                IsDeleted = product.IsDeleted,
                Sizes = allSizes.Select(s =>
                {
                    var existing = product.Sizes.FirstOrDefault(ps => ps.SizeId == s.Id);
                    return new ProductSizeInputViewModel
                    {
                        SizeId = s.Id,
                        SizeName = s.Name,
                        IsSelected = existing != null,
                        Quantity = existing?.Quantity ?? 0
                    };
                }).ToList()
            };

            ViewData["ProductId"] = id;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name", product.StyleId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products
                    .IgnoreQueryFilters()
                    .Include(p => p.Sizes)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null) return NotFound();

                product.Name = vm.Name;
                product.Description = vm.Description;
                product.Price = vm.Price;
                product.StyleId = vm.StyleId;
                product.CategoryId = vm.CategoryId;
                product.IsDeleted = vm.IsDeleted;

                var usedSizeIds = await _context.OrderItems
     .Select(oi => oi.ProductSizeId)
     .ToListAsync();

                var selectedSizeIds = vm.Sizes
                    .Where(s => s.IsSelected)
                    .Select(s => s.SizeId)
                    .ToList();

                var sizesToRemove = product.Sizes
                    .Where(ps => !usedSizeIds.Contains(ps.Id) && !selectedSizeIds.Contains(ps.SizeId))
                    .ToList();
                _context.ProductSizes.RemoveRange(sizesToRemove);

                foreach (var size in vm.Sizes.Where(s => s.IsSelected))
                {
                    var existing = product.Sizes.FirstOrDefault(ps => ps.SizeId == size.SizeId);
                    if (existing != null)
                    {
                        existing.Quantity = size.Quantity;
                    }
                    else
                    {
                        product.Sizes.Add(new ProductSize
                        {
                            SizeId = size.SizeId,
                            Quantity = size.Quantity
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var sizes = await _context.Sizes.ToListAsync();
            foreach (var s in vm.Sizes)
                s.SizeName = sizes.FirstOrDefault(x => x.Id == s.SizeId)?.Name ?? "";

            ViewData["ProductId"] = id;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", vm.CategoryId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "Id", "Name", vm.StyleId);
            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .IgnoreQueryFilters()
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