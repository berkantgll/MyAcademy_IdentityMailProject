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
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.fullName = user.FirstName + " " + user.LastName;

            var messages = await _context.UserMessages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id && !x.IsTrashedByReceiver).ToListAsync();

            ViewBag.Categories = await _context.MailCategories
                .Where(x => x.UserId == user.Id)
                .OrderBy(x => x.Name)
                .ToListAsync();

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


    }
}
