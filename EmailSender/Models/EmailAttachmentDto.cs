namespace EmailSender.Models
{
    public class EmailAttachmentDto
    {
        public bool HasAttachment { get; set; }
        public MemoryStream File { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
    }
}
