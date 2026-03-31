namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string CurrentStatus { get; set; } = null!;
        public List<OrderItemViewModel> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = null!;
        public string SizeName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}