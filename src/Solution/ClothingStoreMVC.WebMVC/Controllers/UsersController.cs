using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var vms = new List<UserListViewModel>();

            foreach (var user in users)
            {
                vms.Add(new UserListViewModel
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email ?? "",
                    PhoneNumber = user.PhoneNumber,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                    EmailConfirmed = user.EmailConfirmed
                });
            }

            return View(vms);
        }

        // GET: /Users/EditRole/id
        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();

            return View(new EditUserRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email ?? "",
                FullName = $"{user.FirstName} {user.LastName}",
                AllRoles = allRoles,
                UserRoles = userRoles.ToList()
            });
        }

        // POST: /Users/EditRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string userId, List<string> selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (selectedRoles.Any())
                await _userManager.AddToRolesAsync(user, selectedRoles);

            TempData["Success"] = $"Roles updated for {user.Email}";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Users/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Email == User.Identity?.Name)
            {
                TempData["Error"] = "Cannot delete your own account";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "User deleted";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Users/ConfirmEmailManually
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmailManually(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);

            TempData["Success"] = $"Email confirmed for {user.Email}";
            return RedirectToAction(nameof(Index));
        }
    }
}