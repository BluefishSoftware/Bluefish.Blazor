namespace Bluefish.Blazor.Models;

/// <summary>
///  The PageInfo class contain information of the current page as well as pages available.
/// </summary>
public class PageInfo
{
    public static int DEFAULT_PAGE_SIZE = 10;

    private int _page;
    private int _pageSize = DEFAULT_PAGE_SIZE;
    private int _totalCount;

    /// <summary>
    /// Initializes a new instance of the PageInfo class.
    /// </summary>
    public PageInfo()
    {
        Page = 1;
    }

    /// <summary>
    /// Initializes a new instance of the PageCriteria class.
    /// </summary>
    /// <param name="page">Page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="totalCount">The total number of items.</param>
    public PageInfo(int page, int pageSize = 10, int totalCount = 0)
    {
        _page = page;
        _pageSize = pageSize;
        _totalCount = totalCount;
    }

    /// <summary>
    /// Event raised whenever the current page number or page size changes.
    /// </summary>
    public event EventHandler Changed;

    /// <summary>
    /// Event raised whenever the total item count changes.
    /// </summary>
    public event EventHandler TotalCountChanged;

    public void Init(int pageSize, int pageNumber)
    {
        _pageSize = pageSize;
        _page = pageNumber;
    }

    /// <summary>
    /// Gets or sets page number.
    /// </summary>
    public int Page
    {
        get { return _page; }
        set
        {
            if (_page == value || value <= 0 || (value > 1 && value > PageCount))
            {
                return;
            }
            _page = value;
            OnChanged();
        }
    }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException("Must be greater than 0", nameof(PageSize));
            }
            if (_pageSize == value)
            {
                return;
            }
            _pageSize = value;
            if (_page > PageCount)
            {
                Page = PageCount;
            }
            OnChanged();
        }
    }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int TotalCount
    {
        get { return _totalCount; }
        set
        {
            if (_totalCount == value)
            {
                return;
            }
            _totalCount = value;
            if (_totalCount == 0)
            {
                Page = 1;
            }
            else if (_page > PageCount)
            {
                Page = PageCount;
            }
            OnTotalCountChanged();
        }
    }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int PageCount
    {
        get
        {
            return (_totalCount / _pageSize) + (_totalCount % _pageSize > 0 ? 1 : 0);
        }
    }

    /// <summary>
    /// Calculates the index of the first item of the current page.
    /// </summary>
    public int PageRangeStart
    {
        get
        {
            return ((_page - 1) * _pageSize) + 1;
        }
    }

    /// <summary>
    /// Calculates the index of the last item of the current page.
    /// </summary>
    public int PageRangeEnd
    {
        get
        {
            var last = ((_page - 1) * _pageSize) + _pageSize;
            return last > _totalCount ? _totalCount : last;
        }
    }

    /// <summary>
    /// Gets whether the current page is the first page.
    /// </summary>
    public bool IsFirstPage => _page == 1;

    /// <summary>
    /// Gets whether the current page is the last page.
    /// </summary>
    public bool IsLastPage => _page == PageCount;

    /// <summary>
    /// Gets the number of items before the current page.
    /// </summary>
    public int PreviousItems => (_page - 1) * _pageSize;

    protected void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }

    protected void OnTotalCountChanged()
    {
        TotalCountChanged?.Invoke(this, EventArgs.Empty);
    }
}
