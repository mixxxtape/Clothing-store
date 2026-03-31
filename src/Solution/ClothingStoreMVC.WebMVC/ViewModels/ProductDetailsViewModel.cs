namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public string StyleName { get; set; } = null!;
        public string Sizes { get; set; } = null!;
        public List<SizeOptionViewModel> SizeOptions { get; set; } = new();
    }

    public class SizeOptionViewModel
    {
        public int ProductSizeId { get; set; }
        public string SizeName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}