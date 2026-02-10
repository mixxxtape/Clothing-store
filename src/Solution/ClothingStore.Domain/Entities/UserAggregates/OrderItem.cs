using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class OrderItem : Entity
    {
        public int ProductId { get; set; }
        public ProductAggregates.Product Product { get; set; }
        public int Quantity { get; set; }
        public int SizeId { get; set; }
        public ProductAggregates.Size Size { get; set; }

    }

}