using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class  Result : Entity 
    {
        [Required(ErrorMessage = "Creation date is required")]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "User reference is required")]
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; } = null!;

        [Required(ErrorMessage = "Quiz reference is required")]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        [Required(ErrorMessage = "Style reference is required")]
        public int StyleId { get; set; }
        public ProductAggregates.Style Style { get; set; } = null!;
    }
}
