namespace IdentityMail.Web.Entities
{
    public class DraftMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ReceiverMail { get; set; }
        public string? Subject {  get; set; }
        public string? Body { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
