using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Review : Entity
    {
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; }
        public int ProductId { get; set; }
        public ProductAggregates.Product Product { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
    }
}