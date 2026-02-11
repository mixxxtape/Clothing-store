using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class Order : Entity, IAggregateRoot
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public ICollection<OrderStatus> StatusHistory { get; set; } = new List<OrderStatus>();
        public string DeliveryAddress { get; set; }
        public Payment Payment { get; set; }
    }
}