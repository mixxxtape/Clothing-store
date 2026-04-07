using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class StyleCreateViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10000)]
        public string Description { get; set; } = null!;

        public IFormFile? ListImageFile { get; set; }
        public string? ListImageUrl { get; set; }
        public string? ListImagePath { get; set; }

        public IFormFile? DetailImageFile { get; set; }
        public string? DetailImageUrl { get; set; }
        public string? DetailImagePath { get; set; }
    }
}