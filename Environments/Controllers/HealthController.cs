using Environments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Environments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IOptions<InformationConfig> _informationConfig;
        private readonly IConfiguration _configuration;
        public HealthController(IOptions<InformationConfig> informationConfig, IConfiguration configuration)
        {
            _informationConfig = informationConfig;
            _configuration = configuration;
        }

        [HttpGet("IOptions")]
        public IActionResult GetIOptions()
        {
            var config = _informationConfig.Value;
            return Ok(config);
        }

        [HttpGet("IConfiguration")]
        public IActionResult GetIConfiguration()
        {
            var config = "Version: " + _configuration["InformationConfig:Version"] + "\nEnvironment: " + _configuration["InformationConfig:Environment"];
            return Ok(config);
        }
    }
}
