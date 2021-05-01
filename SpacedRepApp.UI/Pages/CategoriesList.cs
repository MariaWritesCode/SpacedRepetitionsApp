using Microsoft.AspNetCore.Components;
using SpacedRepApp.Share;
using SpacedRepApp.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Pages
{
    public partial class CategoriesList : ComponentBase
    {
        [Parameter]
        public long id { get; set; }

        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        
        private Category currentCategory;
        private List<Note> notes;
        private string newNoteLink;

        protected override async Task OnParametersSetAsync()
        {
            newNoteLink= $"/newNote/{id}";
            currentCategory = await CategoryService.GetCategory(id);
        }       
    }
}
