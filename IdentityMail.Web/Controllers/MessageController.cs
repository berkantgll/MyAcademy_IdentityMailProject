using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.DraftMessageDtos;
using IdentityMail.Web.DTOs.UserMessageDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize]
    public class MessageController(UserManager<AppUser> _userManager, AppDbContext _context) : Controller
    {
        // GELEN KUTUSU - ARAMA, FİLTRELEME VE SAYFALAMA
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            bool starred = false,
            int? categoryId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string sort = "newest",
            int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            ViewBag.fullName = user.FirstName + " " + user.LastName;

            var query = _context.UserMessages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && !x.IsTrashedByReceiver);

            // Okunmamış mesaj sayısı filtrelerden etkilenmesin.
            ViewBag.UnreadCount = await query.CountAsync(x => !x.IsRead);

            // Gönderen adı veya mesaj konusu ile ara.
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.Subject.Contains(search) ||
                    x.Sender.FirstName.Contains(search) ||
                    x.Sender.LastName.Contains(search) ||
                    (x.Sender.FirstName + " " + x.Sender.LastName).Contains(search));
            }

            // Okundu / okunmadı filtresi
            if (status == "read")
                query = query.Where(x => x.IsRead);
            else if (status == "unread")
                query = query.Where(x => !x.IsRead);

            // Yıldızlı mesajlar
            if (starred)
                query = query.Where(x => x.IsStarred);

            // Kategori filtresi (yalnızca kullanıcının kendi kategorileri)
            if (categoryId.HasValue)
            {
                query = query.Where(x => _context.MailMessageCategories.Any(c =>
                    c.UserId == user.Id &&
                    c.UserMessageId == x.Id &&
                    c.MailCategoryId == categoryId.Value));
            }

            // Tarih aralığı
            if (startDate.HasValue)
                query = query.Where(x => x.SendDate >= startDate.Value.Date);

            if (endDate.HasValue)
            {
                var nextDay = endDate.Value.Date.AddDays(1);
                query = query.Where(x => x.SendDate < nextDay);
            }

            // Filtrelere uyan toplam mesaj sayısı
            int totalMessages = await query.CountAsync();

            // Sayfalama: her sayfada 10 mesaj
            const int pageSize = 10;
            int totalPages = (int)Math.Ceiling(totalMessages / (double)pageSize);

            if (page < 1)
                page = 1;

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            // Tarihe göre sırala
            var ordered = sort == "oldest"
                ? query.OrderBy(x => x.SendDate).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.SendDate).ThenByDescending(x => x.Id);

            var messages = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Categories = await _context.MailCategories
                .Where(x => x.UserId == user.Id)
                .OrderBy(x => x.Name)
                .ToListAsync();

            // Seçilen filtreler View'a geri gönderilir.
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Starred = starred;
            ViewBag.CategoryId = categoryId;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.Sort = sort;
            ViewBag.TotalMessages = totalMessages;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(messages);
        }

        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(SendMailDto sendMailDto)
        {
            if (!ModelState.IsValid)
            {
                return View(sendMailDto);
            }

            var sender = await _userManager.FindByNameAsync(User.Identity.Name);

            var receiver = await _userManager.FindByEmailAsync(sendMailDto.ReceiverMail);

            if (receiver is null)
            {
                ModelState.AddModelError(string.Empty, "Sistemde kayıtlı kullanıcı bulunamadı!");

                return View(sendMailDto);
            }

            var newMessage = new UserMessage
            {
                SendDate = DateTime.Now,
                ReceiverId = receiver.Id,
                SenderId = sender.Id,
                Subject = sendMailDto.Subject,
                Body = sendMailDto.Body
            };

            _context.UserMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> MailDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var message = await _context.UserMessages
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(x =>
                    x.Id == id && x.ReceiverId == user.Id);

            if (message == null)
            {
                return NotFound();
            }

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

        public async Task<IActionResult> Sent()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var messages = await _context.UserMessages
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == user.Id && !x.IsTrashedBySender)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            return View(messages);
        }

        public async Task<IActionResult> SentDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var messages = await _context.UserMessages
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x => x.Id == id && x.SenderId == user.Id);

            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        public async Task<IActionResult> Reply(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var message = await _context.UserMessages
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(x => x.Id == id && x.ReceiverId == user.Id);

            if (message == null)
            {
                return NotFound();
            }

            var replyDto = new SendMailDto()
            {
                ReceiverMail = message.Sender.Email,
                Subject = $"Re: {message.Subject}"
            };

            return View("SendMail", replyDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStar(
            int id, bool returnToStarred = false)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.ReceiverId == user.Id);

            if (message == null)
            {
                return NotFound();
            }

            message.IsStarred = !message.IsStarred;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                returnToStarred ? nameof(Starred) : nameof(Index));
        }



        public async Task<IActionResult> Starred()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var messages = await _context.UserMessages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && x.IsStarred)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(SaveDraftDto saveDraftDto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var draft = new DraftMessage
            {
                UserId = user.Id,
                ReceiverMail = saveDraftDto.ReceiverMail,
                Subject = saveDraftDto.Subject,
                Body = saveDraftDto.Body,
                UpdatedAt = DateTime.UtcNow
            };

            _context.DraftMessages.Add(draft);
            await _context.SaveChangesAsync();


            return RedirectToAction("Drafts");
        }

        public async Task<IActionResult> Drafts()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var drafts = await _context.DraftMessages
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.UpdatedAt)
                .ToListAsync();

            return View(drafts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDraft(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var draft = await _context.DraftMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id && x.UserId == user.Id);

            if (draft == null)
                return NotFound();

            _context.DraftMessages.Remove(draft);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Drafts));
        }


        [HttpGet]
        public async Task<IActionResult> EditDraft(int id)
        {
            // Giriş yapan kullanıcı
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Düzenlenecek taslağı bul
            var draft = await _context.DraftMessages
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);


            if (draft == null)
            {
                return NotFound();
            }

            return View(draft);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDraft(int id, SaveDraftDto saveDraftDto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var draft = await _context.DraftMessages
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

            if (draft == null)
            {
                return NotFound();
            }

            draft.ReceiverMail = saveDraftDto.ReceiverMail;
            draft.Subject = saveDraftDto.Subject;
            draft.Body = saveDraftDto.Body;
            draft.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Drafts));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendDraft(int id, SendMailDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var draft = await _context.DraftMessages
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

            if (draft == null) return NotFound();

            if (!ModelState.IsValid)
                return View("EditDraft", draft);

            var receiver = await _userManager.FindByEmailAsync(dto.ReceiverMail);

            if (receiver == null)
            {
                ModelState.AddModelError("ReceiverMail", "Alıcı Nexora'da kayıtlı değil.");
                return View("EditDraft", draft);
            }

            var message = new UserMessage
            {
                SenderId = user.Id,
                ReceiverId = receiver.Id,
                Subject = dto.Subject,
                Body = dto.Body,
                SendDate = DateTime.Now
            };

            _context.UserMessages.Add(message);
            _context.DraftMessages.Remove(draft);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Sent));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToTrash(
            int id, bool fromSent = false)
        {
            // Giriş yapan kullanıcıyı bul
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            // Kullanıcıya ait mesajı bul
            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    (fromSent
                        ? x.SenderId == user.Id
                        : x.ReceiverId == user.Id));

            if (message == null)
                return NotFound();

            // Mesajı ilgili kullanıcının çöp kutusuna taşı
            if (fromSent)
                message.IsTrashedBySender = true;
            else
                message.IsTrashedByReceiver = true;

            // Veritabanını güncelle
            await _context.SaveChangesAsync();

            // Geldiği sayfaya dön
            return RedirectToAction(
                fromSent ? nameof(Sent) : nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Challenge();

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.ReceiverId == user.Id &&
                    !x.IsTrashedByReceiver);

            if (message == null) return NotFound();

            message.IsRead = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var messages = await _context.UserMessages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x =>
                    (x.ReceiverId == user.Id && x.IsTrashedByReceiver) ||
                    (x.SenderId == user.Id && x.IsTrashedBySender))
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            ViewBag.CurrentUserId = user.Id;

            return View(messages);
        }


        // ÇÖP KUTUSUNDAN GERİ YÜKLE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreFromTrash(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    ((x.ReceiverId == user.Id && x.IsTrashedByReceiver) ||
                     (x.SenderId == user.Id && x.IsTrashedBySender)));

            if (message == null)
                return NotFound();

            if (message.ReceiverId == user.Id)
                message.IsTrashedByReceiver = false;

            if (message.SenderId == user.Id)
                message.IsTrashedBySender = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Trash));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(
            int messageId,
            string reason)
        {
            // 1. Giriş yapan kullanıcıyı bul
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();


            // 2. Şikayet nedenini kontrol et
            if (string.IsNullOrWhiteSpace(reason) ||
                reason.Trim().Length > 500)
            {
                TempData["Error"] =
                    "Şikayet nedeni 1-500 karakter arasında olmalıdır.";

                return RedirectToAction(nameof(Index));
            }


            // 3. Mesaj gerçekten bu kullanıcıya mı gelmiş?
            var messageExists = await _context.UserMessages
                .AnyAsync(x =>
                    x.Id == messageId &&
                    x.ReceiverId == user.Id);

            if (!messageExists)
                return NotFound();


            // 4. Kullanıcı bu mesajı daha önce şikayet etmiş mi?
            var alreadyReported = await _context.MessageReports
                .AnyAsync(x =>
                    x.UserMessageId == messageId &&
                    x.ReporterId == user.Id);

            if (alreadyReported)
            {
                TempData["Error"] =
                    "Bu mesajı daha önce şikayet ettiniz.";

                return RedirectToAction(nameof(Index));
            }


            // 5. Yeni şikayet kaydını oluştur
            var report = new MessageReport
            {
                UserMessageId = messageId,
                ReporterId = user.Id,
                Reason = reason.Trim(),
                ReportedAt = DateTime.UtcNow,
                IsReviewed = false
            };


            // 6. Veritabanına kaydet
            await _context.MessageReports.AddAsync(report);

            await _context.SaveChangesAsync();


            // 7. Kullanıcıya bilgi ver
            TempData["Success"] =
                "Mesaj şikayetiniz başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }



    }
}
