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

        [HttpGet("CoursesWithInstructors")]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetAllWithInstructorsAsync()
        {
            var courses = _mapper.Map<ICollection<CourseResponse>>(await _serviceCourse.GetAllWithInstructorsAsync(static c => c.CourseInstructors));
            foreach(CourseResponse course in courses)
            {
                await GetInstructors(from i in _serviceCourse.GetAllCourseInstructors() where i.CourseId == course.Id select i, course);
            }
            return Ok(courses);
        }

        [HttpGet("CoursesWithInstructors/{id}")]
        public async Task<ActionResult<CourseResponse>> GetWithInstructorsAsync(Guid id)
        {
            var courseResponse = _mapper.Map<CourseResponse>(await _serviceCourse.GetWithInstructorsAsync(id, static c => c.CourseInstructors));
            await GetInstructors(_serviceCourse.FindByCourseInstructors(ci => ci.CourseId == courseResponse.Id), courseResponse);
            return Ok(courseResponse);
        }

        private async Task GetInstructors(IQueryable<CourseInstructor> courseInstructors, CourseResponse courseResponse)
        {
            courseResponse.Instructors = _mapper.Map<ICollection<CourseInstructorResponse>>(await courseInstructors.Select(static ci => ci.Instructor).ToListAsync());
        }
    }
}
