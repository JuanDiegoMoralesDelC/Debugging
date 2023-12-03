using EmailSender.Interfaces;
using EmailSender.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;

namespace EmailSender.Services
{
    public class DotNetEmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger _logger;
        public DotNetEmailService(IOptions<EmailConfiguration> emailConfiguration, ILogger logger)
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

                var fromEmail = new MailAddress(emailDto.From, emailDto.FromName);
                var toEmail = new MailAddress(emailDto.To, emailDto.ToName);

                var email = new MailMessage(fromEmail, toEmail)
                {
                    Subject = emailDto.Subject,
                    Body = emailDto.Body,
                    IsBodyHtml = emailDto.IsHtml
                };

                if(attachmentDto.HasAttachment)
                {
                    var attachment = new Attachment(attachmentDto.File, attachmentDto.FileName, attachmentDto.MediaType);
                    email.Attachments.Add(attachment);
                }

                var smtpClient = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Host = _emailConfiguration.Host,
                    Port = _emailConfiguration.Port,
                    Credentials = new System.Net.NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password),
                    EnableSsl = _emailConfiguration.EnableSsl,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                using var client = smtpClient;
                client.Send(email);
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
