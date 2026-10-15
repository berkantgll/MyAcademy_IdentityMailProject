
using IdentityMail.Web.DTOs;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    [Authorize]
    public class ProfileController(
        UserManager<AppUser> _userManager,
        SignInManager<AppUser> _signInManager) : Controller
    {
        // PROFİLİM
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            return View(user);
        }


        // PROFİL DÜZENLE - GET
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var dto = new EditProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(dto);
        }


        // PROFİL DÜZENLE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            user.FirstName = dto.FirstName.Trim();
            user.LastName = dto.LastName.Trim();

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(dto);
            }

            TempData["Success"] = "Profiliniz güncellendi.";

            return RedirectToAction(nameof(Index));
        }


        // ŞİFRE DEĞİŞTİR - GET
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        // ŞİFRE DEĞİŞTİR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(dto);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["Success"] = "Şifreniz başarıyla değiştirildi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
