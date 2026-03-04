using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Quiz : Entity, IAggregateRoot
    {
        [Required(ErrorMessage = "Назва квізу має бути обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва квізу не може бути довше 100 символів")]
        [Display(Name = "Квіз")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Опис квізу є обов'язковим")]
        [StringLength(500, ErrorMessage = "Опис квізу не може бути довше 500 символів")]
        [Display(Name = "Опис квіза")]
        public string Description { get; set; } = null!;

        [Display(Name = "Перелік питань")]
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        [Display(Name = "Перелік результатів")]
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}