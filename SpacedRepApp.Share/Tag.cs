using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpacedRepApp.Share
{
    public class Tag
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<Note> Notes { get; set; }
    }
}
