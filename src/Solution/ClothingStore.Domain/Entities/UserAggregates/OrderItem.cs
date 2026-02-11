using ClothingStoreMVC.Domain.Entities.ProductAggregates;
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
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }

}