namespace ClothingStoreMVC.WebMVC.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool EmailConfirmed { get; set; }
    }

    public class EditUserRoleViewModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public List<string> AllRoles { get; set; } = new();
        public List<string> UserRoles { get; set; } = new();
    }
}