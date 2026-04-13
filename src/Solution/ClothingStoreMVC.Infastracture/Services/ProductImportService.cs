using ClosedXML.Excel;
using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.Infrastructure.Services
{
    /// <summary>
    /// Imports products from an Excel file.
    /// Expected columns per row:
    ///   1 – Name
    ///   2 – Description
    ///   3 – Price
    ///   4 – Category name
    ///   5 – Style name
    /// Row 1 is a header and is skipped.
    /// </summary>
    public class ProductImportService : IImportService<Product>
    {
        private readonly ClothingStoreContext _context;

        public ProductImportService(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Stream cannot be read.", nameof(stream));

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            foreach (var row in worksheet.RowsUsed().Skip(1)) // skip header
            {
                await AddProductAsync(row, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddProductAsync(IXLRow row, CancellationToken cancellationToken)
        {
            var name = GetName(row);
            var description = GetDescription(row);
            var price = GetPrice(row);

            if (string.IsNullOrWhiteSpace(name)) return; // skip empty rows

            var category = await GetOrCreateCategoryAsync(row, cancellationToken);
            var style = await GetOrCreateStyleAsync(row, cancellationToken);

            // avoid duplicate products (same name + category + style)
            var exists = await _context.Products
                .IgnoreQueryFilters()
                .AnyAsync(p => p.Name == name &&
                               p.CategoryId == category.Id &&
                               p.StyleId == style.Id, cancellationToken);

            if (exists) return;

            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                Style = style,
                IsDeleted = false
            };

            _context.Products.Add(product);
        }

        // ── Column readers ────────────────────────────────────────────────

        private static string GetName(IXLRow row)
            => row.Cell(1).GetString().Trim();

        private static string GetDescription(IXLRow row)
        {
            var val = row.Cell(2).GetString().Trim();
            return string.IsNullOrEmpty(val) ? "Imported from Excel" : val;
        }

        private static decimal GetPrice(IXLRow row)
        {
            if (decimal.TryParse(row.Cell(3).GetString().Trim(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var price) && price > 0)
                return price;
            return 0;
        }

        private static string GetCategoryName(IXLRow row)
        {
            var val = row.Cell(4).GetString().Trim();
            return string.IsNullOrEmpty(val) ? "Uncategorized" : val;
        }

        private static string GetStyleName(IXLRow row)
        {
            var val = row.Cell(5).GetString().Trim();
            return string.IsNullOrEmpty(val) ? "General" : val;
        }

        // ── Entity resolvers ──────────────────────────────────────────────

        private async Task<Category> GetOrCreateCategoryAsync(IXLRow row, CancellationToken ct)
        {
            var name = GetCategoryName(row);
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name, ct);

            if (category is null)
            {
                category = new Category { Name = name };
                _context.Categories.Add(category);
            }

            return category;
        }

        private async Task<Style> GetOrCreateStyleAsync(IXLRow row, CancellationToken ct)
        {
            var name = GetStyleName(row);
            var style = await _context.Styles
                .FirstOrDefaultAsync(s => s.Name == name, ct);

            if (style is null)
            {
                style = new Style { Name = name, Description = "Imported from Excel" };
                _context.Styles.Add(style);
            }

            return style;
        }
    }
}
