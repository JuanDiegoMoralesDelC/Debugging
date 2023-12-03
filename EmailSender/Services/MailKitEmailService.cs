using EmailSender.Interfaces;
using EmailSender.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;

namespace EmailSender.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger _logger;
        public MailKitEmailService(IOptions<EmailConfiguration> emailConfiguration, ILogger logger)
        {
            _emailConfiguration = emailConfiguration.Value;
            _logger = logger;
        }

        public async Task<bool> SendTextEmail(EmailDto emailDto, EmailAttachmentDto attachmentDto)
        {
            var methodName = $"{GetType().Name}.{MethodBase.GetCurrentMethod()!.Name}";
            try
            {
                _logger.Log(LogLevel.Information, $"Inicio: {methodName}");
                _logger.Log(LogLevel.Information, $"{methodName}.emailDto: {JsonSerializer.Serialize(emailDto)}");

                var fromEmail = new MailboxAddress(emailDto.From, emailDto.FromName);
                var toEmail = new MailboxAddress(emailDto.To, emailDto.ToName);

                var bodyFormat = emailDto.IsHtml ? TextFormat.Html : TextFormat.Plain;
                var messageBody = new TextPart(bodyFormat)
                {
                    Text = emailDto.Body
                };
                var body = new Multipart("multipart/mixed")
                {
                    messageBody
                };

                if (attachmentDto.HasAttachment)
                {
                    var messageAttachment = new MimePart(attachmentDto.MediaType)
                    {
                        Content = new MimeContent(attachmentDto.File),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = attachmentDto.FileName
                    };
                    body.Add(messageAttachment);
                }

                var message = new MimeMessage()
                {
                    From = { fromEmail },
                    To = { toEmail },
                    Subject = emailDto.Subject,
                    Body = body
                };

                using var client = new MailKit.Net.Smtp.SmtpClient()
                {
                    Timeout = 10000
                };
                await client.ConnectAsync(
                    _emailConfiguration.Host,
                    _emailConfiguration.Port,
                    MailKit.Security.SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();

                return true;

            }
            catch (Exception e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Error, $"{methodName}: {logMessage}");
                return false;
            }
            finally
            {
                _logger.Log(LogLevel.Information, $"Fin: {methodName}");
            }
        }
    }
}
