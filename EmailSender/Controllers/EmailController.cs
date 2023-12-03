using EmailSender.Interfaces;
using EmailSender.Models;
using EmailSender.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;

namespace EmailSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IHostEnvironment _env;
        private readonly IOptions<EmailConfiguration> _emailConfigurationOptions;
        public EmailController(ILogger<EmailController> logger, IHostEnvironment env, IOptions<EmailConfiguration> emailConfigurationOptions)
        {
            _logger = logger;
            _env = env;
            _emailConfigurationOptions = emailConfigurationOptions;
        }

        private async Task<bool> SendEmail(IEmailService emailService, string subject)
        {

            #region Generacion de Objeto EmailDto
            var emailDto = new EmailDto()
            {
                To = "jdmdelc@gmail.com",
                ToName = "jdmdelc@gmail.com",
                From = _emailConfigurationOptions.Value.UserName,
                FromName = _emailConfigurationOptions.Value.UserName,
                Subject = subject,
                Body = "<h1>Titulo de prueba</h1><p>Paragrafo de pruebas</p>",
                IsHtml = true,
            };
            #endregion

            #region Carga de archivo a memoria
            var directoryPath = Path.Combine(_env.ContentRootPath, "Logs");
            var fileName = $"{DateTime.UtcNow:yyyyMMdd}_Logs.txt";
            var filePath = Path.Combine(directoryPath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var file = new MemoryStream(fileBytes);
            #endregion

            #region Generacion de adjunto EmailAttachmentDto
            var attachmentDto = new EmailAttachmentDto()
            {
                HasAttachment = true,
                File = file,
                FileName = fileName,
                MediaType = "application/text"
            };
            #endregion

            return await emailService.SendTextEmail(emailDto, attachmentDto);
        }

        [HttpGet("DotNetMail")]
        public async Task<IActionResult> DotNetMail()
        {
            var methodName = $"{GetType().Name}.{MethodBase.GetCurrentMethod()!.Name}";
            try
            {
                _logger.Log(LogLevel.Information, $"Inicio: {methodName}");

                var result = await SendEmail(new DotNetEmailService(_emailConfigurationOptions, _logger), "DotNetMail");

                return Ok(result);

            }
            catch (Exception e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Error, $"{methodName}: {logMessage}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.Log(LogLevel.Information, $"Fin: {methodName}");
            }

        }

        [HttpGet("MailKitMail")]
        public async Task<IActionResult> MailKitMail()
        {
            var methodName = $"{GetType().Name}.{MethodBase.GetCurrentMethod()!.Name}";
            try
            {
                _logger.Log(LogLevel.Information, $"Inicio: {methodName}");

                var result = await SendEmail(new MailKitEmailService(_emailConfigurationOptions, _logger), "MailKitMail");
                return Ok(result);

            }
            catch (Exception e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Error, $"{methodName}: {logMessage}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.Log(LogLevel.Information, $"Fin: {methodName}");
            }

        }
    }
}
