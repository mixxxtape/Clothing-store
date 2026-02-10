using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
     public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StyleId { get; set; }
        public Style Style { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductSize> Sizes { get; set; } = new List<ProductSize>();
    }

}