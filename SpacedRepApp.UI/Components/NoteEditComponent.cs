using Microsoft.AspNetCore.Components;
using SpacedRepApp.Share;
using SpacedRepApp.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Components
{
    public partial class NoteEditComponent : ComponentBase
    {
        [Parameter]
        public long NoteId { get; set; }

        [Parameter]
        public long CategoryIdForNewNote { get; set; }

        [Inject]
        public ICategoryService FormCategoryService { get; set; }
        [Inject]
        public INoteService FormNoteService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public ITagService TagFormService { get; set; }

        public Note NoteToEdit { get; set; } = new Note();
        public List<Category> AvailableCategories { get; set; } = new List<Category>();
        public List<Tag> AvailableTags { get; set; } = new List<Tag>();

        private string selectedTagNames { get; set; }
        private List<string> listOfTagNames = new();
        private bool newNote = false;

        protected override async Task OnInitializedAsync()
        {
            if (NoteId < 0)
            {
                NoteToEdit.CategoryId = CategoryIdForNewNote;
                NoteToEdit.Tags=new List<Tag>();
                newNote = true;
            }
            else
            {
                NoteToEdit = await FormNoteService.GetNote(NoteId, true);                
            }

            AvailableCategories = await FormCategoryService.GetAllCategories(false);
            AvailableTags = await TagFormService.GetAvailableTags();

            if(NoteToEdit.Tags != null)
            {
                selectedTagNames = String.Join(' ', NoteToEdit.Tags.Select(x => x.Name));
            }            
        }

        private async Task SubmitHandler()
        {
            NoteToEdit.Category = AvailableCategories.FirstOrDefault(x => x.Id == NoteToEdit.CategoryId);
            HandleTagSelection(listOfTagNames);
            if (newNote)
            {
                await FormNoteService.AddNote(NoteToEdit);
            }
            else
            {
                await FormNoteService.EditNote(NoteToEdit.Id, NoteToEdit);
            }
            
            NavManager.NavigateTo($"/category/{NoteToEdit.CategoryId}");
        }

        private void HandleTagSelection(List<string> tagSelection)
        {
            foreach (var tag in tagSelection)
            {
                if (NoteToEdit.Tags.FirstOrDefault(x=> x.Name == tag) != null)
                {
                    continue;
                }
                else
                {
                    NoteToEdit.Tags.Add(new Tag { Name = tag });                    
                }
            }
        }

        private async Task PostTags(IEnumerable<Tag> tagsToPost)
        {
            foreach (var tag in NoteToEdit.Tags)
            {
                await TagFormService.CreateTag(tag);
            }                
        }
    }        
    }

