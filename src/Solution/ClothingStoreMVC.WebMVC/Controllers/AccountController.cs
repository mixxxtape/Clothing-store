using ClothingStoreMVC.Domain.Entities.UserAggregates;
using ClothingStoreMVC.Infrastructure;
using ClothingStoreMVC.Infrastructure.Services;
using ClothingStoreMVC.WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ClothingStoreContext _context;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ClothingStoreContext context,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "user");

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
            _context.Users.Add(new User
            {
                IdentityUserId = user.Id,
                RoleId = userRole!.Id
            });
            await _context.SaveChangesAsync();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmUrl = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token }, Request.Scheme);

            await _emailService.SendAsync(user.Email!,
                "Confirm your Ravel account",
                $@"<div style='font-family:Georgia,serif; max-width:500px; margin:0 auto;'>
                    <h2 style='color:#0C0950;'>Welcome to Ravel ✦</h2>
                    <p>Hi {user.FirstName}, please confirm your email address:</p>
                    <a href='{confirmUrl}'
                       style='display:inline-block; padding:0.75rem 2rem;
                       background:linear-gradient(135deg,#8062D6,#E8A9A9);
                       color:white; text-decoration:none; border-radius:4px;
                       font-size:0.85rem; letter-spacing:0.1em;'>
                        Confirm Email
                    </a>
                    <p style='color:#6b6b8a; font-size:0.8rem; margin-top:1.5rem;'>
                        If you did not create this account, ignore this email.
                    </p>
                </div>");

            TempData["Info"] = "Registration successful! Please check your email to confirm your account.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["Success"] = "Email confirmed! You can now sign in.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Email confirmation failed. The link may have expired.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null) =>
            View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty,
                    "Please confirm your email before signing in.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("admin")
                ? RedirectToAction("Index", "Products")
                : RedirectToAction("Index", "Catalog");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            return View(new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                DefaultAddress = user.DefaultAddress,
                Email = user.Email
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.PhoneNumber = vm.PhoneNumber;
            user.Bio = vm.Bio;
            user.DefaultAddress = vm.DefaultAddress;

            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Profile updated";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var result = await _userManager.ChangePasswordAsync(
                user, vm.CurrentPassword, vm.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(vm);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Success"] = "Password changed successfully";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetUrl = Url.Action("ResetPassword", "Account",
                    new { token, email = vm.Email }, Request.Scheme);

                await _emailService.SendAsync(vm.Email,
                    "Reset your Ravel password",
                    $@"<div style='font-family:Georgia,serif; max-width:500px; margin:0 auto;'>
                        <h2 style='color:#0C0950;'>Password Reset ✦</h2>
                        <p>Click the link below to reset your password:</p>
                        <a href='{resetUrl}'
                           style='display:inline-block; padding:0.75rem 2rem;
                           background:linear-gradient(135deg,#8062D6,#E8A9A9);
                           color:white; text-decoration:none; border-radius:4px;
                           font-size:0.85rem;'>
                            Reset Password
                        </a>
                        <p style='color:#6b6b8a; font-size:0.8rem; margin-top:1.5rem;'>
                            This link expires in 24 hours.
                            If you did not request a reset, ignore this email.
                        </p>
                    </div>");
            }

            TempData["Info"] = "If that email exists, a reset link has been sent.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email) =>
            View(new ResetPasswordViewModel { Token = token, Email = email });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                TempData["Success"] = "Password has been reset.";
                return RedirectToAction("Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(vm);
            }

            TempData["Success"] = "Password reset successful. You can now sign in.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}