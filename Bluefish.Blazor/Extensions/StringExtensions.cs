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

    public static Boolean ToBoolean(this string value, bool defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }
        if (Boolean.TryParse(value, out bool temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static DateTime ToDateTime(this string value, DateTime defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }
        if (DateTime.TryParse(value, out DateTime temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static DateTimeOffset ToDateTimeOffset(this string value, DateTimeOffset defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }
        if (DateTimeOffset.TryParse(value, out DateTimeOffset temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Decimal ToDecimal(this string value, decimal defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }
        if (Decimal.TryParse(value, out decimal temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Int32 ToInt32(this string value, int defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }
        if (Int32.TryParse(value, out int temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Int64 ToInt64(this string value, long defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }
        if (Int64.TryParse(value, out long temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Boolean? ToNullableBoolean(this string value, bool? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        if (Boolean.TryParse(value, out bool temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static DateTime? ToNullableDateTime(this string value, DateTime? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        if (DateTime.TryParse(value, out DateTime temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static DateTimeOffset? ToNullableDateTimeOffset(this string value, DateTimeOffset? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        if (DateTimeOffset.TryParse(value, out DateTimeOffset temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Decimal? ToNullableDecimal(this string value, decimal? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        if (Decimal.TryParse(value, out decimal temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Int32? ToNullableInt32(this string value, int? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        if (Int32.TryParse(value, out int temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static Int64? ToNullableInt64(this string value, long? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        if (Int64.TryParse(value, out long temp))
        {
            return temp;
        }
        return defaultValue;
    }

    public static string LowerFirstChar(this string text) => string.IsNullOrWhiteSpace(text)
    ? text
    : text[0].ToString().ToLower() + text[1..];

    public static string UpperFirstChar(this string text) => string.IsNullOrWhiteSpace(text)
        ? text
        : text[0].ToString().ToUpper() + text[1..];

}
