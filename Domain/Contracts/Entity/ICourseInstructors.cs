using Domain.DTOs.Response;
using System.Collections.Generic;

namespace Domain.Contracts.Entity
{
    public interface ICourseInstructors
    {
        public ICollection<CourseInstructorResponse> Instructors { get; set; }
    }
}
