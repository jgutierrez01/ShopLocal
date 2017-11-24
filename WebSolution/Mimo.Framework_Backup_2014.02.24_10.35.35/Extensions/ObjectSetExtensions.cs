using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using Mimo.Framework.Data;

namespace Mimo.Framework.Extensions
{
    public static class ObjectSetExtensions
    {

        /// <summary>
        /// Requires a valid context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ObjectQuery<T> Search<T>(this ObjectSet<T> set, List<ObjectSetFilter> filter) where T : class
        {
            ObjectQuery<T> query = (ObjectQuery<T>)set;

            query = query.Search(filter);

            return query;
        }


        public static ObjectQuery<T> Search<T>(this ObjectQuery<T> query, List<ObjectSetFilter> filter)
        {
            if (filter == null || filter.Count <= 0)
            {
                throw new ArgumentException("Parameter filter must contain at least one filter");
            }

            filter.ForEach(x => query = query.Where(
                                                        x.GetESqlWhere(),
                                                        new ObjectParameter(x.ColumnName, x.FilterTypedValue())
                                                    )
                            );

            return query;        
        }


        /// <summary>
        /// Requires a valid context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="totalRowCount"></param>
        /// <returns></returns>
        public static ObjectQuery<T> Page<T>(this ObjectSet<T> set, int pageSize, int pageIndex, List<ObjectSetFilter> filter, List<ObjectSetOrder> order, ref int totalRowCount) where T : class
        {
            ObjectQuery<T> query = (ObjectQuery<T>)set;

            query = query.Page(pageSize, pageIndex, filter, order, ref totalRowCount);

            return query;
        }

        public static ObjectQuery<T> Page<T>(this ObjectQuery<T> query, int pageSize, int pageIndex, List<ObjectSetFilter> filter, List<ObjectSetOrder> order, ref int totalRowCount)
        {
            if (order == null || order.Count <= 0)
            {
                throw new ArgumentException("Parameter order must contain at least one sorting order");
            }

            if (filter != null && filter.Count > 0)
            {
                query = query.Search(filter);
            }

            string sortOrder = string.Empty;
            totalRowCount = query.Count();

            order.ForEach(x => sortOrder += x.GetESqlSort() + ",");
            sortOrder = sortOrder.Substring(0, sortOrder.Length - 1);

            query = query.Skip(sortOrder,
                                "@skip",
                                new ObjectParameter("skip", pageSize * pageIndex))
                         .Top("@top",
                                new ObjectParameter("top", pageSize));

            return query;
        }

    }
}