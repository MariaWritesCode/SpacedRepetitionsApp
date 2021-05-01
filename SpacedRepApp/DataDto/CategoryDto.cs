using SpacedRepApp.Share;
using System.Collections.Generic;

namespace SpacedRepApp.Controllers
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Note> Notes { get; set; }
    }
}