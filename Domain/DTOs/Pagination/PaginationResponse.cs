using System.Collections.Generic;

namespace Domain.DTOs.Pagination
{
    public class PaginationResponse<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public long TotalRecords { get; set; }
        public ICollection<T> Results { get; set; }
    }
}
