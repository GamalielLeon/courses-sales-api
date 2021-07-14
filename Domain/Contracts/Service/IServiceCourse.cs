using Domain.DTOs.Pagination;
using Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceCourse : IServiceGeneric<Course, CoursesPaged>
    {
        Course AddWithInstructorsAndPrice(Course course);
        Task<Course> AddWithInstructorsAndPriceAsync(Course course);
        Course UpdateWithInstructorsAndPrice(Course course, Guid id);
        Task<Course> UpdateWithInstructorsAndPriceAsync(Course course, Guid id);
        IQueryable<CourseInstructor> GetAllCourseInstructors();
        IQueryable<CourseInstructor> FindByCourseInstructors(Expression<Func<CourseInstructor, bool>> predicate);
    }
}
