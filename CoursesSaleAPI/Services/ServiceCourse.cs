using CoursesSaleAPI.Helpers.ErrorHandler;
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
            //If an array is not used, an exception is thrown since the collection gets modified at runtime.
            Guid[] instructorIds = course.CourseInstructors.Select(static ci => ci.Id).ToArray();
            ICollection<CourseInstructor> courseInstructors = new List<CourseInstructor>();
            foreach (Guid instructorId in instructorIds)
            {
                courseInstructors.Add(new CourseInstructor() { InstructorId = instructorId });
            }

            course.CreatedAt = DateTime.Now;
            course.CourseInstructors = courseInstructors;
            Course courseCreated = _repository.Add(course);
            _unitOfWork.Save();
            return courseCreated;
        }

        public async Task<Course> AddWithInstructorsAsync(Course course)
        {
            //If an array is not used, an exception is thrown since the collection gets modified at runtime.
            Guid[] instructorIds = course.CourseInstructors.Select(static ci => ci.Id).ToArray();
            ICollection<CourseInstructor> courseInstructors = new List<CourseInstructor>();
            foreach (Guid instructorId in instructorIds)
            {
                courseInstructors.Add(new CourseInstructor() { InstructorId = instructorId });
            }

            course.CreatedAt = DateTime.Now;
            course.CourseInstructors = courseInstructors;
            Course courseCreated = await _repository.AddAsync(course);
            await _unitOfWork.SaveAsync();
            return courseCreated;
        }

        public Course UpdateWithInstructors(Course course, Guid id)
        {
            Course courseOutOfDate = _repository.Get(id);
            if (courseOutOfDate == null)
                throw new CustomException(NOT_FOUND_ERROR, errorDescriptions[NOT_FOUND_ERROR], Code.Error404);

            //Get the sets to add and delete.
            IEnumerable<Guid> instructorIdsRequest = course.CourseInstructors.Select(static cir => cir.Id);
            //IQueryable<T> is used for performance reasons.
            IQueryable<CourseInstructor> instructors = _courseInstructorRepository.FindBy(ci => ci.CourseId == id);
            IQueryable<CourseInstructor> instructorsToDelete = instructors.Where(ci => !instructorIdsRequest.Contains(ci.InstructorId));
            //If an array is not used, an exception is thrown since the collection gets modified at runtime.
            Guid[] instructorIdsToAdd = instructorIdsRequest.Except(instructors.Select(static i => i.InstructorId)).ToArray();

            _courseInstructorRepository.DeleteRange(instructorsToDelete);
            foreach (Guid instructorId in instructorIdsToAdd)
            {
                _courseInstructorRepository.Add(new CourseInstructor() { CourseId = id, InstructorId = instructorId });
            }

            course.Id = courseOutOfDate.Id;
            course.CreatedAt = courseOutOfDate.CreatedAt;
            course.UpdatedAt = DateTime.Now;
            course.CreatedBy = courseOutOfDate.CreatedBy;
            course.UpdatedBy = courseOutOfDate.UpdatedBy;
            Course courseUpdated = _repository.Update(course);
            _unitOfWork.Save();
            return courseUpdated;
        }
        public async Task<Course> UpdateWithInstructorsAsync(Course course, Guid id)
        {
            Course courseOutOfDate = await _repository.GetAsync(id);
            if (courseOutOfDate == null)
                throw new CustomException(NOT_FOUND_ERROR, errorDescriptions[NOT_FOUND_ERROR], Code.Error404);

            //Get the sets to add and delete.
            IEnumerable<Guid> instructorIdsRequest = course.CourseInstructors.Select(static cir => cir.Id);
            //IQueryable<T> is used for performance reasons.
            IQueryable<CourseInstructor> instructors = _courseInstructorRepository.FindBy(ci => ci.CourseId == id);
            IQueryable<CourseInstructor> instructorsToDelete = instructors.Where(ci => !instructorIdsRequest.Contains(ci.InstructorId));
            //If an array is not used, an exception is thrown since the collection gets modified at runtime.
            Guid[] instructorIdsToAdd = instructorIdsRequest.Except(instructors.Select(static i => i.InstructorId)).ToArray();

            _courseInstructorRepository.DeleteRange(instructorsToDelete);
            foreach(Guid instructorId in instructorIdsToAdd)
            {
                await _courseInstructorRepository.AddAsync(new CourseInstructor() { CourseId = id, InstructorId = instructorId });
            }

            course.Id = courseOutOfDate.Id;
            course.CreatedAt = courseOutOfDate.CreatedAt;
            course.UpdatedAt = DateTime.Now;
            course.CreatedBy = courseOutOfDate.CreatedBy;
            course.UpdatedBy = courseOutOfDate.UpdatedBy;
            Course courseUpdated = await _repository.UpdateAsync(course);
            await _unitOfWork.SaveAsync();
            return courseUpdated;
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
