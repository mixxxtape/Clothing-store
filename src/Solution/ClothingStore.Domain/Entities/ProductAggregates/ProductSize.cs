using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class ProductSize : Entity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int SizeId { get; set; }
        public Size Size { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
