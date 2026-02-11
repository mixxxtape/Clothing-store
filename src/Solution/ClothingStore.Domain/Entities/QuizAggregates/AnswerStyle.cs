using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class AnswerStyle : Entity
    {
        public int AnswerId { get; set; }
        public int StyleId { get; set; }
        public Answer Answer { get; set; }
        public ProductAggregates.Style Style { get; set; } 

    }
}