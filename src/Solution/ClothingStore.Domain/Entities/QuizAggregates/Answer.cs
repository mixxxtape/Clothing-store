using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Answer : Entity
    { 
        public string Text { get; set; }
        public ICollection<AnswerStyle> Styles { get; set; } = new List<AnswerStyle>();
    }
}