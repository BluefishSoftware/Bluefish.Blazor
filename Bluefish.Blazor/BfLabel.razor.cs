using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Bluefish.Blazor
{
    public partial class BfLabel
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public string IconCssClass { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string TextCssClass { get; set; }
    }
}
