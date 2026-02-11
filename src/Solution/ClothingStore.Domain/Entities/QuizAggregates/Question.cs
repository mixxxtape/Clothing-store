using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Question : Entity
    {
        public string Text { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

    }
}