using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [RegularExpression(@"^\+?[0-9]{10,15}$")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        [Display(Name = "About Me")]
        public string? Bio { get; set; }

        [StringLength(300)]
        [Display(Name = "Default Address")]
        public string? DefaultAddress { get; set; }

        public string? Email { get; set; }
    }
}