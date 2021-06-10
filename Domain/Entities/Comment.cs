using Domain.Contracts.Entity;
using System;

namespace Domain.Entities
{
    public class Comment : IEntity
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string StudentName { get; set; }
        public string Message { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual Course Course { get; set; }
    }
}
