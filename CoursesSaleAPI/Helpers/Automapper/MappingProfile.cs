using AutoMapper;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using System.Linq;

namespace CoursesSaleAPI.Helpers.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseRequest, Course>();
            CreateMap<Course, CourseResponse>().ForMember(static x => x.Instructors, static y => y.MapFrom(static z => z.CourseInstructors.Select(static a => a.Instructor)));
            CreateMap<CommentRequest, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<Instructor, CourseInstructorResponse>();
            CreateMap<InstructorRequest, Instructor>();
            CreateMap<InstructorRequest, InstructorResponse>();
            CreateMap<PriceRequest, Price>();
            CreateMap<Price, PriceResponse>();
        }
    }
}
