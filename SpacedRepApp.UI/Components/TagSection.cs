using Microsoft.AspNetCore.Components;
using SpacedRepApp.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Components
{
    public partial class TagSection : ComponentBase
    {
        [Parameter]
        public List<string> DisplayTags { get; set; }
        [Inject]
        public ITagService _tagService { get; set; }

        protected override void OnParametersSet()
        {
            StateHasChanged();
        }

        public async Task DeleteTag(long tagId)
        {
            await _tagService.DeleteTag(tagId);
            StateHasChanged();
        }
    }
}
