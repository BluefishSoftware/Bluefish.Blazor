namespace Bluefish.Blazor.Models;

public class DateRange
{
    /// <summary>
    /// Gets or sets the ranges start date.
    /// </summary>
    public DateTime DateFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets the ranges end date.
    /// </summary>
    public DateTime DateTo { get; set; } = DateTime.Today.AddDays(1);

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if (obj != null && obj is DateRange range)
        {
            return DateFrom == range.DateFrom && DateTo == range.DateTo;
        }
        return false;
    }

    public override string ToString()
    {
        if (Equals(Today))
        {
            return "Today";
        }
        if (Equals(Yesterday))
        {
            return "Yesterday";
        }
        if (Equals(ThisMonth))
        {
            return "This Month";
        }
        if (Equals(LastMonth))
        {
            return "Last Month";
        }
        return $"{DateFrom:d} - {DateTo:d}";
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Gets a DateRange instance that covers today.
    /// </summary>
    public static DateRange Today
    {
        get
        {
            return new DateRange { DateFrom = DateTime.Today, DateTo = DateTime.Today.AddDays(1) };
        }
    }

    /// <summary>
    /// Gets a DateRange instance that covers yesterday.
    /// </summary>
    public static DateRange Yesterday
    {
        get
        {
            return new DateRange { DateFrom = DateTime.Today.AddDays(-1), DateTo = DateTime.Today };
        }
    }

    /// <summary>
    /// Gets a DateRange instance that covers yesterday.
    /// </summary>
    public static DateRange ThisMonth
    {
        get
        {
            return new DateRange { DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTo = DateTime.Today.AddDays(1) };
        }
    }

    /// <summary>
    /// Gets a DateRange instance that covers yesterday.
    /// </summary>
    public static DateRange LastMonth
    {
        get
        {
            return new DateRange { DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1), DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) };
        }
    }
}
