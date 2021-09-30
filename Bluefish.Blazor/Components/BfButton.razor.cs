using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace Bluefish.Blazor.Components
{
    public partial class BfButton
    {
        [Parameter]
        public EventCallback<MouseEventArgs> Click { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string IconCssClass { get; set; }

        [Parameter]
        public bool IsPrimary { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string TextCssClass { get; set; }

        protected override Dictionary<string, object> RootAttributes
        {
            get
            {
                var attr = base.RootAttributes;
                if (!attr.ContainsKey("class"))
                {
                    attr.Add("class", $"bf-button btn btn-sm {(IsPrimary ? "btn-primary" : "")}");
                }
                return attr;
            }
        }
    }
}
