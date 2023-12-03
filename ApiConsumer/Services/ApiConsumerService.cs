using Newtonsoft.Json.Linq;

namespace ApiConsumer.Services
{
    public class ApiConsumerDto
    {
        public ApiConsumerDto() { }

        public ApiConsumerDto(string url)
        {
            Url = url;
        }

        public ApiConsumerDto(string url, StringContent body)
        {
            Url = url;
            Body = body;
        }

        public string Url { get; set; }
        public StringContent Body { get; set; }
        public string MediaType { get; set; } = "application/json";
        public string Token { get; set; }
    }
    public class ApiConsumerService
    {
        public async Task<string> ApiGet(ApiConsumerDto consumerDto)
        {
            try
            {
                string? result = null;
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                if (!string.IsNullOrEmpty(consumerDto.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer {consumerDto.Token}");
                }

                var response = await client.GetAsync(consumerDto.Url);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                return result;
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
        public async Task<string> ApiDelete(ApiConsumerDto consumerDto)
        {
            try
            {
                string? result = null;
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(consumerDto.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer {consumerDto.Token}");
                }

                var response = await client.DeleteAsync(consumerDto.Url);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                return result;
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
        public async Task<string> ApiPost(ApiConsumerDto consumerDto)
        {
            try
            {
                string? result = null;
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(consumerDto.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer {consumerDto.Token}");
                }

                var response = await client.PostAsync(consumerDto.Url,consumerDto.Body);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                return result;
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
        public async Task<string> ApiPut(ApiConsumerDto consumerDto)
        {
            try
            {
                string? result = null;
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(consumerDto.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer {consumerDto.Token}");
                }

                var response = await client.PutAsync(consumerDto.Url, consumerDto.Body);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                return result;
            }
            catch (Exception)
            {
                return "ERROR";
                throw;
            }
        }
    }
}
