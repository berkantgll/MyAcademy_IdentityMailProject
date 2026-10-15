
namespace IdentityMail.Web.Entities
{
    public class MessageReport
    {
        public int Id { get; set; }

        public int UserMessageId { get; set; }

        public int ReporterId { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public bool IsReviewed { get; set; } = false;
    }
}
