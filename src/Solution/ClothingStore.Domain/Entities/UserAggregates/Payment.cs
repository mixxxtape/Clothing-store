using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    internal class Payment : Entity
    {
        public double Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
