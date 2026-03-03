using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
     public class Product : Entity, IAggregateRoot
    {
        [Required(ErrorMessage = "Назва продукту є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва продукту не може бути довше 100 символів")]
        [Display(Name = "Назва продукту")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Опис продукту є обов'язковим")]
        [StringLength(500, ErrorMessage = "Опис продукту не може бути довше 500 символів")]
        [Display(Name = "Опис продукту")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Ціна обов'язкова")]
        [Range(0.01, 100000, ErrorMessage = "Ціна повинна бути додатньою")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Стиль обов'язковий")]
        [Display(Name = "Стиль")]
        public int StyleId { get; set; } 
        public Style? Style { get; set; } = null!;

        [Required(ErrorMessage = "Категорія обов'язкова")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } = null!;
       
        [Display(Name = "Видалено")]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Відгуки")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [Display(Name = "Розміри")]
        public ICollection<ProductSize> Sizes { get; set; } = new List<ProductSize>();
    }

}