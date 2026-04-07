namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class WishlistViewModel
    {
        public List<WishlistItemViewModel> Items { get; set; } = new();
    }

    public class WishlistItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string StyleName { get; set; } = null!;
        public string? ImagePath { get; set; }
    }
}