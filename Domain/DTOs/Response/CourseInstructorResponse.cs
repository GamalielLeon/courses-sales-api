using System;

namespace Domain.DTOs.Response
{
    public class CourseInstructorResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
    }
}
