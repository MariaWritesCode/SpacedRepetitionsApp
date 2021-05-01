using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAvailableTags();
        Task CreateTag(Tag newTag);
        Task DeleteTag(long tagId);
    }
}
