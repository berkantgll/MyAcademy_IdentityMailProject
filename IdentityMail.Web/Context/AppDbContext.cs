
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // GÖNDERİLEN MESAJ İLİŞKİSİ
            builder.Entity<AppUser>()
                .HasMany(message => message.SentMessages)
                .WithOne(s => s.Sender)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


            // GELEN MESAJ İLİŞKİSİ
            builder.Entity<AppUser>()
                .HasMany(message => message.ReceivedMessages)
                .WithOne(s => s.Receiver)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);


            // MESAJ KATEGORİ İLİŞKİLERİ
            builder.Entity<MailMessageCategory>(entity =>
            {
                // Mesaj ile ilişki
                entity.HasOne(x => x.UserMessage)
                    .WithMany()
                    .HasForeignKey(x => x.UserMessageId)
                    .OnDelete(DeleteBehavior.Cascade);


                // Kategori ile ilişki
                entity.HasOne(x => x.MailCategory)
                    .WithMany()
                    .HasForeignKey(x => x.MailCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);


                // Aynı kategori aynı mesaja iki kez atanmasın
                entity.HasIndex(x => new
                {
                    x.UserId,
                    x.UserMessageId,
                    x.MailCategoryId
                }).IsUnique();
            });


            // MESAJ ŞİKAYET İLİŞKİLERİ
            builder.Entity<MessageReport>(entity =>
            {
                // Şikayet edilen mesaj
                entity.HasOne<UserMessage>()
                    .WithMany()
                    .HasForeignKey(x => x.UserMessageId)
                    .OnDelete(DeleteBehavior.Restrict);


                // Şikayeti oluşturan kullanıcı
                entity.HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey(x => x.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);


                // Şikayet nedeni zorunlu
                entity.Property(x => x.Reason)
                    .IsRequired()
                    .HasMaxLength(500);


                // Aynı kullanıcı aynı mesajı
                // bir kez şikayet edebilsin
                entity.HasIndex(x => new
                {
                    x.ReporterId,
                    x.UserMessageId
                }).IsUnique();
            });
        }


        // MESAJLAR
        public DbSet<UserMessage> UserMessages { get; set; }

        // TASLAKLAR
        public DbSet<DraftMessage> DraftMessages { get; set; }

        // KATEGORİLER
        public DbSet<MailCategory> MailCategories { get; set; }

        // MESAJ KATEGORİ EŞLEŞTİRMELERİ
        public DbSet<MailMessageCategory> MailMessageCategories { get; set; }

        // MESAJ ŞİKAYETLERİ
        public DbSet<MessageReport> MessageReports { get; set; }
    }
}
