using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class Quiz : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}