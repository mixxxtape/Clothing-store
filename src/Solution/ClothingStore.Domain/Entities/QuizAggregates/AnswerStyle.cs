using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class AnswerStyle : Entity
    {
        [Required(ErrorMessage = "Answer reference is required")]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; } = null!;

        [Required(ErrorMessage = "Style reference is required")]
        public int StyleId { get; set; }
        public ProductAggregates.Style Style { get; set; } = null!;

    }
}