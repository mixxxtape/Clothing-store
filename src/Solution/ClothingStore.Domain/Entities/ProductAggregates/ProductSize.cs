using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class ProductSize : Entity
    {
        [Required(ErrorMessage = "Select a product") ]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required(ErrorMessage = "Select a size")]
        public int SizeId { get; set; }
        public Size Size { get; set; } = null!;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive integer")]
        public int Quantity { get; set; }
    }
}
