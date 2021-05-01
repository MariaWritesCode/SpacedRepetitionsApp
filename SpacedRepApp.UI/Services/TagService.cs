using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace SpacedRepApp.UI.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve };

        private readonly string requestUrl;

        public TagService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            requestUrl = _configuration["Settings:TagAPIEndpoint"];
        }

        public async Task CreateTag(Tag newTag)
        {
            var jsonStringTag = JsonSerializer.Serialize(newTag, jsonOptions);
            var content = new StringContent(jsonStringTag, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        public async Task DeleteTag(long tagId)
        {
            var response = await _httpClient.DeleteAsync($"{requestUrl}/{tagId}");

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        public async Task<List<Tag>> GetAvailableTags()
        {
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Tag>>(responseContent, jsonOptions);
            }
            return default;
        }
    }
}
