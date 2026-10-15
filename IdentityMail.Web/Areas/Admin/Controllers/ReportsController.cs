
using IdentityMail.Web.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportsController(
        AppDbContext _context) : Controller
    {
        // ŞİKAYETLERİ LİSTELE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reports = await _context.MessageReports
                .AsNoTracking()
                .OrderBy(x => x.IsReviewed)
                .ThenByDescending(x => x.ReportedAt)
                .ToListAsync();

            return View(reports);
        }


        // ŞİKAYET DETAYI
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Şikayeti bul
            var report = await _context.MessageReports
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (report == null)
                return NotFound();


            // Şikayet edilen mesajı getir
            var message = await _context.UserMessages
                .AsNoTracking()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(
                    x => x.Id == report.UserMessageId);

            if (message == null)
                return NotFound();


            // Şikayeti yapan kullanıcıyı getir
            var reporter = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == report.ReporterId);


            ViewBag.Message = message;
            ViewBag.Reporter = reporter;

            return View(report);
        }


        // İNCELENDİ OLARAK İŞARETLE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkReviewed(int id)
        {
            var report = await _context.MessageReports
                .FirstOrDefaultAsync(x => x.Id == id);

            if (report == null)
                return NotFound();


            // Zaten incelenmişse tekrar güncelleme
            if (!report.IsReviewed)
            {
                report.IsReviewed = true;

                await _context.SaveChangesAsync();
            }


            TempData["Success"] =
                "Şikayet incelendi olarak işaretlendi.";

            return RedirectToAction(
                nameof(Details),
                new { id = report.Id });
        }
    }
}
