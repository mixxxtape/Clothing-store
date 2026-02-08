using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    internal class Question : Entity
    {
        public string Text { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}