using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class OrderItem : Entity
    {
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }
        public ProductAggregates.Product Product { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product size is required")]
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }

        [Required(ErrorMessage = "Order reference is required")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }

}