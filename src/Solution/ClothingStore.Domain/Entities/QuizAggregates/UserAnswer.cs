using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    internal class UserAnswer : Entity
    {
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int AnserId { get; set; }
        public Answer Answer { get; set; }
    }
}
