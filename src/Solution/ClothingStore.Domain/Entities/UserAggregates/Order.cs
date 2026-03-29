using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class Order : Entity, IAggregateRoot
    {
        [Required(ErrorMessage = "User reference is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid user ID")]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Required(ErrorMessage = "Order date is required")]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        public ICollection<OrderStatus> StatusHistory { get; set; } = new List<OrderStatus>();

        [Required(ErrorMessage = "Delivery address is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Delivery address must be between 5 and 300 characters")]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        public Payment Payment { get; set; }
    }
}