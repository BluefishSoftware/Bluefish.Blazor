using Microsoft.AspNetCore.Components;
using System;

namespace Bluefish.Blazor.Components
{
    public partial class BfTabPage
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }


        [CascadingParameter]
        private BfTabPanel Parent { get; set; }

        [Parameter]
        public string Text { get; set; }

        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent), "BfTabPage must exist within a BfTabPanel");

            Parent.AddPage(this);

            base.OnInitialized();
        }
    }
}
