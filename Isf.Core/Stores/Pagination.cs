using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Stores
{
    public class PagedList<T>
    {
        public Paging Paging { get; set; }
        public IEnumerable<T> List { get; set; }
    }

    public class Paging
    {
        public int Page { get; set; } = 1;
        public string OrderBy { get; set; }
        public int PageSize { get; set; }

        public int TotalItems { get; set; }
        public int TotalPages
        {
            get
            {
                if (TotalItems == 0)
                {
                    return 0;
                }
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }

        public bool IsValid()
        {
            return Page > 0
                && !string.IsNullOrWhiteSpace(OrderBy)
                && PageSize > 0;
        }
    }
}
