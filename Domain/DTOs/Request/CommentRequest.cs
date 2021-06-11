using Domain.Validations.RequestValidations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Request
{
    public class CommentRequest
    {
        [RequiredField(nameof(CourseId))]
        public Guid CourseId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [RequiredField(nameof(StudentName))]
        public string StudentName { get; set; }
        public string Message { get; set; }
        [RequiredField(nameof(Score))]
        [NumberRange(1,5, nameof(Score))]
        public int? Score { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
