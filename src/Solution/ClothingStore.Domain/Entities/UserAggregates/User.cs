using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using System.Collections.Generic;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class User : Entity, IAggregateRoot
    {
        public string IdentityUserId { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}