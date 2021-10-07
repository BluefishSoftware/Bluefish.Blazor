using Microsoft.AspNetCore.Components;

namespace Bluefish.Blazor.Components
{
    public partial class BfToolBar
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
