using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpacedRepApp.Share
{
    public class Note
    {
        public long Id { get; set; }        
        public DateTime DateCreated { get; set; }
        
        public string Contents { get; set; }

        public DateTime NextRepetition { get; set; }
        public bool Revised { get; set; }

        public List<Tag> Tags { get; set; }
        public long CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
