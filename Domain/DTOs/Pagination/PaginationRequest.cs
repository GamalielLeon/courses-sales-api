using Domain.Constants;
using Domain.Validations.RequestValidations;

namespace Domain.DTOs.Pagination
{
    public class PaginationRequest
    {
        [RangeField(1, int.MaxValue/GlobalConstants.MAX_PAGE_SIZE)]
        public int Page { get; set; } = 1;
        [RangeField(1, GlobalConstants.MAX_PAGE_SIZE)]
        public int PageSize { get; set; } = GlobalConstants.DEFAULT_PAGE_SIZE_10;
        public string SortBy { get; set; } = GlobalConstants.DEFAULT_SORT_BY_ID;
        public bool IsSortDescendent { get; set; } = false;
    }
}
