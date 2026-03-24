using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = await _userManager.GetRolesAsync(user),
                AllRoles = _roleManager.Roles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.AddToRolesAsync(user, roles.Except(userRoles));
            await _userManager.RemoveFromRolesAsync(user, userRoles.Except(roles));

            return RedirectToAction("UserList");
        }
    }
}