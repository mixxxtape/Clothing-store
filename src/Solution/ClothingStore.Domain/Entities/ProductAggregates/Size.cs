using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Size : Entity
    {
        public string Name { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}