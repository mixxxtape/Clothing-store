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
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

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
                    }).ToList()
            };

            return View(vm);
        }


    }
}