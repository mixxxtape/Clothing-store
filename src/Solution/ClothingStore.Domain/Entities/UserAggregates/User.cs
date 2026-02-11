using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class User : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();


    }
}