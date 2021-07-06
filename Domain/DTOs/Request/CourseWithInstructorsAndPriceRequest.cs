using Domain.Validations.RequestValidations;
using System;
using System.Collections.Generic;

namespace Domain.DTOs.Request
{
    public class CourseWithInstructorsAndPriceRequest
    {
        [RequiredField]
        [StringLengthField(500, 5)]
        public string Title { get; set; }
        [StringLengthField(1000, 10)]
        public string Description { get; set; }
        [RangeField("2020-01-01", "2050-12-31")]
        public DateTime? PublishingDate { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        [RequiredField]
        public PriceRequest Price { get; set; }
        [RequiredField]
        public ICollection<CourseInstructorRequest> CourseInstructors { get; set; }
    }
}
