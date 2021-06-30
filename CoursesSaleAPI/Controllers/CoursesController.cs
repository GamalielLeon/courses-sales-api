using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class CoursesController : GenericController<Course, CourseRequest, CourseResponse>
    {
        private readonly IServiceCourse _serviceCourse;
        public CoursesController(IServiceCourse service, IMapper mapper) : base(service, mapper)
        {
            _serviceCourse = (IServiceCourse)_service;
        }

        [HttpGet(GlobalConstants.COURSES_WITH_INSTRUCTORS)]
        public async Task<ActionResult<IEnumerable<CourseView>>> GetAllWithInstructorsAsync()
        {
            ICollection<CourseView> courses = _mapper.Map<ICollection<CourseView>>(await _service.GetAllAsync());
            IQueryable<CourseInstructor> courseInstructors = _serviceCourse.GetAllCourseInstructors();
            foreach (CourseView course in courses)
            {
                await GetInstructors(from ci in courseInstructors where ci.CourseId == course.Id select ci, course);
            }
            return Ok(courses);
        }

        [HttpGet(GlobalConstants.COURSES_WITH_INSTRUCTORS + "/{id}")]
        public async Task<ActionResult<CourseView>> GetWithInstructorsAsync(Guid id)
        {
            CourseView courseResponse = _mapper.Map<CourseView>(await _service.GetAsync(id));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        [HttpPost(GlobalConstants.COURSES_WITH_INSTRUCTORS)]
        public async Task<ActionResult<CourseView>> PostWithInstructorsAsync([FromBody] CourseWithInstructorsRequest courseRequest)
        {
            CourseView courseResponse = _mapper.Map<CourseView>(await _serviceCourse.AddWithInstructorsAsync(_mapper.Map<Course>(courseRequest)));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        [HttpPut(GlobalConstants.COURSES_WITH_INSTRUCTORS + "/{id}")]
        public async Task<ActionResult<CourseView>> PutWithInstructorsAsync([FromBody] CourseWithInstructorsRequest courseRequest, Guid id)
        {
            CourseView courseResponse = _mapper.Map<CourseView>(await _serviceCourse.UpdateWithInstructorsAsync(_mapper.Map<Course>(courseRequest), id));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        private async Task GetInstructors(IQueryable<CourseInstructor> courseInstructors, CourseView courseResponse)
        {
            courseResponse.Instructors = _mapper.Map<ICollection<CourseInstructorResponse>>(await courseInstructors.Select(static ci => ci.Instructor).ToListAsync());
        }
    }
}
