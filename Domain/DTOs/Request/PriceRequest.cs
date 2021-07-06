using Domain.Validations.RequestValidations;
using System;

namespace Domain.DTOs.Request
{
    public class PriceRequest
    {
        public Guid? CourseId { get; set; }
        [RequiredField]
        [RangeField(0,999999)]
        public decimal? CurrentPrice { get; set; }
        [RangeField(0, 999999)]
        public decimal Promotion { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
