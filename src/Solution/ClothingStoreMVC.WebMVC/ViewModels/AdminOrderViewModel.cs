namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class AdminOrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string CurrentStatus { get; set; } = null!;
        public List<OrderItemViewModel> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}