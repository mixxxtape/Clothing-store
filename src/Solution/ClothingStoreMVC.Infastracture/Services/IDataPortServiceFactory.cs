namespace ClothingStoreMVC.Infrastructure.Services
{
    public interface IDataPortServiceFactory<TEntity>
    {
        IImportService<TEntity> GetImportService(string contentType);
        IExportService<TEntity> GetExportService(string contentType);
    }
}
