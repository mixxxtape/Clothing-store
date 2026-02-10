using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Style : Entity
    {
          public string Name { get; set; }
          public string Description { get; set; }
          public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}