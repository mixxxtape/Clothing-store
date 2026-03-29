using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Question : Entity
    {
        [Required(ErrorMessage = "Question text is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Question text must be between 5 and 300 characters")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Quiz reference is required")]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}