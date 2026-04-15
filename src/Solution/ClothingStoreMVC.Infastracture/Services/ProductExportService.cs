using ClosedXML.Excel;
using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.Infrastructure.Services
{
    public class ProductExportService : IExportService<Product>
    {
        private static readonly IReadOnlyList<string> Headers = new[]
        {
            "Name", "Description", "Price (₴)", "Category", "Style", "Sizes"
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

            var products = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Style)
                .Include(p => p.Sizes)
                    .ThenInclude(ps => ps.Size)
                .OrderBy(p => p.Category.Name)
                .ThenBy(p => p.Name)
                .ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Products");

            WriteHeader(worksheet);
            WriteProducts(worksheet, products);
            worksheet.Columns().AdjustToContents();

            worksheet.SheetView.FreezeRows(1);

            workbook.SaveAs(stream);
        }

        private static void WriteHeader(IXLWorksheet ws)
        {
            for (int i = 0; i < Headers.Count; i++)
                ws.Cell(1, i + 1).Value = Headers[i];

            var row = ws.Row(1);
            row.Style.Font.Bold = true;
            row.Style.Fill.BackgroundColor = XLColor.FromHtml("#8062D6");
            row.Style.Font.FontColor = XLColor.White;
            row.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            row.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            row.Style.Border.BottomBorderColor = XLColor.FromHtml("#0C0950");
        }

        private static void WriteProducts(IXLWorksheet ws, List<Product> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                int rowIndex = i + 2;
                var p = products[i];

                var sizes = string.Join(", ",
                    p.Sizes.Select(ps => $"{ps.Size?.Name} ({ps.Quantity})"));

                ws.Cell(rowIndex, 1).Value = p.Name;
                ws.Cell(rowIndex, 2).Value = p.Description;
                ws.Cell(rowIndex, 3).Value = (double)p.Price;
                ws.Cell(rowIndex, 3).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(rowIndex, 4).Value = p.Category?.Name ?? "—";
                ws.Cell(rowIndex, 5).Value = p.Style?.Name ?? "—";
                ws.Cell(rowIndex, 6).Value = sizes;

                if (rowIndex % 2 == 0)
                    ws.Row(rowIndex).Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#F9DFDF");
            }
        }
    }
}