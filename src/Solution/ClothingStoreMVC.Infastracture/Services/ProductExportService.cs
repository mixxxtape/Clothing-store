using ClosedXML.Excel;
using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.Infrastructure.Services
{
    /// <summary>
    /// Exports all active products to an Excel file.
    /// One sheet per category; columns: Name, Description, Price, Style, Sizes.
    /// </summary>
    public class ProductExportService : IExportService<Product>
    {
        private static readonly IReadOnlyList<string> Headers = new[]
        {
            "Name", "Description", "Price (₴)", "Style", "Sizes"
        };

        private readonly ClothingStoreContext _context;

        public ProductExportService(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is not writable.", nameof(stream));

            var categories = await _context.Categories
                .Include(c => c.Products.Where(p => !p.IsDeleted))
                    .ThenInclude(p => p.Style)
                .Include(c => c.Products)
                    .ThenInclude(p => p.Sizes)
                        .ThenInclude(ps => ps.Size)
                .ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();

            foreach (var category in categories)
            {
                var products = category.Products.Where(p => !p.IsDeleted).ToList();
                if (!products.Any()) continue;

                // worksheet name max 31 chars
                var sheetName = category.Name.Length > 31
                    ? category.Name[..31]
                    : category.Name;

                var worksheet = workbook.Worksheets.Add(sheetName);
                WriteHeader(worksheet);
                WriteProducts(worksheet, products);
                worksheet.Columns().AdjustToContents();
            }

            // if no categories had products, add a placeholder sheet
            if (!workbook.Worksheets.Any())
                workbook.Worksheets.Add("No Data");

            workbook.SaveAs(stream);
        }

        // ── Writers ───────────────────────────────────────────────────────

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int i = 0; i < Headers.Count; i++)
                worksheet.Cell(1, i + 1).Value = Headers[i];

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#8062D6");
            headerRow.Style.Font.FontColor = XLColor.White;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        private static void WriteProducts(IXLWorksheet worksheet, List<Product> products)
        {
            int rowIndex = 2;
            foreach (var product in products)
            {
                WriteProduct(worksheet, product, rowIndex);

                // zebra striping
                if (rowIndex % 2 == 0)
                    worksheet.Row(rowIndex).Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#F9DFDF");

                rowIndex++;
            }
        }

        private static void WriteProduct(IXLWorksheet worksheet, Product product, int rowIndex)
        {
            var sizes = string.Join(", ",
                product.Sizes.Select(ps => $"{ps.Size?.Name} ({ps.Quantity})"));

            worksheet.Cell(rowIndex, 1).Value = product.Name;
            worksheet.Cell(rowIndex, 2).Value = product.Description;
            worksheet.Cell(rowIndex, 3).Value = (double)product.Price;
            worksheet.Cell(rowIndex, 3).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(rowIndex, 4).Value = product.Style?.Name ?? "—";
            worksheet.Cell(rowIndex, 5).Value = sizes;
        }
    }
}
