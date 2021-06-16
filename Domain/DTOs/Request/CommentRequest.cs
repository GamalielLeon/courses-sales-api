using Domain.Validations.RequestValidations;
using System;

namespace Domain.DTOs.Request
{
    public class CommentRequest
    {
        [RequiredField]
        public Guid? CourseId { get; set; }
        [RequiredField]
        [StringLengthField(60, 3)]
        public string StudentName { get; set; }
        [StringLengthField(1500, 4)]
        public string Message { get; set; }
        [RequiredField]
        [RangeField(1, 5)]
        public byte? Score { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
