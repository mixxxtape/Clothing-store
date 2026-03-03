using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Size : Entity
    {
        [Required(ErrorMessage = "Назва розміру обов'язкова")]
        [StringLength(10, ErrorMessage = "Назва розміру не може бути довше 10 символів")]
        [Display(Name = "Розмір")]
        public string Name { get; set; } = null!;
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}