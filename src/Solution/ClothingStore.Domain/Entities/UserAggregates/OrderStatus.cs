using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    internal class OrderStatus : Entity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Status { get; set; }
        public string ChangeReason { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
