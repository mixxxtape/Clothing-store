using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Category : Entity
    {
        [Required(ErrorMessage = "Назва категорії є обов'язковою")]
        [StringLength(50, ErrorMessage = "Назва категорії не може бути довше 50 символів")]
        [Display(Name = "Категорія")]
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}