using Domain.Contracts.Entity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Instructor : IEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}
