using System.Collections.Generic;
using System.Threading.Tasks;
using SpacedRepApp.Share;

namespace SpacedRepApp.Infrastructure.Domain
{
    public interface INoteRepository
    {
        Task<IList<Note>> GetAll(bool includeAll = true);
        Task<Note> GetById(long id, bool includeAll = true);
        Task<Note> Create(Note note);
        Task<Note> Update(long id, Note updatedNote);
        Task Delete(long id);
    }
}