using ApiConsumer.Models;
using ApiConsumer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiConsumerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        public ApiConsumerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = _configuration["ApiUrl"];
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var apiConsumer = new ApiConsumerService();
                var result = await apiConsumer.ApiGet(new ApiConsumerDto($"{_baseUrl}/api/PostgreSQL/api/ExampleModels"));
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var apiConsumer = new ApiConsumerService();
                var result = await apiConsumer.ApiDelete(new ApiConsumerDto($"{_baseUrl}/api/PostgreSQL/api/ExampleModels/{id}"));
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ExampleModel data)
        {
            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(data),System.Text.Encoding.UTF8, "application/json");
                var apiConsumer = new ApiConsumerService();
                var result = await apiConsumer.ApiPost(new ApiConsumerDto($"{_baseUrl}/api/PostgreSQL/api/ExampleModels", body));
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, ExampleModel data)
        {
            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var apiConsumer = new ApiConsumerService();
                var result = await apiConsumer.ApiPut(new ApiConsumerDto($"{_baseUrl}/api/PostgreSQL/api/ExampleModels/{id}", body));
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
