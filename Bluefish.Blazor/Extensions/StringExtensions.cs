using CronExpressionDescriptor;

namespace Bluefish.Blazor.Extensions;

public static class StringExtensions
{
    public static string GetCronDescription(this string expression, bool dayOfWeekStartIndexZero = false, bool use24HourTimeFormat = true)
    {
        return ExpressionDescriptor.GetDescription(
            expression,
            new Options
            {
                DayOfWeekStartIndexZero = dayOfWeekStartIndexZero,
                Use24HourTimeFormat = use24HourTimeFormat
            });
    }
}
