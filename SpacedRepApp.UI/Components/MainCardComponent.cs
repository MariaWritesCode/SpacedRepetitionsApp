using Microsoft.AspNetCore.Components;
using SpacedRepApp.Share;
using SpacedRepApp.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Components
{
    public partial class MainCardComponent : ComponentBase
    {
        private List<Note> NotesToRevise;

        [Inject]
        public INoteRepetitionService _noteRepetitiionService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NotesToRevise = await _noteRepetitiionService.GetNotesToRevise();
        }
    }
}
