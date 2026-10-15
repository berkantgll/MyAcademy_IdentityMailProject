
namespace IdentityMail.Web.Entities
{
    public class MailMessageCategory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int UserMessageId { get; set; }

        public int MailCategoryId { get; set; }

        public UserMessage UserMessage { get; set; } = null!;

        public MailCategory MailCategory { get; set; } = null!;
    }
}
