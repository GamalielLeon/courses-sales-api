using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGenericRepository<Price> _priceRepository;
        public ServiceCourse(IGenericRepository<CourseInstructor> courseInstructorRepository, IGenericRepository<Price> priceRepository, IGenericRepository<Course> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _courseInstructorRepository = courseInstructorRepository;
            _priceRepository = priceRepository;
        }

        public async Task<Course> AddWithInstructorsAndPriceAsync(Course course)
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

            try
            {
                Course courseCreated = await _repository.AddAsync(course);
                await _unitOfWork.SaveAsync();
                return courseCreated;
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, nameof(Course));
            }
        }

        public async Task<Course> UpdateWithInstructorsAndPriceAsync(Course course, Guid id)
        {
            Course courseOutOfDate = await _repository.GetIncludingAsync(id, static c => c.Price);
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
            course.Price = await _priceRepository.UpdateAsync(SetDefaultPropertiesOfPrice(course.Price, courseOutOfDate.Price));
            
            try
            {
                Course courseUpdated = await _repository.UpdateAsync(course);
                await _unitOfWork.SaveAsync();
                return courseUpdated;
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, nameof(Course));
            }
        }

        public IQueryable<CourseInstructor> GetAllCourseInstructors()
        {
            return _courseInstructorRepository.GetAllIncluding(static ci => ci.Instructor);
        }

        public IQueryable<CourseInstructor> FindByCourseInstructors(Expression<Func<CourseInstructor, bool>> predicate)
        {
            return _courseInstructorRepository.FindByIncluding(predicate, static ci => ci.Instructor);
        }

        private static Price SetDefaultPropertiesOfPrice(Price currentPrice, Price oldPrice)
        {
            currentPrice.Id = oldPrice.Id;
            currentPrice.CourseId = oldPrice.CourseId;
            currentPrice.CreatedAt = oldPrice.CreatedAt;
            currentPrice.UpdatedAt = DateTime.Now;
            currentPrice.CreatedBy = oldPrice.CreatedBy;
            currentPrice.UpdatedBy = oldPrice.UpdatedBy;
            return currentPrice;
        }
    }
}
