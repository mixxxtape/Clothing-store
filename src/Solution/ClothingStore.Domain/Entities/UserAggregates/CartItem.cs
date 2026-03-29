using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class CartItem : Entity
    {
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }
        public ProductAggregates.Product Product { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product size is required")]
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; } = null!;

        [Required(ErrorMessage = "Cart reference is required")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}