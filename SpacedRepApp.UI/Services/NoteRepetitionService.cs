using Microsoft.Extensions.Configuration;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Services
{
    public class NoteRepetitionService : INoteRepetitionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly string _requestUrl;


        public NoteRepetitionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _requestUrl = _configuration["Settings:NoteAPIEndpoint"];
        }

        public async Task ReviseNote(long id, Note note)
        {
            note.NextRepetition = SetNextRepetetionDate(note);
            var jsonStringNote = JsonSerializer.Serialize(note, jsonOptions);

            using (HttpContent httpContent = new StringContent(jsonStringNote, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await _httpClient.PutAsync($"{_requestUrl}/{id}", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
        }

        public async Task<List<Note>> GetNotesToRevise()
        {
            var response = await _httpClient.GetAsync(_requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                var listOfNotes = JsonSerializer.Deserialize<List<Note>>(stringContent, jsonOptions);
                return listOfNotes.Where(x => x.NextRepetition.Date == DateTime.Today.Date).ToList();
            }

            return default;
        }

        private DateTime SetNextRepetetionDate(Note note)
        {
            switch (note.RepetitionCount)
            {
                case 0:
                    note.RepetitionCount++;
                    return note.DateCreated.AddDays(1);
                case 1:
                    note.RepetitionCount++;
                    return note.NextRepetition.AddDays(7);
                case 2:
                    note.RepetitionCount++;
                    return note.NextRepetition.AddDays(16);
                case 3:
                    note.RepetitionCount++;
                    return note.NextRepetition.AddDays(35);
                default:
                    return DateTime.MinValue;
            }
        }
    }
}
