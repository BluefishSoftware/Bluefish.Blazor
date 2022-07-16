namespace Bluefish.Blazor.Models;

public class Filter
{
    public Filter()
    {
    }

    public Filter(string key)
    {
        Key = key;
    }

    public Filter(string key, FilterTypes op)
    {
        Key = key;
        Type = op;
    }

    public Filter(string key, FilterTypes op, IEnumerable<string> values)
    {
        Key = key;
        Type = op;
        Values.AddRange(values);
    }

    public Filter(string key, FilterTypes op, params string[] values)
    {
        Key = key;
        Type = op;
        Values.AddRange(values);
    }

    public string Key { get; set; } = string.Empty;

    public FilterTypes Type { get; set; }

    public List<string> Values { get; } = new();

    public override string ToString()
    {
        var values = Values.Select(x => x.AddQuotes()).ToArray();
        switch (Type)
        {
            case FilterTypes.Contains:
                return $"{Key}:*{Values[0]}*";

            case FilterTypes.DoesNotContain:
                return $"{Key}:!in({string.Join(',', Values)})";

            case FilterTypes.DoesNotEqual:
                return $"{Key}:!{string.Join(',', Values)}";

            case FilterTypes.EndsWith:
                return $"{Key}:*{Values[0]}";

            case FilterTypes.GreaterThan:
                return $"{Key}:>{Values[0]}";

            case FilterTypes.GreaterThanOrEqual:
                return $"{Key}:>={Values[0]}";

            case FilterTypes.In:
                return $"{Key}:in({string.Join(',', Values)})";

            case FilterTypes.IsEmpty:
                return $"{Key}:(empty)";

            case FilterTypes.IsNotEmpty:
                return $"{Key}:!(empty)";

            case FilterTypes.IsNotNull:
                return $"{Key}:!(null)";

            case FilterTypes.IsNull:
                return $"{Key}:(null)";

            case FilterTypes.LessThan:
                return $"{Key}:<{Values[0]}";

            case FilterTypes.LessThanOrEqual:
                return $"{Key}:<={Values[0]}";

            case FilterTypes.Range:
                return $"{Key}:{Values[0]}<>{Values[1]}";

            case FilterTypes.StartsWith:
                return $"{Key}:{Values[0]}*";

            default:
                return $"{Key}:{Values[0]}";
        }
    }
}
