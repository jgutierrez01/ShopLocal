using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAM.Web.Shop.Models
{
    [DataContract]
    public class PagedResult<T> : PagerModel
    {
        public PagedResult(IEnumerable<T> rows, int pageSize, int pageNumber, int totalRows)
        {
            Records = rows;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalRows = totalRows;
        }

        public IEnumerable<T> Records { get; set; }
    }

    [DataContract]
    public abstract class PagerModel
    {
        [DataMember]
        public int PageNumber { get; set; }
        
        [DataMember]
        public int PageSize { get; set; }
        
        [DataMember]
        public int TotalRows { get; set; }
        
        [DataMember]
        public int TotalPages
        {
            get { return (int)Math.Ceiling(TotalRows / (1.0m * PageSize)); }
        }
    }
}