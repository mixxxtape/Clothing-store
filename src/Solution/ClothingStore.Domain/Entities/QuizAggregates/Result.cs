using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.QuizAggregates
{
    public class  Result : Entity 
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public int StyleId { get; set; }
        public ProductAggregates.Style Style { get; set; }
    }
}
