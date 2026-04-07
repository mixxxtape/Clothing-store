using ClothingStoreMVC.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ChartsController : ControllerBase
    {
        private readonly ClothingStoreContext _context;

        public ChartsController(ClothingStoreContext context)
        {
            _context = context;
        }

        // 1. Кількість товарів по стилях
        private record ProductsByStyleItem(string Style, int Count);

        [HttpGet("productsByStyle")]
        public async Task<JsonResult> GetProductsByStyleAsync(CancellationToken cancellationToken)
        {
            var data = await _context.Products
                .IgnoreQueryFilters()
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Style.Name)
                .Select(g => new ProductsByStyleItem(g.Key, g.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(data);
        }

        // 2. Кількість товарів по категоріях
        private record ProductsByCategoryItem(string Category, int Count);

        [HttpGet("productsByCategory")]
        public async Task<JsonResult> GetProductsByCategoryAsync(CancellationToken cancellationToken)
        {
            var data = await _context.Products
                .IgnoreQueryFilters()
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Category.Name)
                .Select(g => new ProductsByCategoryItem(g.Key, g.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(data);
        }

        // 3. Популярність стилів по результатах квізу
        private record StylePopularityItem(string Style, int Count);

        [HttpGet("stylesByQuiz")]
        public async Task<JsonResult> GetStylesByQuizAsync(CancellationToken cancellationToken)
        {
            var data = await _context.Results
                .Include(r => r.Style)
                .GroupBy(r => r.Style.Name)
                .Select(g => new StylePopularityItem(g.Key, g.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(data);
        }

        // 4. Середня ціна по категоріях
        private record AvgPriceByCategoryItem(string Category, double AvgPrice);

        [HttpGet("avgPriceByCategory")]
        public async Task<JsonResult> GetAvgPriceByCategoryAsync(CancellationToken cancellationToken)
        {
            var data = await _context.Products
                .IgnoreQueryFilters()
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Category.Name)
                .Select(g => new AvgPriceByCategoryItem(g.Key, Math.Round((double)g.Average(p => p.Price), 2)))
                .ToListAsync(cancellationToken);

            return new JsonResult(data);
        }
    }
}
