namespace Bluefish.Blazor.Extensions;

public static class FilterTypesExtensions
{
    public static string GetOperator(this FilterTypes op) => op switch
    {
        FilterTypes.Equals => "==",
        FilterTypes.DoesNotEqual => "!=",
        FilterTypes.LessThan => "<=",
        FilterTypes.LessThanOrEqual => "<=",
        FilterTypes.GreaterThan => ">",
        FilterTypes.GreaterThanOrEqual => ">=",
        FilterTypes.IsNull => "==",
        FilterTypes.IsNotNull => "!=",
        _ => ""
    };

    public static string GetDisplayName(this FilterTypes op) => op switch
    {
        FilterTypes.Contains => "Contains",
        FilterTypes.DoesNotContain => "Does not contain",
        FilterTypes.DoesNotEqual => "Does not equal",
        FilterTypes.EndsWith => "Ends with",
        FilterTypes.Equals => "Equals",
        FilterTypes.GreaterThan => "Greater than",
        FilterTypes.GreaterThanOrEqual => "Greater than or equal",
        FilterTypes.In => "In",
        FilterTypes.IsEmpty => "Is empty",
        FilterTypes.IsNotEmpty => "Is not empty",
        FilterTypes.IsNotNull => "Is not null",
        FilterTypes.IsNull => "Is null",
        FilterTypes.LessThan => "Less than",
        FilterTypes.LessThanOrEqual => "Less than or equal",
        FilterTypes.Range => "In range",
        FilterTypes.StartsWith => "Starts with",
        _ => "Unknown"
    };

}
