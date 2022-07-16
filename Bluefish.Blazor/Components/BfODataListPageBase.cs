using Bluefish.Blazor.Utility;
using Simple.OData.Client;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Bluefish.Blazor.Components
{
    public abstract class BfODataListPageBase<TItem, TKey> : ComponentBase where TItem : class
    {
        protected PageInfo _pageInfo = new();
        protected SortInfo _sortInfo = new();
        protected FilterInfo _filterInfo = new();
        protected BfTable<TItem, TKey> _table = null!;
        protected BfTableToolbar<TItem, TKey> _tableToolbar = null!;
        protected TKey? _selectedId;

        [Inject]
        public INotificationService NotificationService { get; set; } = null!;

        protected abstract IBoundClient<TItem> GetBaseQuery();

        protected virtual SortInfo? DefaultSort { get; set; }

        protected virtual IBoundClient<TItem> ApplyBasicFilter(IBoundClient<TItem> query, string searchText)
        {
            return query;
        }

        protected virtual IBoundClient<TItem> ApplyFilter(IBoundClient<TItem> query, Filter filter)
        {
            Expression<Func<TItem, bool>>? newPredicate = null;

            var col = _table.AllColumns.FirstOrDefault(x => x.FilterKey == filter.Key);
            if (col?.DataMember != null)
            {
                var property = GetPropertyName(filter, col);
                var propertyType = GetPropertyType(filter, col);
                var op = filter.Type.GetOperator();
                var v1 = filter.Values.FirstOrDefault() ?? String.Empty;

                if (propertyType is null)
                {
                    return query;
                }
                else if (propertyType.IsNullable() && (filter.Type == FilterTypes.IsNull || filter.Type == FilterTypes.IsNotNull))
                {
                    newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} {op} null");
                }
                else if (propertyType.IsBool())
                {
                    newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} {op} {v1}");
                }
                else if (propertyType.IsEnum)
                {
                    if (filter.Type == FilterTypes.DoesNotEqual)
                    {
                        newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} != @0", v1);
                    }
                    else if (filter.Type == FilterTypes.In)
                    {
                        var q = string.Join(" || ", filter.Values.Select((x, i) => $"it.{property} == @{i}").ToArray());
                        var vs = filter.Values.ToArray();
                        newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, q, vs);
                    }
                    else
                    {
                        newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} == @0", v1);
                    }
                }
                else if (propertyType.IsText())
                {
                    v1 = filter.Values.FirstOrDefault()?.RemoveQuotes().ToLower() ?? String.Empty;
                    switch (filter.Type)
                    {
                        case FilterTypes.Contains:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property}.ToLower().Contains(@0)", v1);
                            break;

                        case FilterTypes.DoesNotContain:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"!{property}.ToLower().Contains(@0)", v1);
                            break;

                        case FilterTypes.DoesNotEqual:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property}.ToLower() != @0", v1);
                            break;

                        case FilterTypes.Equals:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property}.ToLower() == @0", v1);
                            break;

                        case FilterTypes.EndsWith:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property}.ToLower().EndsWith(@0)", v1);
                            break;

                        case FilterTypes.In:
                            var q = string.Join(" || ", filter.Values.Select((x, i) => $"it.{property}.ToLower() == @{i}").ToArray());
                            var vs = filter.Values.Select(x => x?.RemoveQuotes()?.ToLower() ?? "").ToArray();
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, q, vs);
                            break;

                        case FilterTypes.IsEmpty:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} == @0", String.Empty);
                            break;

                        case FilterTypes.IsNotEmpty:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} != @0", String.Empty);
                            break;

                        case FilterTypes.Range:
                            var v2 = filter.Values[1]?.ToString()?.RemoveQuotes().ToLower() ?? String.Empty;
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} >= @0 && {property} <= @1", v1, v2);
                            break;

                        case FilterTypes.StartsWith:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property}.ToLower().StartsWith(@0)", v1);
                            break;
                    }
                }
                else if (propertyType.IsDate())
                {
                    var s1 = filter.Values.FirstOrDefault() ?? String.Empty;
                    if (DateTimeOffset.TryParse(s1, out DateTimeOffset d1))
                    {
                        switch (filter.Type)
                        {
                            case FilterTypes.DoesNotEqual:
                            case FilterTypes.Equals:
                            case FilterTypes.GreaterThan:
                            case FilterTypes.GreaterThanOrEqual:
                            case FilterTypes.LessThan:
                            case FilterTypes.LessThanOrEqual:
                                newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} {op} @0", d1.UtcDateTime);
                                break;

                            case FilterTypes.Range:
                                var s2 = filter.Values[1] ?? String.Empty;
                                if (DateTimeOffset.TryParse(s2, out DateTimeOffset d2))
                                {
                                    newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} >= @0 && {property} <= @1", d1.UtcDateTime, d2.UtcDateTime);
                                }
                                break;
                        }
                    }
                }
                else if (propertyType.IsInteger() || propertyType.IsFloat())
                {
                    var s1 = filter.Values.FirstOrDefault() ?? String.Empty;
                    var n1 = Convert.ChangeType(s1, propertyType);
                    switch (filter.Type)
                    {
                        case FilterTypes.DoesNotEqual:
                        case FilterTypes.Equals:
                        case FilterTypes.GreaterThan:
                        case FilterTypes.GreaterThanOrEqual:
                        case FilterTypes.LessThan:
                        case FilterTypes.LessThanOrEqual:
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} {op} @0", n1);
                            break;

                        case FilterTypes.Range:
                            var s2 = filter.Values[1]?.ToString() ?? String.Empty;
                            var n2 = Convert.ChangeType(s2, propertyType);
                            newPredicate = DynamicExpressionParser.ParseLambda<TItem, bool>(ParsingConfig.Default, false, $"{property} >= @0 && {property} <= @1", n1, n2);
                            break;
                    }
                }
            }

            return newPredicate is null ? query : query.Filter(newPredicate);
        }

        protected virtual IBoundClient<TItem> ApplyFilters(IBoundClient<TItem> query, IEnumerable<Filter> filters)
        {
            foreach (var filter in filters)
            {
                query = ApplyFilter(query, filter);
            }
            return query;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && _tableToolbar != null && _table != null)
            {
                _tableToolbar.SetTable(_table);
            }
        }

        protected virtual string GetPropertyName(Filter filter, BfColumn<TItem, TKey> column)
        {
            var members = PropertyPath<TItem>.Get(column.DataMember);
            return string.Join(".", members.Select(p => p.Name));
        }

        protected virtual Type? GetPropertyType(Filter filter, BfColumn<TItem, TKey> column)
        {
            if (column.DataType != null)
            {
                return column.DataType;
            }
            var members = PropertyPath<TItem>.Get(column.DataMember);
            return members.LastOrDefault() is PropertyInfo pi ? pi.PropertyType : null;
        }

        protected virtual async Task<IEnumerable<TItem>> OnGetData(PageInfo pageInfo, SortInfo sortInfo, FilterInfo filterInfo)
        {
            try
            {
                var annotations = new ODataFeedAnnotations();
                var query = GetBaseQuery();
                if (filterInfo.Filters.Count > 0)
                {
                    // perform a advanced search
                    query = ApplyFilters(query, filterInfo.Filters);
                }
                else if (!string.IsNullOrWhiteSpace(filterInfo.SearchText))
                {
                    // perform a basic search
                    query = ApplyBasicFilter(query, filterInfo.SearchText);
                }
                if (pageInfo != null)
                {
                    // page query
                    query = query.Skip(pageInfo.PageRangeStart - 1).Top(pageInfo.PageSize);
                }
                if (sortInfo != null)
                {
                    // sort query
                    var col = _table.AllColumns.FirstOrDefault(x => x.Id == sortInfo.ColumnId);
                    if (col != null)
                    {
                        query = sortInfo.Direction == SortDirections.Ascending
                            ? query.OrderBy(col.DataMember)
                            : query.OrderByDescending(col.DataMember);
                    }
                }
                var results = await query.FindEntriesAsync(annotations).ConfigureAwait(true);
                if (pageInfo != null)
                {
                    pageInfo.TotalCount = (int)(annotations?.Count ?? 0);
                }
                return results.ToArray();
            }
            catch (Exception ex)
            {
                NotificationService?.Notify(NotificationLevels.Error, ex.Message, "Fetch Data");
            }
            return Array.Empty<TItem>();
        }

        protected virtual void OnSelectionChanged(IEnumerable<TKey> keys)
        {
            _selectedId = keys.Any() ? keys.First() : default;
        }

        protected override void OnInitialized()
        {
            if (DefaultSort != null)
            {
                _sortInfo = DefaultSort;
            }
        }
    }
}
