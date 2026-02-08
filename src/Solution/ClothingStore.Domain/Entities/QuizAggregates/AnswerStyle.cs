using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    internal class AnswerStyle : Entity
    {
        public int AnswerId { get; set; }
        public int StyleId { get; set; }

    }
}