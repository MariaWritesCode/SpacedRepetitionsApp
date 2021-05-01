using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpacedRepApp.Share;
using SpacedRepApp.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Components
{
    public partial class CategoryMenuComponent : ComponentBase
    {
        [Inject]
        public ICategoryService CategoryService { get; set; }

        [Inject]
        public IJSRuntime _jsRuntime { get; set; }

        private bool collapse = false;

        IEnumerable<Category> allCategories;

        protected override async Task OnInitializedAsync()
        {
            allCategories = await CategoryService.GetAllCategories(false);
        }

        protected override async Task OnParametersSetAsync()
        {
            allCategories = await CategoryService.GetAllCategories(false);
        }

        public async Task DeleteCategory(long categoryId)
        {
            bool confirmed = await _jsRuntime.InvokeAsync<bool>("confirm", "Deleting a category will erase all notes assigned to it. Continue?");
            if (confirmed)
            {
                await CategoryService.DeleteCategory(categoryId);
                allCategories = await CategoryService.GetAllCategories(false);
                StateHasChanged();
            }
        }
    }
}
