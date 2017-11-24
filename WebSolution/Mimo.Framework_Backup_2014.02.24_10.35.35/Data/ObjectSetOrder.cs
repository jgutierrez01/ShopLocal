namespace Mimo.Framework.Data
{
    public class ObjectSetOrder
    {
        public string TableAliasName { get; set; }
        public string ColumnName { get; set; }
        public SortOrder Order { get; set; }
        
        public string GetESqlSort()
        {
            string colPrefix = "it." + (string.IsNullOrEmpty(TableAliasName) ? string.Empty : TableAliasName + ".") + ColumnName;
            colPrefix = string.Concat(colPrefix, " ", (Order == SortOrder.Ascending ? "asc" : "desc"));
            return colPrefix;
        }
    }

    public enum SortOrder
    {
        None,
        Ascending,
        Descending
    }
}