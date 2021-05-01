using Microsoft.AspNetCore.Components;
using SpacedRepApp.Share;
using SpacedRepApp.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Components
{
    public partial class CategoryFormComponent : ComponentBase
    {
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Category NewCategory { get; set; }  = new Category();

        private async Task SubmitCategory()
        {
            await CategoryService.AddCategory(NewCategory);
            NavManager.NavigateTo($"/");
        }
    }
}
