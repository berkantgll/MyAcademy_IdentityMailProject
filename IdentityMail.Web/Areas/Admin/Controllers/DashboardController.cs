
using IdentityMail.Web.Areas.Admin.ViewModels;
using IdentityMail.Web.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(
        AppDbContext _context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // BUGÜNÜN TARİH ARALIĞI
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);


            // DASHBOARD VERİLERİ
            var model = new AdminDashboardViewModel
            {
                // TOPLAM KULLANICI
                TotalUsers = await _context.Users
                    .CountAsync(),


                // AKTİF KULLANICI
                ActiveUsers = await _context.Users
                    .CountAsync(x =>
                        !x.LockoutEnabled ||
                        x.LockoutEnd != DateTimeOffset.MaxValue),


                // TOPLAM MESAJ
                TotalMessages = await _context.UserMessages
                    .CountAsync(),


                // BUGÜN GÖNDERİLEN MESAJLAR
                TodayMessages = await _context.UserMessages
                    .CountAsync(x =>
                        x.SendDate >= today &&
                        x.SendDate < tomorrow),


                // OKUNMAMIŞ MESAJLAR
                UnreadMessages = await _context.UserMessages
                    .CountAsync(x => !x.IsRead),


                // ÇÖP KUTUSUNDAKİ MESAJLAR
                TrashedMessages = await _context.UserMessages
                    .CountAsync(x =>
                        x.IsTrashedBySender ||
                        x.IsTrashedByReceiver)
            };


            // EN ÇOK MESAJ GÖNDEREN 5 KULLANICI
            model.TopSenders = await (
                from message in _context.UserMessages.AsNoTracking()

                join user in _context.Users.AsNoTracking()
                    on message.SenderId equals user.Id

                group message by new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName
                }
                into grouped

                select new TopSenderViewModel
                {
                    UserId = grouped.Key.Id,

                    Name =
                        (grouped.Key.FirstName ?? "") +
                        " " +
                        (grouped.Key.LastName ?? ""),

                    MessageCount = grouped.Count()
                }
            )
            .OrderByDescending(x => x.MessageCount)
            .ThenBy(x => x.Name)
            .Take(5)
            .ToListAsync();


            // EN ÇOK KULLANILAN 5 KATEGORİ
            model.TopCategories = await (
                from messageCategory in
                    _context.MailMessageCategories.AsNoTracking()

                join category in _context.MailCategories.AsNoTracking()
                    on messageCategory.MailCategoryId equals category.Id

                group messageCategory by category.Name
                into grouped

                select new TopCategoryViewModel
                {
                    CategoryName = grouped.Key,

                    UsageCount = grouped.Count()
                }
            )
            .OrderByDescending(x => x.UsageCount)
            .ThenBy(x => x.CategoryName)
            .Take(5)
            .ToListAsync();


            return View(model);
        }
    }
}
