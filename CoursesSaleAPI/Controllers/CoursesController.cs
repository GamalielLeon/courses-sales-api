using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Entity;
using Domain.Contracts.Service;
using Domain.DTOs.Pagination;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Authorize(Roles = GlobalConstants.ROLES_ALLOWED_FOR_COURSES_CONTROLLER)]
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class CoursesController : PaginationController<Course, CoursesPaged, CourseRequest, CourseResponse>
    {
        private readonly IServiceCourse _serviceCourse;
        public CoursesController(IPaginationService<CoursesPaged> paginationService, IServiceCourse service, IMapper mapper) : base(paginationService, service, mapper)
        {
            _serviceCourse = (IServiceCourse)_service;
        }

        [HttpGet(GlobalConstants.COURSES_WITH_ALL_DETAILS)]
        public async Task<ActionResult<IEnumerable<CourseViewComments>>> GetAllWithInstructorsPriceAndCommentsAsync()
        {
            ICollection<CourseViewComments> courses = _mapper.Map<ICollection<CourseViewComments>>(await _service.GetAllIncludingAsync(static c => c.Price, static c => c.Comments));
            IQueryable<CourseInstructor> courseInstructors = _serviceCourse.GetAllCourseInstructors();
            foreach (CourseViewComments course in courses)
            {
                await GetInstructors(from ci in courseInstructors where ci.CourseId == course.Id select ci, course);
            }
            return Ok(courses);
        }

        [HttpGet(GlobalConstants.COURSES_WITH_ALL_DETAILS + "/{id}")]
        public async Task<ActionResult<CourseViewComments>> GetWithInstructorsPriceAndCommentsAsync(Guid id)
        {
            CourseViewComments courseResponse = _mapper.Map<CourseViewComments>(await _service.GetIncludingAsync(id, static c => c.Price, static c => c.Comments));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        [HttpGet(GlobalConstants.COURSES_WITH_INSTRUCTORS_AND_PRICE)]
        public async Task<ActionResult<IEnumerable<CourseView>>> GetAllWithInstructorsAndPriceAsync()
        {
            ICollection<CourseView> courses = _mapper.Map<ICollection<CourseView>>(await _service.GetAllIncludingAsync(static c => c.Price));
            IQueryable<CourseInstructor> courseInstructors = _serviceCourse.GetAllCourseInstructors();
            foreach (CourseView course in courses)
            {
                await GetInstructors(from ci in courseInstructors where ci.CourseId == course.Id select ci, course);
            }
            return Ok(courses);
        }

        [HttpGet(GlobalConstants.COURSES_WITH_INSTRUCTORS_AND_PRICE + "/{id}")]
        public async Task<ActionResult<CourseView>> GetWithInstructorsAndPriceAsync(Guid id)
        {
            CourseView courseResponse = _mapper.Map<CourseView>(await _service.GetIncludingAsync(id, static c => c.Price));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        [HttpPost(GlobalConstants.COURSES_WITH_INSTRUCTORS_AND_PRICE)]
        public async Task<ActionResult<CourseView>> PostWithInstructorsAndPriceAsync([FromBody] CourseWithInstructorsAndPriceRequest courseRequest)
        {
            CourseView courseResponse = _mapper.Map<CourseView>(await _serviceCourse.AddWithInstructorsAndPriceAsync(_mapper.Map<Course>(courseRequest)));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        [HttpPut(GlobalConstants.COURSES_WITH_INSTRUCTORS_AND_PRICE + "/{id}")]
        public async Task<ActionResult<CourseView>> PutWithInstructorsAsync([FromBody] CourseWithInstructorsAndPriceRequest courseRequest, Guid id)
        {
            CourseView courseResponse = _mapper.Map<CourseView>(await _serviceCourse.UpdateWithInstructorsAndPriceAsync(_mapper.Map<Course>(courseRequest), id));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        private async Task GetInstructors(IQueryable<CourseInstructor> courseInstructors, ICourseInstructors courseResponse)
        {
            courseResponse.Instructors = _mapper.Map<ICollection<CourseInstructorResponse>>(await courseInstructors.Select(static ci => ci.Instructor).ToListAsync());
        }
    }
}
