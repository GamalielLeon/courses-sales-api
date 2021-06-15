using System;

namespace Domain.DTOs.Response
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string StudentName { get; set; }
        public string Message { get; set; }
        public byte Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
