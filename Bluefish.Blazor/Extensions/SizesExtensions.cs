using Bluefish.Blazor.Models;

namespace Bluefish.Blazor.Extensions
{
    public static class SizesExtensions
    {
        public static string CssClass(this Sizes size, string small, string medium, string large) => size switch
        {
            Sizes.Small => small,
            Sizes.Medium => medium,
            Sizes.Large => large,
            _ => large
        };
    }
}
