using System;

namespace Mimo.Framework.Data
{
    public class ObjectSetFilter
    {
        public string TableAliasName { get; set; }
        public string ColumnName { get; set; }
        public QueryOperator Operator { get; set; }
        public string FilterValue { get; set; }
        public string ColumnType { get; set; }

        private string _eSQLWhere = string.Empty;

        public string OperatorStringValue()
        {
            string op = string.Empty;

            switch (Operator)
            {
                case QueryOperator.EqualTo:
                    op = " = {0} ";
                    break;
                case QueryOperator.DifferentThan:
                    op = " != {0} ";
                    break;
                case QueryOperator.GreaterThan:
                    op = " > {0} ";
                    break;
                case QueryOperator.LessThan:
                    op = " < {0} " ;
                    break;
                case QueryOperator.GreaterThanOrEqualTo:
                    op = " >= {0} ";
                    break;
                case QueryOperator.LessThanOrEqualTo:
                    op = " <= {0}" ;
                    break;
                case QueryOperator.Contains:
                    op = " like \"%\" + {0} + \"%\" ";
                    break;
                case QueryOperator.StartsWith:
                    op = " like {0} + \"%\" ";
                    break;
                case QueryOperator.EndsWith:
                    op = " like \"%\" + {0} ";
                    break;
                default:
                       throw new NotSupportedException("This operator does not have a string representation and therefore is not supported");
            }

            return op;
        }

        public object FilterTypedValue()
        {
            switch (ColumnType)
            {
                case "System.Int32":
                    return Convert.ToInt32(FilterValue);
                case "System.String":
                    return FilterValue;
                case "System.Double":
                    return Convert.ToDouble(FilterValue);
                case "System.DateTime":
                    return Convert.ToDateTime(FilterValue);
                case "System.Byte":
                    return Convert.ToByte(FilterValue);
                default:
                    throw new NotSupportedException("This type is not supported for filtering");
            }
        }

        public string GetESqlWhere()
        {
            if (string.IsNullOrEmpty(_eSQLWhere))
            {
                string tblPrefix = "it." + (string.IsNullOrEmpty(TableAliasName) ? string.Empty : TableAliasName + ".") + ColumnName;
                tblPrefix = string.Concat(tblPrefix, string.Format(OperatorStringValue(), "@" + ColumnName));
                _eSQLWhere = tblPrefix;
            }

            return _eSQLWhere;
        }

        public void SetESqlWhere(string whereClause)
        {
            _eSQLWhere = whereClause;
        }
    }


    public enum QueryOperator
    {
        NoOperator = 0,
        EqualTo,
        DifferentThan,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        Between,
        NotBetween,
        Contains,
        StartsWith,
        EndsWith
    }
}