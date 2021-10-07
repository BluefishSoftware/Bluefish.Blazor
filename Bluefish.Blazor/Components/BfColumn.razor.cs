using Bluefish.Blazor.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Bluefish.Blazor.Components
{
    public partial class BfColumn<TItem, TKey>
    {
        private Lazy<Func<TItem, object>> _dataMemberGetter;

        [Parameter]
        public Alignment Align { get; set; }

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
        public string HeaderToolTip { get; set; }

        [Parameter]
        public bool IsSortable { get; set; } = true;

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [CascadingParameter(Name = "Table")]
        public BfTable<TItem, TKey> Table { get; set; }

        [Parameter]
        public RenderFragment<TItem> Template { get; set; }

        [Parameter]
        public Func<TItem, string> ToolTip { get; set; }

        private static string GetCssClass(Alignment align) => align switch
        {
            Alignment.Start => "text-start",
            Alignment.Center => "text-center",
            Alignment.End => "text-end",
            _ => string.Empty
        };
        public string GetHeaderCssClass(BfTable<TItem, TKey> table)
        {
            return $"{HeaderCssClass} {(table.AllowSort && IsSortable ? "cursor-pointer" : "")} {GetCssClass(Align)}";
        }

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

        protected override void OnInitialized()
        {
            Table?.AddColumn(this);

            if (DataMember != null)
            {
                _dataMemberGetter = new Lazy<Func<TItem, object>>(() => DataMember.Compile());
            }
        }
    }
}
