namespace IdentityMail.Web.DTOs.DraftMessageDtos
{
    public class SaveDraftDto
    {
        public string? ReceiverMail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
