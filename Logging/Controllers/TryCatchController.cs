using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TryCatchController : ControllerBase
    {
        private readonly ILogger<TryCatchController> _logger;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public TryCatchController(ILogger<TryCatchController> logger, IHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            _configuration = configuration;

        }

        [HttpGet]
        public IActionResult Get(int errorType)
        {
            var methodName = $"{GetType().Name}.{MethodBase.GetCurrentMethod()!.Name}";
            try
            {
                _logger.Log(LogLevel.Information, $"Inicio: {methodName}");
                switch(errorType)
                {
                    case 0:
                        #region IndexOutOfRangeException
                        var listaLetras = new string[] { "a", "b", "c" };
                        Console.WriteLine(listaLetras[listaLetras.Length]); //Aqui deberia fallar
                        //Console.WriteLine(listaLetras[listaLetras.Length-1]); //Aqui deberia funcionar
                        #endregion
                        break;
                    case 1:
                        #region NullReferenceException
                        List<string> listaNumeros = null; //Aqui deberia fallar
                        //List<string> listaNumeros = new List<string>(); //Aqui deberia funcionar
                        Console.WriteLine(listaNumeros.ToString());
                        #endregion
                        break;
                    case 2:
                        #region ArgumentOutOfRangeException
                        var lista = new List<string>();
                        //lista.Add(""); //Aqui deberia funcionar
                        Console.WriteLine(lista[0]); //Aqui deberia fallar
                        #endregion
                        break;
                    case 3:
                        #region DirectoryNotFoundException
                        var directory = _env.ContentRootPath;
                        var noExistingDirectory = Path.Combine(directory, "test", "appsettings.json"); //Aqui deberia fallar
                        //var noExistingDirectory = Path.Combine(directory, "appsettings.json"); //Aqui deberia funcionar
                        Console.WriteLine(System.IO.File.ReadAllText(noExistingDirectory));
                        #endregion
                        break;
                    case 4:
                        #region FileNotFoundException
                        var path = _env.ContentRootPath;
                        var filePath = Path.Combine(path, "appsettings.Production.json"); //Aqui deberia fallar
                        //var filePath = Path.Combine(path, "appsettings.json"); //Aqui deberia funcionar
                        Console.WriteLine(System.IO.File.ReadAllText(filePath));
                        #endregion
                        break;
                    case 5:
                        #region Exception
                        var verion = int.Parse(_configuration["InformationConfig:Version"]);
                        #endregion
                        break;
                    default:
                        throw new NotImplementedException("Sin implementar");
                }
            }
            catch (IndexOutOfRangeException e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null? "" :$"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Warning, $"{methodName}: {logMessage}");
                return BadRequest();
            }
            catch (NullReferenceException e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Warning, $"{methodName}: {logMessage}");
                return BadRequest();
            }
            catch (ArgumentOutOfRangeException e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Warning, $"{methodName} : {logMessage}");
                return BadRequest();
            }
            catch (DirectoryNotFoundException e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Warning, $"{methodName}: {logMessage}");
                return BadRequest();
            }
            catch (FileNotFoundException e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Warning, $"{methodName}: {logMessage}");
                return BadRequest();
            }
            catch (NotImplementedException e)
            {
                var logMessage = $"{e.GetType()}, Message: {e.Message}" +
                    (e.InnerException == null ? "" : $"\nInnerException: {e.InnerException}");
                _logger.Log(LogLevel.Warning, $"{methodName}: {logMessage}");
                return BadRequest();
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
            return Ok();
        }
    }
}
