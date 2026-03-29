using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class Payment : Entity
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        [Display(Name = "Paid At")]
        public DateTime PaidAt { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Payment method must be between 2 and 50 characters")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Payment status is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Payment status must be between 2 and 50 characters")]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "Order reference is required")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
