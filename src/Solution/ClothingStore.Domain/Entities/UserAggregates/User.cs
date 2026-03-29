using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class User : Entity, IAggregateRoot
    {
        [Required(ErrorMessage = "Identity user reference is required")]
        [StringLength(450, ErrorMessage = "Identity user ID cannot exceed 450 characters")]
        public string IdentityUserId { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}