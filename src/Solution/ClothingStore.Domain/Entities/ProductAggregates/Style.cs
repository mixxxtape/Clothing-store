using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Style : Entity
    {
        [Required(ErrorMessage = "Style name is required")]
        [StringLength(50, ErrorMessage = "The style name cannot be longer than 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Style description is required")]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters")]
        public string Description { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}