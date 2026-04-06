namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class CatalogFilterViewModel
    {
        public IEnumerable<ProductListViewModel> Products { get; set; } = new List<ProductListViewModel>();
        public List<string> Categories { get; set; } = new();
        public List<string> Styles { get; set; } = new();
        public string? SearchQuery { get; set; }
        public string? SelectedCategory { get; set; }
        public string? SelectedStyle { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal AbsoluteMinPrice { get; set; }
        public decimal AbsoluteMaxPrice { get; set; }
    }
}