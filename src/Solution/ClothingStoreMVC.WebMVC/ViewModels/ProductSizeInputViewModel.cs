using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class ProductSizeInputViewModel
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; } = null!;
        public bool IsSelected { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductCreateViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Required]
        public int StyleId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }

        public List<ProductSizeInputViewModel> Sizes { get; set; } = new();
    }
}