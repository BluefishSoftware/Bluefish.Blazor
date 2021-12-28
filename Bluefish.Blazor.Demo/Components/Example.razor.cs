namespace Bluefish.Blazor.Demo.Components;

public partial class Example
{
    [Parameter]
    public RenderFragment Code { get; set; }

    [Parameter]
    public RenderFragment Demo { get; set; }

    [Parameter]
    public RenderFragment Description { get; set; }

    [Parameter]
    public string Title { get; set; }
}
