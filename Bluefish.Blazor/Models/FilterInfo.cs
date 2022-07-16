namespace Bluefish.Blazor.Models;

public class FilterInfo
{
    private string _searchText = string.Empty;

    public void Clear()
    {
        Filters.Clear();
        SearchText = string.Empty;
        FilterChanged?.Invoke(this, new EventArgs());
    }

    public event EventHandler FilterChanged;

    public string SearchText
    {
        get
        {
            return _searchText;
        }
        set
        {
            if (value != _searchText)
            {
                Update(value);
                _searchText = value;
            }
        }
    }

    public List<Filter> Filters { get; private set; } = new();

    public FilterInfo()
    {
    }

    public FilterInfo(string searchText)
    {
        Update(searchText);
    }

    public void AddFilter(Filter filter)
    {
        Filters.Add(filter);
        Update();
        FilterChanged?.Invoke(this, new EventArgs());
    }

    public void RemoveFilter(Filter filter)
    {
        Filters.Remove(filter);
        Update();
        FilterChanged?.Invoke(this, new EventArgs());
    }

    private void Update()
    {
        var sb = new StringBuilder();
        foreach (var filter in Filters)
        {
            if (sb.Length > 0)
            {
                sb.Append(' ');
            }
            sb.Append(filter.ToString());
        }
        _searchText = sb.ToString();
    }

    public void Update(string text)
    {
        Filters.Clear();

        // attempt to parse individual field filters
        var filters = ParseMany(text).Where(x => x != null).ToArray();
        if (filters.Length > 0)
        {
            Filters.AddRange(filters);
        }
        _searchText = text;
    }

    private static IEnumerable<Filter> ParseMany(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || !text.Contains(':'))
        {
            yield break;
        }

        bool token = false;
        bool quoted = false;
        bool hashed = false;
        var sb = new StringBuilder();

        // read next token
        foreach (var ch in text)
        {
            if (token)
            {
                if (char.IsWhiteSpace(ch) && !quoted && !hashed)
                {
                    // not within quotes or hashes so end of next token
                    yield return ParseSingle(sb.ToString());
                    sb.Clear();
                    token = false;
                }
                else
                {
                    if (ch == '"')
                    {
                        quoted = !quoted;
                    }
                    else if (ch == '#' && !quoted)
                    {
                        hashed = !hashed;
                    }
                    sb.Append(ch);
                }
            }
            else
            {
                // consume leading whitespace
                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }
                token = true;
                sb.Append(ch);
            }
        }

        // possible end of string while still quoted
        if (token)
        {
            if (quoted)
            {
                sb.Append('"');
            }
            else if (hashed)
            {
                sb.Append('#');
            }
            yield return ParseSingle(sb.ToString());
        }
    }

    private static Filter ParseSingle(string token)
    {
        var colonIdx = token.IndexOf(':');
        if (colonIdx == -1)
        {
            return null;
        }
        var key = token[..colonIdx];
        var encodedValue = token[(colonIdx + 1)..];

        if (encodedValue == "!(empty)")
        {
            return new Filter(key, FilterTypes.IsNotEmpty);
        }

        if (encodedValue == "(empty)")
        {
            return new Filter(key, FilterTypes.IsEmpty);
        }

        if (encodedValue == "!(null)")
        {
            return new Filter(key, FilterTypes.IsNotNull);
        }

        if (encodedValue == "(null)")
        {
            return new Filter(key, FilterTypes.IsNull);
        }

        if (encodedValue.StartsWith("in(", StringComparison.OrdinalIgnoreCase) && encodedValue.EndsWith(")") && encodedValue.Length > 3)
        {
            return new Filter(key, FilterTypes.In, encodedValue[3..^1].ParseAsCsv());
        }

        if (encodedValue.StartsWith("!*") && encodedValue.EndsWith("*") && encodedValue.Length > 2)
        {
            return new Filter(key, FilterTypes.DoesNotContain, encodedValue[2..^1]);
        }

        if (encodedValue.StartsWith("*") && encodedValue.EndsWith("*") && encodedValue.Length > 1)
        {
            return new Filter(key, FilterTypes.Contains, encodedValue[1..^1]);
        }

        if (encodedValue.Contains("<>") && encodedValue.Length > 2)
        {
            var idx = encodedValue.IndexOf("<>");
            return new Filter(key, FilterTypes.Range, encodedValue[..idx], encodedValue[(idx + 2)..]);
        }

        if (encodedValue.EndsWith("*"))
        {
            return new Filter(key, FilterTypes.StartsWith, encodedValue[..^1]);
        }

        if (encodedValue.StartsWith("*"))
        {
            return new Filter(key, FilterTypes.EndsWith, encodedValue[1..]);
        }

        if (encodedValue.StartsWith("!"))
        {
            return new Filter(key, FilterTypes.DoesNotEqual, encodedValue[1..]);
        }

        if (encodedValue.StartsWith(">="))
        {
            return new Filter(key, FilterTypes.GreaterThanOrEqual, encodedValue[2..]);
        }

        if (encodedValue.StartsWith("<="))
        {
            return new Filter(key, FilterTypes.LessThanOrEqual, encodedValue[2..]);
        }

        if (encodedValue.StartsWith(">"))
        {
            return new Filter(key, FilterTypes.GreaterThan, encodedValue[1..]);
        }

        if (encodedValue.StartsWith("<"))
        {
            return new Filter(key, FilterTypes.LessThan, encodedValue[1..]);
        }

        return new Filter(key, FilterTypes.Equals, encodedValue);
    }
}
