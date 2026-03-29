using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class UserAnswer : Entity
    {
        [Required(ErrorMessage = "User reference is required")]
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; } = null!;

        [Required(ErrorMessage = "Question reference is required")]
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        [Required(ErrorMessage = "Answer reference is required")]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; } = null!;
    }
}
