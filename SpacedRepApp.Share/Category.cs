using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SpacedRepApp.Share
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }        

        public List<Note> Notes { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
