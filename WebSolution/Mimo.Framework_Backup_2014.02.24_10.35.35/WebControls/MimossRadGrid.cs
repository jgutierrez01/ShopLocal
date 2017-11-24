using System;
using System.Collections.Generic;
using System.Linq;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using System.ComponentModel;
using System.Web.UI;
using Mimo.Framework.Resources;

namespace Mimo.Framework.WebControls
{
    public class MimossRadGrid : RadGrid
    {

        #region Constructor

        /// <summary>
        /// Aquí establecemos lo que es por default.  A estas alturas el ViewState ni siquiera existe.
        /// </summary>
        public MimossRadGrid()
            : base()
        {
            this.AllowPaging = true;
            this.AllowSorting = true;
            this.AllowFilteringByColumn = true;
            this.EnableHeaderContextMenu = true;
            this.EnableEmbeddedSkins = false;
            this.ShowStatusBar = true;
            this.ShowGroupPanel = false;

            this.PagerStyle.AlwaysVisible = true;
            this.PagerStyle.PageButtonCount = 6;
            this.PagerStyle.Mode = GridPagerMode.NextPrevAndNumeric;

            this.GroupingSettings.CaseSensitive = false;
            this.GroupingSettings.ShowUnGroupButton = false;

            this.ClientSettings.AllowColumnHide = true;
            this.ClientSettings.AllowColumnsReorder = true;
            this.ClientSettings.ColumnsReorderMethod = GridClientSettings.GridColumnsReorderMethod.Reorder;
            this.ClientSettings.EnableRowHoverStyle = true;
            this.ClientSettings.ReorderColumnsOnClient = false;
            this.ClientSettings.Resizing.AllowColumnResize = true;
            this.ClientSettings.Resizing.ResizeGridOnColumnResize = false;
            this.ClientSettings.Scrolling.ScrollHeight = 400;
            this.ClientSettings.Scrolling.UseStaticHeaders = true;
            this.ClientSettings.Scrolling.AllowScroll = true;
            this.ClientSettings.Selecting.AllowRowSelect = true;

            this.MasterTableView.PageSize = 50;

            this.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
        }

        #endregion

        #region Private "per-request" variables

        private int _newPageSize = 0;
        private ObjectSetOrder _newOrder = null;
        private ObjectSetFilter _newFilter = null;

        #endregion

        #region Properties

        [Browsable(true)]
        public int FreezeColumnStartIndex
        {
            get
            {
                object o = ControlState["FreezeColumnStartIndex"];

                if (o == null)
                {
                    return 2;
                }
                else
                {
                    return (int)o;
                }
            }
            set
            {
                ControlState["FreezeColumnStartIndex"] = value;
            }
        }

        [Browsable(true)]
        public bool AllowColumnFreezing
        {
            get
            {
                object o = ControlState["AllowColumnFreezing"];

                if (o == null)
                {
                    return true;
                }
                else
                {
                    return (bool)o;
                }
            }
            set
            {
                ControlState["AllowColumnFreezing"] = value;
            }
        }

        [Browsable(false)]
        public int SelectedPageSize
        {
            get
            {
                return _newPageSize > 0 ? _newPageSize : MasterTableView.PageSize;
            }
        }

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            if (AllowColumnFreezing)
            {
                HeaderContextMenu.ItemClick += new RadMenuEventHandler(HeaderContextMenu_ItemClick);

                if (string.IsNullOrEmpty(ClientSettings.ClientEvents.OnHeaderMenuShowing))
                {
                    ClientSettings.ClientEvents.OnHeaderMenuShowing = "Sam.Utilerias.OnGridHeaderMenuShowing";
                }
            }

            base.OnInit(e);
        }

        protected override void OnItemCommand(GridCommandEventArgs e)
        {

            if (!EnableViewState)
            {
                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filterPair = (Pair)e.CommandArgument;

                    string filterName = filterPair.First.ToString(); //Query operator
                    string uniqueName = filterPair.Second.ToString(); //Column uniquename

                    GridColumn column = MasterTableView.Columns.FindByUniqueName(uniqueName);

                    _newFilter = new ObjectSetFilter
                    {
                        ColumnName = ((GridBoundColumn)column).DataField,
                        FilterValue = column.CurrentFilterValue,
                        Operator = getQueryOperator(filterName),
                        ColumnType = column.DataTypeName
                    };

                    MasterTableView.CurrentPageIndex = 0;
                    forceBind();
                }
            }

            base.OnItemCommand(e);
        }

        protected override void OnSortCommand(GridSortCommandEventArgs e)
        {
            if (!EnableViewState)
            {
                _newOrder = new ObjectSetOrder { ColumnName = e.SortExpression, Order = e.NewSortOrder.ToObjectSetOrder() };
                MasterTableView.CurrentPageIndex = 0;
                forceBind();
            }

            base.OnSortCommand(e);
        }

        protected override void OnPageSizeChanged(GridPageSizeChangedEventArgs e)
        {
            if (!EnableViewState)
            {
                _newPageSize = e.NewPageSize;
                MasterTableView.CurrentPageIndex = 0;
                forceBind();
            }

            base.OnPageSizeChanged(e);
        }

        protected void HeaderContextMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            RadMenuItem item = e.Item;

            if (item.Value == "Freeze")
            {
                GridColumn column = Columns.FindByUniqueNameSafe(item.Attributes["ColumnName"]);

                if (column != null)
                {
                    ClientSettings.Scrolling.FrozenColumnsCount = column.OrderIndex - FreezeColumnStartIndex;
                }
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            removeSortAndGroupingOptions();

            if (AllowColumnFreezing)
            {
                addFreezeMenuOption();
            }
        }

        #endregion

        #region Public Methods

        public List<ObjectSetFilter> GetCurrentFilters()
        {
            List<ObjectSetFilter> list = new List<ObjectSetFilter>();
            foreach (GridColumn column in MasterTableView.Columns)
            {
                appendFilter(list, column);
            }

            if (_newFilter != null)
            {
                ObjectSetFilter filter = list.Where(x => x.ColumnName == _newFilter.ColumnName).FirstOrDefault();

                if (filter != null)
                {
                    if (_newFilter.Operator == QueryOperator.NoOperator)
                    {
                        list.Remove(filter);
                    }
                    else
                    {
                        filter = _newFilter;
                    }
                }
                else if (_newFilter.Operator != QueryOperator.NoOperator)
                {
                    list.Add(_newFilter);
                }
            }

            return list;
        }

        public List<ObjectSetOrder> GetCurrentSortings()
        {
            List<ObjectSetOrder> lst = null;

            if (MasterTableView.SortExpressions.Count == 0 && _newOrder == null)
            {
                lst = null;
            }
            else
            {
                lst = new List<ObjectSetOrder>();
                string sortClause = string.Empty;

                foreach (GridSortExpression sortExp in MasterTableView.SortExpressions)
                {
                    lst.Add(new ObjectSetOrder { ColumnName = sortExp.FieldName, Order = sortExp.SortOrder.ToObjectSetOrder() });
                }
            }

            if (_newOrder != null)
            {
                ObjectSetOrder order = lst.Where(x => x.ColumnName == _newOrder.ColumnName).FirstOrDefault();

                if (order != null)
                {
                    order.Order = _newOrder.Order;
                }
                else
                {
                    lst.Add(_newOrder);
                }

                lst = lst.Where(x => x.Order != SortOrder.None).ToList();
            }

            return lst;
        }

        public void ResetBind()
        {
            foreach (GridColumn column in this.Columns)
            {
                column.ResetCurrentFilterValue();
            }
            this.MasterTableView.FilterExpression = string.Empty;
            this.MasterTableView.CurrentPageIndex = 0;
        }

        #endregion

        #region Private/Helper methods

        private void forceBind()
        {
            DataSource = null;
            Rebind();
        }

        private void removeSortAndGroupingOptions()
        {
            RadMenuItem menu = HeaderContextMenu.Items.Where(x => x.Value == "ColumnsContainer").SingleOrDefault();

            if (menu != null)
            {
                for (int i = menu.Items.Count - 1; i >= 0; i--)
                {
                    string colUniqueName = menu.Items[i].Value.Split('|')[1];

                    if (colUniqueName.EndsWith("_h"))
                    {
                        menu.Items.RemoveAt(i);
                    }
                }
            }
        }

        private void addFreezeMenuOption()
        {
            RadContextMenu menu = HeaderContextMenu;
            RadMenuItem item = new RadMenuItem();
            item.Text = MimossRadGridMessages.Freeze;
            item.PostBack = true;
            item.Value = "Freeze";
            item.Attributes["TableID"] = string.Empty;
            item.Attributes["ColumnName"] = string.Empty;
            menu.Items.Add(item);
        }

        private static void appendFilter(List<ObjectSetFilter> list, GridColumn column)
        {
            if (column is GridBoundColumn)
            {
                ObjectSetFilter filter = new ObjectSetFilter
                {
                    ColumnName = ((GridBoundColumn)column).DataField,
                    FilterValue = column.CurrentFilterValue,
                    Operator = getQueryOperator(column.CurrentFilterFunction.ToString()),
                    ColumnType = column.DataTypeName
                };

                if (filter.Operator != QueryOperator.NoOperator)
                {
                    list.Add(filter);
                }
            }
        }

        private static QueryOperator getQueryOperator(string currentFilter)
        {
            GridKnownFunction function = (GridKnownFunction)Enum.Parse(typeof(GridKnownFunction), currentFilter);

            switch (function)
            {
                case GridKnownFunction.Contains:
                    return QueryOperator.Contains;
                case GridKnownFunction.EqualTo:
                    return QueryOperator.EqualTo;
                case GridKnownFunction.GreaterThan:
                    return QueryOperator.GreaterThan;
                case GridKnownFunction.GreaterThanOrEqualTo:
                    return QueryOperator.GreaterThanOrEqualTo;
                case GridKnownFunction.LessThan:
                    return QueryOperator.LessThan;
                case GridKnownFunction.LessThanOrEqualTo:
                    return QueryOperator.LessThanOrEqualTo;
                case GridKnownFunction.StartsWith:
                    return QueryOperator.StartsWith;
                case GridKnownFunction.EndsWith:
                    return QueryOperator.EndsWith;
                default:
                    return QueryOperator.NoOperator;
            }
        }

        #endregion
    }
}