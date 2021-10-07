using Bluefish.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Bluefish.Blazor.Components
{
    public partial class BfEditBar
    {
        [Parameter]
        public EventCallback Cancel { get; set; }

        [Parameter]
        public bool CanSave { get; set; } = true;

        [CascadingParameter]
        protected EditContext EditContext { get; set; }

        [Parameter]
        public bool IsEditing { get; set; }

        [Parameter]
        public bool IsPrimary { get; set; }

        [Parameter]
        public EventCallback<bool> IsEditingChanged { get; set; }

        [Parameter]
        public Sizes Size { get; set; }

        [Parameter]
        public EventCallback Save { get; set; }

        private bool SaveEnabled()
        {
            if (EditContext != null)
            {
                return EditContext.Validate();
            }
            else
            {
                return CanSave;
            }
        }

        private async Task OnSaveAsync()
        {
            try
            {
                await Save.InvokeAsync(null).ConfigureAwait(true);
                IsEditing = false;
                await IsEditingChanged.InvokeAsync(IsEditing).ConfigureAwait(true);
            }
            catch
            {
            }
        }

        private async Task OnCancelAsync()
        {
            IsEditing = false;
            await IsEditingChanged.InvokeAsync(IsEditing).ConfigureAwait(true);
            await Cancel.InvokeAsync(null).ConfigureAwait(true);
        }

        private async Task OnEditAsync()
        {
            IsEditing = true;
            await IsEditingChanged.InvokeAsync(IsEditing).ConfigureAwait(true);
        }
    }
}
