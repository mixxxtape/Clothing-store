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
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public bool CanReview { get; set; }
        public bool AlreadyReviewed { get; set; }
        public double AverageRating { get; set; }
    }

    public class SizeOptionViewModel
    {
        public int ProductSizeId { get; set; }
        public string SizeName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}