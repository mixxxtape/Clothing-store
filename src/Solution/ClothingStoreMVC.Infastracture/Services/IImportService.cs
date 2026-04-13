namespace ClothingStoreMVC.Infrastructure.Services
{
    public interface IImportService<TEntity>
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }
}
