using Microsoft.AspNetCore.Components;

namespace Bluefish.Blazor.Components
{
    public partial class BfPanel
    {
        [Parameter]
        public RenderFragment PanelBody { get; set; }

        [Parameter]
        public bool ShowBorder { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment PanelToolbar { get; set; }
    }
}
