using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace Bluefish.Blazor
{
    public partial class BfButton
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> Click { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public string IconCssClass { get; set; }

        [Parameter]
        public bool IsPrimary { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string TextCssClass { get; set; }

        private Dictionary<string, object> GetAttributes()
        {
            var attr = new Dictionary<string, object>(AdditionalAttributes ?? new Dictionary<string, object>());
            if (!attr.ContainsKey("class"))
            {
                attr.Add("class", $"btn btn-sm {(IsPrimary ? "btn-primary" : "btn-secondary")}");
            }
            if (!Enabled)
            {
                attr.Add("disabled", true);
            }
            return attr;
        }
    }
}
