using Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceCourse : IServiceGeneric<Course>
    {
        Course AddWithInstructors(Course course);
        Task<Course> AddWithInstructorsAsync(Course course);
        Course UpdateWithInstructors(Course course, Guid id);
        Task<Course> UpdateWithInstructorsAsync(Course course, Guid id);
        IQueryable<CourseInstructor> GetAllCourseInstructors();
        IQueryable<CourseInstructor> FindByCourseInstructors(Expression<Func<CourseInstructor, bool>> predicate);
    }
}
