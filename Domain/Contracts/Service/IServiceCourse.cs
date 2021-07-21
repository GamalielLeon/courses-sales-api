using Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceCourse : IServiceGeneric<Course>
    {
        Task<Course> AddWithInstructorsAndPriceAsync(Course course);
        Task<Course> UpdateWithInstructorsAndPriceAsync(Course course, Guid id);
        IQueryable<CourseInstructor> GetAllCourseInstructors();
        IQueryable<CourseInstructor> FindByCourseInstructors(Expression<Func<CourseInstructor, bool>> predicate);
    }
}
