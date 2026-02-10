using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class Wishlist : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}