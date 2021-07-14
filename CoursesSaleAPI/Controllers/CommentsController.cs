using AutoMapper;
using Domain.Contracts.Service;
using Domain.DTOs.Pagination;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoursesSaleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : GenericController<Comment, CommentsPaged, CommentRequest, CommentResponse>
    {
        public CommentsController(IServiceGeneric<Comment, CommentsPaged> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
