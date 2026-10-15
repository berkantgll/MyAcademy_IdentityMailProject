namespace IdentityMail.Web.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsImportant { get; set; }
        public bool IsStarred { get; set; }
        public bool IsTrashedBySender { get; set; }
        public bool IsTrashedByReceiver { get; set; }


        // AppUser yani kullanıcı entitysinde hem alıcı var hem gönderici var.
        // AppUser hem mail alabilir hem mail gönderebilir.
        public AppUser Sender { get; set; } //Gönderen
        public int SenderId { get; set; } // Gönderenin Id
        public AppUser Receiver { get; set; } // Alan
        public int ReceiverId { get; set; } // Alıcının Id
    }
}
