using ClosedXML.Excel;
using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.Infrastructure.Services
{
    public class ProductImportService : IImportService<Product>
    {
        private readonly ClothingStoreContext _context;

        public ProductImportService(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken ct)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Stream cannot be read.", nameof(stream));

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var categories = await _context.Categories.ToListAsync(ct);
            var styles = await _context.Styles.ToListAsync(ct);

            var errors = new List<string>();

            var dataRows = worksheet.RowsUsed().Skip(1).ToList(); 

            if (!dataRows.Any())
                throw new ImportException("The file contains no data rows.");

            var productsToAdd = new List<Product>();

            foreach (var row in dataRows)
            {
                var rowErrors = ValidateRow(row, categories, styles,
                                            out var product);
                if (rowErrors.Count > 0)
                {
                    errors.AddRange(rowErrors);
                }
                else
                {
                    productsToAdd.Add(product!);
                }
            }

            if (productsToAdd.Any())
            {
                _context.Products.AddRange(productsToAdd);
                await _context.SaveChangesAsync(ct);
            }

            if (errors.Any())
            {
                throw new ImportException(string.Join("\n", errors));
            }

            foreach (var p in productsToAdd)
            {
                bool exists = await _context.Products
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Name == p.Name &&
                                   x.CategoryId == p.CategoryId &&
                                   x.StyleId == p.StyleId, ct);
                if (!exists)
                    _context.Products.Add(p);
            }

            await _context.SaveChangesAsync(ct);
        }


        private static List<string> ValidateRow(
            IXLRow row,
            List<Category> categories,
            List<Style> styles,
            out Product? product)
        {
            product = null;
            var errors = new List<string>();
            int rowNum = row.RowNumber();

            var name = row.Cell(1).GetString().Trim();
            if (string.IsNullOrEmpty(name))
                errors.Add($"Row {rowNum}: Name is required.");
            else if (name.Length < 2)
                errors.Add($"Row {rowNum}: Name is too short (min 2 characters).");
            else if (name.Length > 100)
                errors.Add($"Row {rowNum}: Name is too long (max 100 characters).");

            var description = row.Cell(2).GetString().Trim();
            if (string.IsNullOrEmpty(description))
                errors.Add($"Row {rowNum}: Description is required.");
            else if (description.Length > 500)
                errors.Add($"Row {rowNum}: Description is too long (max 500 characters).");

            decimal price = 0;
            var priceCell = row.Cell(3);
            string priceRaw = priceCell.Value.ToString().Trim().Replace(',', '.');

            if (string.IsNullOrEmpty(priceRaw))
            {
                errors.Add($"Row {rowNum}: Price is required.");
            }
            else if (!decimal.TryParse(priceRaw,
                         System.Globalization.NumberStyles.Any,
                         System.Globalization.CultureInfo.InvariantCulture,
                         out price))
            {
                errors.Add($"Row {rowNum}: Price \"{priceRaw}\" is not a valid number.");
            }
            else if (price <= 0)
            {
                errors.Add($"Row {rowNum}: Price must be greater than 0.");
            }
            else if (price > 100000)
            {
                errors.Add($"Row {rowNum}: Price exceeds the maximum allowed value (100 000).");
            }

            var categoryName = row.Cell(4).GetString().Trim();
            Category? category = null;
            if (string.IsNullOrEmpty(categoryName))
            {
                errors.Add($"Row {rowNum}: Category is required.");
            }
            else
            {
                category = categories.FirstOrDefault(c =>
                    string.Equals(c.Name, categoryName,
                                  StringComparison.OrdinalIgnoreCase));
                if (category is null)
                    errors.Add($"Row {rowNum}: Category \"{categoryName}\" does not exist. " +
                               $"Available: {string.Join(", ", categories.Select(c => c.Name))}.");
            }

            var styleName = row.Cell(5).GetString().Trim();
            Style? style = null;
            if (string.IsNullOrEmpty(styleName))
            {
                errors.Add($"Row {rowNum}: Style is required.");
            }
            else
            {
                style = styles.FirstOrDefault(s =>
                    string.Equals(s.Name, styleName,
                                  StringComparison.OrdinalIgnoreCase));
                if (style is null)
                    errors.Add($"Row {rowNum}: Style \"{styleName}\" does not exist. " +
                               $"Available: {string.Join(", ", styles.Select(s => s.Name))}.");
            }

            if (errors.Count == 0)
            {
                product = new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Category = category!,
                    CategoryId = category!.Id,
                    Style = style!,
                    StyleId = style!.Id,
                    IsDeleted = false
                };
            }

            return errors;
        }
    }


    public class ImportException : Exception
    {
        public ImportException(string message) : base(message) { }
    }
}