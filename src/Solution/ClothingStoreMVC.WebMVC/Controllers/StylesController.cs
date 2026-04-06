using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Styles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Name,Description,Id")] Style style)
        {
            if (ModelState.IsValid)
            {
                _context.Add(style);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(style);
        }

        // GET: Styles/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Styles.FindAsync(id);
            if (style == null)
            {
                return NotFound();
            }
            return View(style);
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id")] Style style)
        {
            if (id != style.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(style);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StyleExists(style.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(style);
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
