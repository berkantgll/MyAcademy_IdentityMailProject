
namespace IdentityMail.Web.Entities
{
    public class MailCategory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Color { get; set; } = "#8b5cf6";
    }
}
