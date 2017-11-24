using System.Data;
using System.Collections.Generic;
using System;

namespace Mimo.Framework.Common
{
    public static class DataUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool DataSetHasRows(DataSet ds)
        {
            return  ds != null && 
                    ds.Tables != null && 
                    ds.Tables.Count > 0 && 
                    ds.Tables[0].Rows != null &&
                    ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool DataTableHasRows(DataTable dt)
        {
            return  dt != null &&
                    dt.Rows != null &&
                    dt.Rows.Count > 0;
        }

        public static bool IsEmpty(object rowData)
        {
            return rowData.ToString() != string.Empty ? false : true; 
        }

        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);          //add a column to table for each public property on T         
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                t.Columns.Add(propInfo.Name, ColType);
            }
            //go through each property on T and add each value to the table         
            foreach (T item in list)
            {
                DataRow row = t.NewRow();
                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }
                t.Rows.Add(row);
            }
            return ds;
        }
    }
}
