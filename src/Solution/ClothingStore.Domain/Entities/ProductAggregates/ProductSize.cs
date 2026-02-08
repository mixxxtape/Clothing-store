using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    internal class ProductSize : Entity
    {
        public int SizeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}