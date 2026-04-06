using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsOwner { get; set; }
    }

    public class CreateReviewViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}