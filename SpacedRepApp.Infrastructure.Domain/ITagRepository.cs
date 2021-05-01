using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepApp.Infrastructure.Domain
{
    public interface ITagRepository
    {
        Task<Tag> Create(Tag tag);
        Task AddAll(List<Tag> tags);
        Task<List<Tag>> GetAll();
        Task<Tag> GetByName(string tagName);
        Task Delete(long id);
    }
}
