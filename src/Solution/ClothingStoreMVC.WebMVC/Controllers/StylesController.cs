using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    [Authorize]
    public class StylesController : Controller
    {
        private readonly ClothingStoreContext _context;

        public StylesController(ClothingStoreContext context)
        {
            _context = context;
        }

        // GET: Styles
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Styles.ToListAsync());
        }

        // GET: Styles/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var style = await _context.Styles
                .Include(s => s.Products)
                    .ThenInclude(p => p.Category)
                .Include(s => s.Products)
                    .ThenInclude(p => p.Sizes)
                        .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (style == null) return NotFound();

            style.Products = style.Products
                .Where(p => !p.IsDeleted)
                .ToList();

            return View(style);
        }

        // GET: Styles/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create() => View(new StyleCreateViewModel());

        // POST: Styles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StyleCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var style = new Style
            {
                Name = vm.Name,
                Description = vm.Description
            };

            style.ListImagePath = await SaveStyleImage(vm.ListImageFile, vm.ListImageUrl);
            style.DetailImagePath = await SaveStyleImage(vm.DetailImageFile, vm.DetailImageUrl);

            _context.Styles.Add(style);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Styles/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var style = await _context.Styles.FindAsync(id);
            if (style == null) return NotFound();

            return View(new StyleCreateViewModel
            {
                Name = style.Name,
                Description = style.Description,
                ListImagePath = style.ListImagePath,
                DetailImagePath = style.DetailImagePath
            });
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StyleCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var style = await _context.Styles.FindAsync(id);
            if (style == null) return NotFound();

            style.Name = vm.Name;
            style.Description = vm.Description;

            var newList = await SaveStyleImage(vm.ListImageFile, vm.ListImageUrl);
            if (newList != null) style.ListImagePath = newList;

            var newDetail = await SaveStyleImage(vm.DetailImageFile, vm.DetailImageUrl);
            if (newDetail != null) style.DetailImagePath = newDetail;

            _context.Update(style);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<string?> SaveStyleImage(IFormFile? file, string? url)
        {
            if (file != null && file.Length > 0)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (allowed.Contains(ext))
                {
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var folder = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "images", "styles");
                    Directory.CreateDirectory(folder);
                    var path = Path.Combine(folder, fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    return $"/images/styles/{fileName}";
                }
            }
            if (!string.IsNullOrWhiteSpace(url))
                return url.Trim();
            return null;
        }

        // GET: Styles/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Styles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var style = await _context.Styles.FindAsync(id);
            if (style != null)
            {
                _context.Styles.Remove(style);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StyleExists(int id)
        {
            return _context.Styles.Any(e => e.Id == id);
        }
    }
}
