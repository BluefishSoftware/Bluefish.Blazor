using CronExpressionDescriptor;

namespace Bluefish.Blazor.Extensions;

public static class StringExtensions
{
    public static string AddQuotes(this string text)
    {
        if (!string.IsNullOrWhiteSpace(text) && text.Contains(' '))
        {
            return $"\"{text}\"";
        }
        return text;
    }

    public static string GetCronDescription(this string expression, bool dayOfWeekStartIndexZero = false, bool use24HourTimeFormat = true)
    {
        return ExpressionDescriptor.GetDescription(
            expression,
            new CronExpressionDescriptor.Options
            {
                DayOfWeekStartIndexZero = dayOfWeekStartIndexZero,
                Use24HourTimeFormat = use24HourTimeFormat
            });
    }

    /// <summary>
    /// Parses the given text into separate fields. Field values can be delimited by double quotes
    /// so as to include the separator character.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <param name="separator">The field separator character.</param>
    /// <returns>An enumerable containing all fields.</returns>
    public static IEnumerable<string> ParseAsCsv(this string text, char separator = ',')
    {
        if (string.IsNullOrWhiteSpace(text))
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
                if (ch == separator && !quoted && !hashed)
                {
                    // not within quotes or hashes so end of next token
                    yield return sb.ToString();
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
            yield return sb.ToString();
        }
    }

    public static string RemoveQuotes(this string text)
    {
        if (text.StartsWith("\"") && text.EndsWith("\""))
        {
            return text[1..^1];
        }
        return text;
    }
}
