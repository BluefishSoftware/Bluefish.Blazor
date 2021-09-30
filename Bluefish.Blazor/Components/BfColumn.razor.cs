using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Bluefish.Blazor.Components
{
    public partial class BfColumn<TItem, TKey>
    {
        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public string DataField { get; set; }

        [Parameter]
        public Expression<Func<TItem, object>> DataMember { get; set; }

        [Parameter]
        public string HeaderCssClass { get; set; }

        [Parameter]
        public string HeaderText { get; set; }

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [CascadingParameter(Name = "Table")]
        public BfTable<TItem, TKey> Table { get; set; }

        [Parameter]
        public RenderFragment<TItem> Template { get; set; }

        [Parameter]
        public Func<TItem, string> ToolTip { get; set; }

        protected override void OnInitialized()
        {
            Table?.AddColumn(this);

            if (DataMember != null)
            {
                _dataMemberGetter = new Lazy<Func<TItem, object>>(() => DataMember.Compile());
            }
        }

        private Lazy<Func<TItem, object>> _dataMemberGetter;

        public object GetValue(TItem item)
        {
            if (_dataMemberGetter != null)
            {
                try
                {
                    return _dataMemberGetter.Value.Invoke(item);
                }
                catch
                {
                    return null;
                }
            }

            var propInfo = item?.GetType()?.GetProperty(DataField);
            if (propInfo is null)
            {
                return null;
            }
            return propInfo.GetValue(item);
        }
    }
}
