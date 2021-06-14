using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceCourse : ServiceGeneric<Course>, IServiceCourse
    {
        private readonly IGenericRepository<CourseInstructor> _courseInstructorRepository;
        public ServiceCourse(IGenericRepository<Course> repository, IUnitOfWork unitOfWork, IGenericRepository<CourseInstructor> courseInstructorRepository) : base(repository, unitOfWork)
        {
            _courseInstructorRepository = courseInstructorRepository;
        }

        public virtual Course GetWithInstructors(Guid id, params Expression<Func<Course, object>>[] includeProperties)
        {
            return _repository.GetIncluding(id, includeProperties);
        }

        public virtual async Task<Course> GetWithInstructorsAsync(Guid id, params Expression<Func<Course, object>>[] includeProperties)
        {
            return await _repository.GetIncludingAsync(id, includeProperties);
        }

        public virtual IQueryable<Course> GetAllWithInstructors(params Expression<Func<Course, object>>[] includeProperties)
        {
            return _repository.GetAllIncluding(includeProperties);
        }

        public virtual async Task<ICollection<Course>> GetAllWithInstructorsAsync(params Expression<Func<Course, object>>[] includeProperties)
        {
            return await _repository.GetAllIncludingAsync(includeProperties);
        }

        public IQueryable<CourseInstructor> GetAllCourseInstructors()
        {
            return _courseInstructorRepository.GetAllIncluding(static ci => ci.Instructor);
        }

        public IQueryable<CourseInstructor> FindByCourseInstructors(Expression<Func<CourseInstructor, bool>> predicate)
        {
            return _courseInstructorRepository.FindByIncluding(predicate, static ci => ci.Instructor);
        }
    }
}
