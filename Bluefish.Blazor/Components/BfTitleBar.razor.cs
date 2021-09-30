using Microsoft.AspNetCore.Components;

namespace Bluefish.Blazor.Components
{
    public partial class BfTitleBar
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string IconCssClass { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string TitleCssClass { get; set; }
    }
}
