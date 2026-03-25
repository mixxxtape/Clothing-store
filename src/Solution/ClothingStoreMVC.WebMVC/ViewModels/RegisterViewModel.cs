using System.ComponentModel.DataAnnotations;

namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯіІїЇєЄ']+$",
            ErrorMessage = "First name can only contain letters")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯіІїЇєЄ']+$",
            ErrorMessage = "Last name can only contain letters")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?[0-9]{10,15}$",
            ErrorMessage = "Phone must contain 10-15 digits")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
    }
}