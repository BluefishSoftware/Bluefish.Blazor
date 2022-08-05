namespace Bluefish.Blazor.Interfaces;

public interface IDataSource<TItem>
{
    Task<IEnumerable<TItem>> FetchAsync(PageInfo pageInfo = null, SortInfo sortInfo = null, FilterInfo filterInfo = null);
}

public interface IDataUpdater<TItem>
{
    Task<bool> UpdateAsync(TItem item, IDictionary<string, object> updates);
}