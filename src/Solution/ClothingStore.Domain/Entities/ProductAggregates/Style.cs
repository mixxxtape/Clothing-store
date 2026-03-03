using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Style : Entity
    {
        [Required(ErrorMessage = "Назва стилю є обов'язковою")]
        [StringLength(50, ErrorMessage = "Назва стилю не може бути довше 50 символів")]
        [Display(Name = "Стиль")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Опис стилю є обов'язковим")]
        [StringLength(200, ErrorMessage = "Опис не може бути довше 200 символів")]
        [Display(Name = "Опис стилю")]
        public string Description { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}