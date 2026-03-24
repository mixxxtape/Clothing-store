using ClothingStoreMVC.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClothingStoreContext _context;

        public HomeController(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var newArrivals = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Style)
                .OrderByDescending(p => p.Id)
                .Take(3)
                .ToListAsync();

            return View(newArrivals);
        }
    }
}