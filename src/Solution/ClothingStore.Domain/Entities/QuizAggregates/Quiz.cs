using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Quiz : Entity, IAggregateRoot
    {
        [Required(ErrorMessage = "Quiz name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Quiz name must be between 2 and 100 characters")]
        [Display(Name = "Quiz Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Display(Name = "Questions")]
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        [Display(Name = "Results")]
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}