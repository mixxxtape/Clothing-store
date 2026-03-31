using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Delivery address is required")]
        [StringLength(300, MinimumLength = 5)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = null!;

        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}