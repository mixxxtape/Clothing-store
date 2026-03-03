using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Review : Entity
    {
        [Required]
        [Display(Name = "Користувач")]
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; } = null!;

        [Required]
        [Display(Name = "Товар")]
        public int ProductId { get; set; }
        public ProductAggregates.Product Product { get; set; } = null!;

        [Required]
        [Range(1, 5, ErrorMessage = "Рейтинг повинен бути від 1 до 5")]
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Коментар не може бути довше 500 символів")]
        [Display(Name = "Коментар")]
        public string? Comment { get; set; }
    }
}