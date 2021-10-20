using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Bluefish.Blazor.Components
{
    public partial class BfDropzone : IDisposable
    {
        private DotNetObjectReference<BfDropzone> _jsThisRef;
        private IJSObjectReference _jsModule;
        private bool _initialized;

        [Parameter]
        public string AcceptedFiles { get; set; }

        [Parameter] public EventCallback AllUploadsComplete { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public int MaxFilesize { get; set; } = 50;

        [Parameter]
        public int Timeout { get; set; } = 30000;

        [Parameter]
        public string Url { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _jsThisRef = DotNetObjectReference.Create<BfDropzone>(this);
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/js/dropzone.js").ConfigureAwait(true);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_initialized && JSRuntime != null && _jsModule != null)
            {
                _initialized = true;
                var options = new
                {
                    Url,
                    Timeout,
                    MaxFilesize,
                    AcceptedFiles,
                    PreviewItemTemplate = "#my-dropzone-template"
                };
                await _jsModule.InvokeVoidAsync("initDropzone", "#my-dropzone", options, _jsThisRef).ConfigureAwait(true);
            }
        }

        private async void OnClear()
        {
            await _jsModule.InvokeVoidAsync("clearDropzone", "#my-dropzone").ConfigureAwait(true);
        }

        [JSInvokable("Bluefish.Blazor.BfDropzone.OnAllUploadsComplete")]
        public async Task OnAllUploadsComplete()
        {
            await AllUploadsComplete.InvokeAsync(null).ConfigureAwait(true);
        }

        private async Task OnClick()
        {
            await _jsModule.InvokeVoidAsync("dropzoneClick", "#my-dropzone").ConfigureAwait(true);
        }

        public void Dispose()
        {
            //_jsModule.InvokeVoidAsync("RemoveTextEditor", Id);
            _jsThisRef?.Dispose();
        }
    }
}
