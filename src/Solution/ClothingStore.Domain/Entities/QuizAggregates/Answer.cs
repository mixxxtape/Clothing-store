using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Answer : Entity
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(100, ErrorMessage = "Answer cannot be longer than 100 characters")]
        public string Text { get; set; } = null!;

        public ICollection<AnswerStyle> Styles { get; set; } = new List<AnswerStyle>();

        [Required(ErrorMessage = "Choose question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

    }
}