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

        public Course AddWithInstructors(Course course)
        {
            Guid[] instructorIds = course.CourseInstructors.Select(static ci => ci.Id).ToArray();
            ICollection<CourseInstructor> courseInstructors = new List<CourseInstructor>();
            foreach (Guid instructorId in instructorIds)
            {
                courseInstructors.Add(new CourseInstructor() { InstructorId = instructorId });
            }
            course.CourseInstructors = courseInstructors;
            Course courseCreated = _repository.Add(course);
            _unitOfWork.Save();
            return courseCreated;
        }

        public async Task<Course> AddWithInstructorsAsync(Course course)
        {
            Guid[] instructorIds = course.CourseInstructors.Select(static ci => ci.Id).ToArray();
            ICollection<CourseInstructor> courseInstructors = new List<CourseInstructor>();
            foreach (Guid instructorId in instructorIds)
            {
                courseInstructors.Add(new CourseInstructor() { InstructorId = instructorId });
            }
            course.CourseInstructors = courseInstructors;
            Course courseCreated = await _repository.AddAsync(course);
            await _unitOfWork.SaveAsync();
            return courseCreated;
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
