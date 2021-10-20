using CronExpressionDescriptor;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Bluefish.Blazor.Components
{
    public partial class BfCronExpression
    {
        string seconds = "*";
        string minutes = "*";
        string hours = "*";
        string days = "? *";
        string months = "*";
        string years = "*";
        string description = "";

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public bool IncludeYears { get; set; } = true;

        [Parameter]
        public bool ShowDescription { get; set; } = true;

        [Parameter]
        public bool ShowExpression { get; set; } = true;

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Value))
                Value = IncludeYears ? "0 0 * ? * * *" : "0 0 * ? * *";
            else
            {
                var parts = Value.Split(' ');
                if (parts.Length > 5)
                {
                    seconds = parts[0];
                    minutes = parts[1];
                    hours = parts[2];
                    days = parts[3] + " " + DecodeDayNames(parts[5]);
                    months = DecodeMonthNames(parts[4]);
                    if (parts.Length > 6) years = parts[6];
                }
            }
            description = ExpressionDescriptor.GetDescription(Value, new Options { DayOfWeekStartIndexZero = false, Use24HourTimeFormat = true });
        }

        private async void UpdateValue()
        {
            var dayOfMonth = days.Split(' ')[0];
            var dayOfWeek = days.Split(' ')[1];
            dayOfWeek = dayOfWeek.Any(x => x == '*' || x == '-' || x == '/' || x == 'L') ? dayOfWeek : EncodeDayNames(dayOfWeek);
            var month = months.Any(x => x == '*' || x == '-' || x == '/' || x == '?' || x == 'L' || x == '#') ? months : EncodeMonthNames(months);
            Value = IncludeYears ? $"{seconds} {minutes} {hours} {dayOfMonth} {month} {dayOfWeek} {years}" : $"{seconds} {minutes} {hours} {dayOfMonth} {month} {dayOfWeek}";
            description = ExpressionDescriptor.GetDescription(Value, new Options { DayOfWeekStartIndexZero = false, Use24HourTimeFormat = true });
            await ValueChanged.InvokeAsync(Value);
        }

        private string EncodeMonthNames(string months)
        {
            return string.Join(',', months.Split(',').Select(x => x switch
            {
                "1" => "JAN",
                "2" => "FEB",
                "3" => "MAR",
                "4" => "APR",
                "5" => "MAY",
                "6" => "JUN",
                "7" => "JUL",
                "8" => "AUG",
                "9" => "SEP",
                "10" => "OCT",
                "11" => "NOV",
                "12" => "DEC",
                _ => x
            }).ToArray());
        }

        private string DecodeMonthNames(string months)
        {
            return string.Join(',', months.Split(',').Select(x => x switch
            {
                "JAN" => "1",
                "FEB" => "1",
                "MAR" => "3",
                "APR" => "4",
                "MAY" => "5",
                "JUN" => "6",
                "JUL" => "7",
                "AUG" => "8",
                "SEP" => "9",
                "OCT" => "10",
                "NOV" => "11",
                "DEC" => "12",
                _ => x
            }).ToArray());
        }

        private string EncodeDayNames(string days)
        {
            return string.Join(',', days.Split(',').Select(x => x switch
            {
                "1" => "SUN",
                "2" => "MON",
                "3" => "TUE",
                "4" => "WED",
                "5" => "THU",
                "6" => "FRI",
                "7" => "SAT",
                _ => x
            }).ToArray());
        }

        private string DecodeDayNames(string days)
        {
            return string.Join(',', days.Split(',').Select(x => x switch
            {
                "SUN" => "1",
                "MON" => "2",
                "TUE" => "3",
                "WED" => "4",
                "THU" => "5",
                "FRI" => "6",
                "SAT" => "7",
                _ => x
            }).ToArray());
        }

        private string Seconds
        {
            get { return seconds; }
            set { seconds = value; UpdateValue(); }
        }

        private string Minutes
        {
            get { return minutes; }
            set { minutes = value; UpdateValue(); }
        }

        private string Hours
        {
            get { return hours; }
            set { hours = value; UpdateValue(); }
        }

        private string Days
        {
            get { return days; }
            set { days = value; UpdateValue(); }
        }

        private string Months
        {
            get { return months; }
            set { months = value; UpdateValue(); }
        }

        private string Years
        {
            get { return years; }
            set { years = value; UpdateValue(); }
        }
    }
}
