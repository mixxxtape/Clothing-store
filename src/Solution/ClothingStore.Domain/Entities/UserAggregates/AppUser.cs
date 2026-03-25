using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.Domain.Entities.UserAggregates
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯіІїЇєЄ']+$",
            ErrorMessage = "First name can only contain letters")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯіІїЇєЄ']+$",
            ErrorMessage = "Last name can only contain letters")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^\+?[0-9]{10,15}$",
            ErrorMessage = "Phone must contain 10-15 digits, optionally starting with +")]
        [Display(Name = "Phone Number")]
        public override string? PhoneNumber { get; set; }
    }
}