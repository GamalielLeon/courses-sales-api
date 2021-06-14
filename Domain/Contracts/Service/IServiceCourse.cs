using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceCourse : IServiceGeneric<Course>
    {
        Course GetWithInstructors(Guid id, params Expression<Func<Course, object>>[] includeProperties);
        Task<Course> GetWithInstructorsAsync(Guid id, params Expression<Func<Course, object>>[] includeProperties);
        IQueryable<Course> GetAllWithInstructors(params Expression<Func<Course, object>>[] includeProperties);
        Task<ICollection<Course>> GetAllWithInstructorsAsync(params Expression<Func<Course, object>>[] includeProperties);
        IQueryable<CourseInstructor> GetAllCourseInstructors();
        IQueryable<CourseInstructor> FindByCourseInstructors(Expression<Func<CourseInstructor, bool>> predicate);
    }
}
