using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopping.WebApi.Helpers
{
    public class Paginator<T> where T: class
    {
        public int CurrentPage { get; set; }
        public int RecordsPerPage { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Records { get; set; }

        public Paginator(IEnumerable<T> records, int total, int currentPage, int recordsPerPage) 
        {
            Records = records;
            TotalRecords = total;
            CurrentPage = currentPage;
            RecordsPerPage = recordsPerPage;
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalRecords / (double)RecordsPerPage);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

    }
}
