using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Size : Entity
    {
        [Required(ErrorMessage = "Size is required")]
        [StringLength(20, ErrorMessage = "The size cannot be longer than 20 characters")]
        public string Name { get; set; } = null!;

        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}