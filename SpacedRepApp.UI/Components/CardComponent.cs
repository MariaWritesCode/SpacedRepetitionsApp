using Blazorise;
using Microsoft.AspNetCore.Components;
using SpacedRepApp.Share;
using SpacedRepApp.UI.Pages;
using SpacedRepApp.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Components
{
    public partial class CardComponent : ComponentBase
    {
        [Inject]
        public ICategoryService _categoryService { get; set; }
        [Inject]
        public INoteService _noteService { get; set; }

        [Parameter]
        public List<Note> NotesToDisplay { get; set; }       

        public async Task DeleteNote(Note note)
        {
            await _noteService.Delete(note.Id);
            NotesToDisplay = await _noteService.GetAllNotesForCategory(note.CategoryId);
            StateHasChanged();
        }

        public async Task ReviseNote(Note note)
        {
            await _noteService.ReviseNote(note.Id, note);
            NotesToDisplay = await _noteService.GetAllNotesForCategory(note.CategoryId);
            StateHasChanged();
        }
    }
}
