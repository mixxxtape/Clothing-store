using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class Cart : Entity
    {
        [Required(ErrorMessage = "User reference is required")]
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}