using Domain.Validations.RequestValidations;
using System;

namespace Domain.DTOs.Request
{
    public class CourseRequest
    {
        [RequiredField(nameof(Title))]
        public string Title { get; set; }
        public string Description { get; set; }
        [DateRange("2020-01-01","2050-12-31", nameof(PublishingDate))]
        public DateTime? PublishingDate { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
