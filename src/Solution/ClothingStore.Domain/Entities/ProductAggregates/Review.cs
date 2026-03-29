using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStoreMVC.Domain.Entities.ProductAggregates
{
    public class Review : Entity
    {
        [Required]
        public int UserId { get; set; }
        public UserAggregates.User User { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
        public ProductAggregates.Product Product { get; set; } = null!;

        [Required]
        [Range(1, 5, ErrorMessage = "Rating should be on a scale of 1 to 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters")]
        public string? Comment { get; set; }
    }
}