using EmailSender.Models;

namespace EmailSender.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendTextEmail(EmailDto emailDto, EmailAttachmentDto attachmentDto);
    }
}
