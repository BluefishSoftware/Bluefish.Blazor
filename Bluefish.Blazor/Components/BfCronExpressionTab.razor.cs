namespace Bluefish.Blazor.Components;

public partial class BfCronExpressionTab
{
    [Parameter]
    public string ItemName { get; set; }

    [Parameter]
    public IEnumerable<Tuple<string, int>> Items { get; set; }

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public string DropdownWidth { get; set; } = "80px";

    [Parameter]
    public Sizes Size { get; set; }

    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public bool IsDay { get; set; }

    Tuple<string, int>[] occurrence;
    Tuple<string, int>[] occurrence2;
    Tuple<string, int>[] dayOfMonth;
    Tuple<string, int>[] days;
    //Dictionary<string, object> dropdownAttributes;
    State state = new State();

    protected override void OnInitialized()
    {
        occurrence = Enumerable.Range(1, Items.Count()).Select(x => new Tuple<string, int>(x.ToString(), x)).ToArray();
        occurrence2 = Enumerable.Range(1, Items.Count()).Select(x => new Tuple<string, int>(x.ToString(), x)).ToArray();
        dayOfMonth = Enumerable.Range(1, 31).Select(x => new Tuple<string, int>(DayPostfix(x), x)).ToArray();
        days = Enumerable.Range(1, 31).Select(x => new Tuple<string, int>(x.ToString(), x)).ToArray();
        //dropdownAttributes = new Dictionary<string, object>()
        //{
        //    { "width", DropdownWidth }
        //};

        // defaults
        state.Occurence = state.Occurence2 = 1;
        state.Range1 = Items.First().Item2;
        state.Range2a = Items.First().Item2;
        state.Range2b = Items.Last().Item2;
        state.Range3 = Items.First().Item2;
        state.LastDayOfMonth = Items.First().Item2;
        state.DaysBeforeEnd = state.NearestWeekday = state.DayOfMonth1 = state.DayOfMonth2 = 1;

        if (IsDay)
        {
            var parts = Value.Split(' ');
            if (parts[0] == "?" && parts[1] == "*")
            {
                state.Method = "All";
            }
            else if (parts[0] == "L" && parts[1] == "?")
            {
                state.Method = "LastDay";
            }
            else if (parts[0] == "LW" && parts[1] == "?")
            {
                state.Method = "LastWeekDay";
            }
            else if (parts[0] == "?" && parts[1].EndsWith("L"))
            {
                state.Method = "LastDayOfMonth";
                state.LastDayOfMonth = Convert.ToInt32(parts[1].Substring(0, parts[1].IndexOf("L")));
            }
            else if (parts[0].StartsWith("L-") && parts[1] == "?")
            {
                state.Method = "BeforeEnd";
                state.DaysBeforeEnd = Convert.ToInt32(parts[0].Substring(2));
            }
            else if (parts[0].EndsWith("W") && parts[1] == "?")
            {
                state.Method = "NearestWeekday";
                state.NearestWeekday = Convert.ToInt32(parts[0].Substring(0, parts[0].IndexOf("W")));
            }
            else if (parts[0] == "?" && parts[1].Contains("#"))
            {
                state.Method = "DayOfMonth";
                state.DayOfMonth2 = Convert.ToInt32(parts[1].Substring(0, parts[1].IndexOf("#")));
                state.DayOfMonth1 = Convert.ToInt32(parts[1].Substring(parts[1].IndexOf("#") + 1));
            }
            else if (parts[0] == "?" && parts[1].Contains("/"))
            {
                state.Method = "Range1";
                var segs = Split(parts[1], '/');
                state.Range1 = segs[0];
                state.Occurence = segs[1];
            }
            else if (parts[0].Contains("/") && parts[1] == "?")
            {
                state.Method = "Range3";
                var segs = Split(parts[0], '/');
                state.Range3 = segs[0];
                state.Occurence2 = segs[1];
            }
            else if (parts[0] == "?")
            {
                state.Method = "Individual";
                state.Individual.AddRange(Split(parts[1], ','));
            }
            else if (parts[1] == "?")
            {
                state.Method = "Individual2";
                // Split can throw error if invalid expression provided
                state.Individual2.AddRange(Split(parts[0], ','));
            }
        }
        else
        {
            if (Value.Contains("/"))
            {
                state.Method = "Range1";
                var parts = Split(Value, '/');
                state.Range1 = parts[0];
                state.Occurence = parts[1];
            }
            else if (Value.Contains("-"))
            {
                state.Method = "Range2";
                var parts = Split(Value, '-');
                state.Range2a = parts[0];
                state.Range2b = parts[1];
            }
            else if (Value == "*")
            {
                state.Method = "All";
            }
            else
            {
                state.Method = "Individual";
                state.Individual.AddRange(Split(Value, ','));
            }
        }
    }

    int[] Split(string values, char separator = ',')
    {
        return values.Split(separator).Select(x => Convert.ToInt32(x)).ToArray();
    }

    string DayPostfix(int day)
    {
        switch (day)
        {
            case 1:
            case 21:
            case 31:
                return $"{day}st";

            case 2:
            case 22:
                return $"{day}nd";

            case 3:
            case 23:
                return $"{day}rd";

            default:
                return $"{day}th";
        }
    }

    void Toggle(Tuple<string, int> item, List<int> list)
    {
        if (list.Contains(item.Item2))
            list.Remove(item.Item2);
        else
            list.Add(item.Item2);
        Refresh();
    }

    async void Refresh()
    {
        if (IsDay)
        {
            switch (state.Method)
            {
                case "All":
                    Value = "? *";
                    break;
                case "Range1":
                    Value = $"? {state.Range1}/{state.Occurence}";
                    break;
                case "Individual":
                    if (state.Individual.Count == 0)
                        Value = "? 1";
                    else
                    {
                        state.Individual.Sort();
                        Value = "? " + string.Join(',', state.Individual);
                    }
                    break;
                case "Range3":
                    Value = $"{state.Range3}/{state.Occurence2}" + " ?";
                    break;
                case "Individual2":
                    if (state.Individual2.Count == 0)
                        Value = "1 ?";
                    else
                    {
                        state.Individual2.Sort();
                        Value = string.Join(',', state.Individual2) + " ?";
                    }
                    break;
                case "LastDay":
                    Value = "L ?";
                    break;
                case "LastWeekDay":
                    Value = "LW ?";
                    break;
                case "LastDayOfMonth":
                    Value = $"? {state.LastDayOfMonth}L";
                    break;
                case "BeforeEnd":
                    Value = $"L-{state.DaysBeforeEnd} ?";
                    break;
                case "NearestWeekday":
                    Value = $"{state.NearestWeekday}W ?";
                    break;
                case "DayOfMonth":
                    Value = $"? {state.DayOfMonth2}#{state.DayOfMonth1}";
                    break;
            }
        }
        else
        {
            switch (state.Method)
            {
                case "All":
                    Value = "*";
                    break;
                case "Range1":
                    Value = $"{state.Range1}/{state.Occurence}";
                    break;
                case "Individual":
                    if (state.Individual.Count == 0)
                        Value = Items.First().Item2.ToString();
                    else
                    {
                        state.Individual.Sort();
                        Value = string.Join(',', state.Individual);
                    }
                    break;
                case "Range2":
                    Value = $"{state.Range2a}-{state.Range2b}";
                    break;
            }
        }
        await ValueChanged.InvokeAsync(Value);
    }

    class State
    {
        public string Method { get; set; }
        public int Occurence { get; set; }
        public int Range1 { get; set; }
        public int Range2a { get; set; }
        public int Range2b { get; set; }
        public List<int> Individual { get; set; } = new List<int>();
        // day specific
        public int Occurence2 { get; set; }
        public int Range3 { get; set; }
        public int LastDayOfMonth { get; set; }
        public int DaysBeforeEnd { get; set; }
        public int NearestWeekday { get; set; }
        public int DayOfMonth1 { get; set; }
        public int DayOfMonth2 { get; set; }
        public List<int> Individual2 { get; set; } = new List<int>();
    }
}
