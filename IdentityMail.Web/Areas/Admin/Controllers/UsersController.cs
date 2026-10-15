
using IdentityMail.Web.Context;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController(
        AppDbContext _context,
        UserManager<AppUser> _userManager) : Controller
    {
        // KULLANICILARI LİSTELE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .AsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();

            var userRoles = new Dictionary<int, string>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userRoles[user.Id] = roles.Count > 0
                    ? string.Join(", ", roles)
                    : "Standart Kullanıcı";
            }

            ViewBag.UserRoles = userRoles;

            return View(users);
        }


        // KULLANICI DETAYI
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            // Kullanıcının rolü
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.UserRole = roles.Count > 0
                ? string.Join(", ", roles)
                : "Standart Kullanıcı";

            // Kullanıcının mesaj istatistikleri
            ViewBag.SentCount = await _context.UserMessages
                .CountAsync(x => x.SenderId == id);

            ViewBag.ReceivedCount = await _context.UserMessages
                .CountAsync(x => x.ReceiverId == id);

            ViewBag.DraftCount = await _context.DraftMessages
                .CountAsync(x => x.UserId == id);

            ViewBag.CategoryCount = await _context.MailCategories
                .CountAsync(x => x.UserId == id);

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            // Yalnızca bu iki role izin veriyoruz.
            if (role != "Admin" && role != "User")
                return BadRequest();

            // Rolü değiştirilecek kullanıcıyı bul.
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound();

            // Admin kendi yetkisini değiştiremesin.
            if (_userManager.GetUserId(User) == user.Id.ToString())
            {
                TempData["Error"] = "Kendi rolünüzü değiştiremezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            // Kullanıcının mevcut rollerini getir.
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Aynı rol zaten atanmışsa işlem yapma.
            if (currentRoles.Count == 1 && currentRoles[0] == role)
                return RedirectToAction(nameof(Index));

            // İşlemlerden biri başarısız olursa değişiklikleri geri al.
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            // Yeni rolü ekle.
            if (!currentRoles.Contains(role))
            {
                var addResult = await _userManager.AddToRoleAsync(user, role);

                if (!addResult.Succeeded)
                {
                    TempData["Error"] = "Yeni rol atanamadı.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Eski rolleri kaldır.
            var rolesToRemove = currentRoles
                .Where(x => x != role)
                .ToArray();

            if (rolesToRemove.Length > 0)
            {
                var removeResult = await _userManager
                    .RemoveFromRolesAsync(user, rolesToRemove);

                if (!removeResult.Succeeded)
                {
                    TempData["Error"] = "Eski roller kaldırılamadı.";
                    return RedirectToAction(nameof(Index));
                }
            }

            await transaction.CommitAsync();

            TempData["Success"] = "Kullanıcı rolü güncellendi.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, bool isActive)
        {
            // Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound();


            // Admin kendi hesabını pasife alamaz
            if (_userManager.GetUserId(User) == user.Id.ToString())
            {
                TempData["Error"] =
                    "Kendi hesabınızın durumunu değiştiremezsiniz.";

                return RedirectToAction(nameof(Index));
            }


            // Admin hesaplarının durumu değiştirilemez
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["Error"] =
                    "Admin hesaplarının durumu değiştirilemez.";

                return RedirectToAction(nameof(Index));
            }


            // Kullanıcıyı aktif veya pasif yap
            user.LockoutEnabled = true;

            user.LockoutEnd = isActive
                ? null
                : DateTimeOffset.MaxValue;


            // Veritabanına kaydet
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] =
                    "Hesap durumu güncellenemedi.";

                return RedirectToAction(nameof(Index));
            }


            // Kullanıcının eski oturumlarını geçersiz kıl
            var stampResult =
                await _userManager.UpdateSecurityStampAsync(user);

            if (!stampResult.Succeeded)
            {
                TempData["Error"] =
                    "Hesap durumu değişti ancak oturumlar geçersiz kılınamadı.";

                return RedirectToAction(nameof(Index));
            }


            TempData["Success"] = isActive
                ? "Kullanıcı hesabı aktif edildi."
                : "Kullanıcı hesabı pasife alındı.";

            return RedirectToAction(nameof(Index));
        }





    }
}
