using IdentityMail.Web.Context;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace IdentityMail.Web.Controllers
{
    [Authorize]
    public class CategoryController(
        UserManager<AppUser> _userManager,
        AppDbContext _context) : Controller
    {
        // 1. KATEGORİLERİ LİSTELE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var categories = await _context.MailCategories
                .Where(x => x.UserId == user.Id)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(categories);
        }


        // 2. YENİ KATEGORİ OLUŞTUR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string? name, string? color)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
            {
                TempData["CategoryError"] =
                    "Kategori adı 1-50 karakter arasında olmalıdır.";

                return RedirectToAction(nameof(Index));
            }

            name = name.Trim();

            var exists = await _context.MailCategories
                .AnyAsync(x =>
                    x.UserId == user.Id &&
                    x.Name.ToLower() == name.ToLower());

            if (exists)
            {
                TempData["CategoryError"] =
                    "Bu isimde bir kategorin zaten var.";

                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(color) ||
                !Regex.IsMatch(color, @"^#[0-9a-fA-F]{6}$"))
            {
                color = "#8b5cf6";
            }

            var category = new MailCategory
            {
                UserId = user.Id,
                Name = name,
                Color = color
            };

            _context.MailCategories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // 3. KATEGORİ SİL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var category = await _context.MailCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == id && x.UserId == user.Id);

            if (category == null) return NotFound();

            _context.MailCategories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // 4. KATEGORİDEKİ MESAJLARI LİSTELE

        [HttpGet]
        public async Task<IActionResult> Messages(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            // Kullanıcıya ait kategoriyi bul
            var category = await _context.MailCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == id && x.UserId == user.Id);

            if (category == null)
                return NotFound();

            // Kategoriye ait mesajları getir
            var messages = await _context.UserMessages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x =>
                    _context.MailMessageCategories.Any(mc =>
                        mc.UserMessageId == x.Id &&
                        mc.MailCategoryId == id &&
                        mc.UserId == user.Id)
                    &&
                    (
                        (x.ReceiverId == user.Id && !x.IsTrashedByReceiver)
                        ||
                        (x.SenderId == user.Id && !x.IsTrashedBySender)
                    ))
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryColor = category.Color;
            ViewBag.CategoryId = category.Id;
            ViewBag.CurrentUserId = user.Id;

            return View(messages);
        }



        // 5. MESAJI KATEGORİYE EKLE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(
            int messageId, int categoryId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var categoryExists = await _context.MailCategories
                .AnyAsync(x =>
                    x.Id == categoryId && x.UserId == user.Id);

            var messageExists = await _context.UserMessages
                .AnyAsync(x =>
                    x.Id == messageId &&
                    (
                        (x.ReceiverId == user.Id &&
                         !x.IsTrashedByReceiver)
                        ||
                        (x.SenderId == user.Id &&
                         !x.IsTrashedBySender)
                    ));

            if (!categoryExists || !messageExists)
                return NotFound();

            var alreadyAssigned = await _context.MailMessageCategories
                .AnyAsync(x =>
                    x.UserId == user.Id &&
                    x.UserMessageId == messageId &&
                    x.MailCategoryId == categoryId);

            if (!alreadyAssigned)
            {
                _context.MailMessageCategories.Add(
                    new MailMessageCategory
                    {
                        UserId = user.Id,
                        UserMessageId = messageId,
                        MailCategoryId = categoryId
                    });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                nameof(Messages), new { id = categoryId });
        }


        // 6. MESAJI KATEGORİDEN ÇIKAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unassign(
            int messageId, int categoryId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var categoryExists = await _context.MailCategories
                .AnyAsync(x =>
                    x.Id == categoryId && x.UserId == user.Id);

            if (!categoryExists) return NotFound();

            var assignment = await _context.MailMessageCategories
                .FirstOrDefaultAsync(x =>
                    x.UserId == user.Id &&
                    x.UserMessageId == messageId &&
                    x.MailCategoryId == categoryId);

            if (assignment == null) return NotFound();

            _context.MailMessageCategories.Remove(assignment);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Messages), new { id = categoryId });
        }
    }
}
