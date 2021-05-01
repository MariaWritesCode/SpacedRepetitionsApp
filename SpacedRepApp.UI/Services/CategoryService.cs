using Microsoft.Extensions.Configuration;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private static IConfiguration _configuration;

        private readonly string requestUrl;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public CategoryService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            requestUrl = _configuration["Settings:CategoryAPIEndpoint"];            
        }

        public async Task AddCategory(Category category)
        {
            var jsonStringCategory = JsonSerializer.Serialize(category, jsonOptions);
                
            var content = new StringContent(jsonStringCategory, Encoding.UTF8, "application/json");
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

        public async Task DeleteCategory(long categoryId)
        {
            var response = await _httpClient.DeleteAsync($"{requestUrl}/{categoryId}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        public async Task<List<Category>> GetAllCategories(bool includeAll = true)
        {
            var response =  await _httpClient.GetAsync($"{requestUrl}?includeAll={includeAll}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Category>>(responseContent, jsonOptions);
            }

            else return default;
        }

        public async Task<Category> GetCategory(long id, bool includeAll = true)
        {
            var response = await _httpClient.GetAsync($"{requestUrl}/{id}");


            if(response.IsSuccessStatusCode)
            {
                var reponseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Category>(reponseContent, jsonOptions);
            }
            return default;
        }
    }
}
