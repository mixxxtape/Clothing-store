using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using ClothingStoreMVC.Infrastructure;

namespace ClothingStoreMVC.Infrastructure.Services
{
    public class ProductDataPortServiceFactory : IDataPortServiceFactory<Product>
    {
        private const string ExcelContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly ClothingStoreContext _context;

        public ProductDataPortServiceFactory(ClothingStoreContext context)
        {
            _context = context;
        }

        public IImportService<Product> GetImportService(string contentType)
        {
            if (contentType == ExcelContentType)
                return new ProductImportService(_context);

            throw new NotSupportedException(
                $"Import not supported for content type: {contentType}");
        }

        public IExportService<Product> GetExportService(string contentType)
        {
            if (contentType == ExcelContentType)
                return new ProductExportService(_context);

            throw new NotSupportedException(
                $"Export not supported for content type: {contentType}");
        }
    }
}
