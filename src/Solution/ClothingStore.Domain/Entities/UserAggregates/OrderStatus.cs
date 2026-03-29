using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class OrderStatus : Entity
    {
        [Required(ErrorMessage = "Order reference is required")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Status must be between 2 and 50 characters")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [StringLength(300, ErrorMessage = "Change reason cannot exceed 300 characters")]
        [Display(Name = "Change Reason")]
        public string? ChangeReason { get; set; }

        [Required(ErrorMessage = "Change date is required")]
        [Display(Name = "Changed At")]
        public DateTime ChangedAt { get; set; }
    }
}
