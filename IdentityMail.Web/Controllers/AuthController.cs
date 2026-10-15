
using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    public class AuthController(
        UserManager<AppUser> _userManager,
        SignInManager<AppUser> _signInManager) : Controller
    {
        // REGISTER GET
        public IActionResult Register()
        {
            return View();
        }


        // REGISTER POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Şifreler birbiri ile uyumlu değil!"
                );

                return View(registerDto);
            }

            var user = new AppUser()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(
                user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        error.Code,
                        error.Description);
                }

                return View(registerDto);
            }

            return RedirectToAction("Login");
        }


        // LOGIN GET
        public IActionResult Login()
        {
            return View();
        }


        // LOGIN POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Bu Email sistemde kayıtlı değil!"
                );

                return View(loginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                loginDto.Password,
                false,
                false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Email veya Şifre hatalı!"
                );

                return View(loginDto);
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" }
                );
            }

            return RedirectToAction(
                "Index",
                "Message",
                new { area = "" }
            );
        }


        // ŞİFREMİ UNUTTUM GET
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        // ŞİFREMİ UNUTTUM POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            ViewBag.Message =
                "Bu projede e-posta gönderimi aktif değil. " +
                "Şifre sıfırlama bağlantısı gönderilmedi.";

            return View(dto);
        }


        // LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Auth");
        }
    }
}
