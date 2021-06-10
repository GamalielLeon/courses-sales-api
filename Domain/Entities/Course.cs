using Domain.Contracts.Entity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishingDate { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        
        public virtual Price Price { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}
