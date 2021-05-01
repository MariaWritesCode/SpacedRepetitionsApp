using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Services
{
    public interface INoteService
    {
        Task EditNote(long id, Note note);
        Task<Note> GetNote(long id, bool includeAll = true);
        Task<List<Note>> GetAllNotesForCategory(long categoryId);
        Task AddNote(Note note);
        Task Delete(long noteId);
    }
}
