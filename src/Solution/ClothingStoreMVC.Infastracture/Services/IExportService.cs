namespace ClothingStoreMVC.Infrastructure.Services
{
    public interface IExportService<TEntity>
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
