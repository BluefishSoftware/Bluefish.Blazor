using Microsoft.AspNetCore.Components;

namespace Bluefish.Blazor
{
    public partial class BfLoadingView
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool ShowContentWhenLoading { get; set; } = true;

        [Parameter]
        public bool IsLoading { get; set; }

        public void StartLoading()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                StateHasChanged();
            }
        }

        public void StopLoading()
        {
            if (IsLoading)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
            StateHasChanged();
        }
    }
}
