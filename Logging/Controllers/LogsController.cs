using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;
        private readonly IHostEnvironment _env;
        public LogsController(ILogger<LogsController> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet("{date}")]
        public IActionResult Logs(string date)
        {
            var methodName = $"{GetType().Name}.{MethodBase.GetCurrentMethod()!.Name}";
            try
            {
                _logger.Log(LogLevel.Information, $"Inicio: {methodName}");

                var directoryPath = Path.Combine(_env.ContentRootPath, "Logs");
                if (date.Length != 8) return NotFound();
                var fileName = $"{date}_Logs.txt";
                var filePath = Path.Combine(directoryPath, fileName);
                if(!System.IO.File.Exists(filePath)) return NotFound();
                var file = System.IO.File.ReadAllText(filePath);

                return Ok(file);

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
