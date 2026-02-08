using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    internal class Cart : Entity
    {
        public int UserId { get; set; }
        User User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}