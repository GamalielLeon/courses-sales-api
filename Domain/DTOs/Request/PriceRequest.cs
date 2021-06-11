using Domain.Validations.RequestValidations;
using System;

namespace Domain.DTOs.Request
{
    public class PriceRequest
    {
        [RequiredField(nameof(CourseId))]
        public Guid CourseId { get; set; }
        [RequiredField(nameof(CurrentPrice))]
        [NumberRange(0,999999, nameof(CurrentPrice))]
        public decimal? CurrentPrice { get; set; }
        [NumberRange(0, 999999, nameof(Promotion))]
        public decimal Promotion { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
