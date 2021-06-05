using SpacedRepApp.Share;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Services
{
    public interface INoteRepetitionService
    {
        Task<List<Note>> GetNotesToRevise();
        Task ReviseNote(long id, Note note);
    }
}