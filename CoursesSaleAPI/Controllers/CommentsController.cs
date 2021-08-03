using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesSaleAPI.Controllers
{
    [Authorize(Roles = GlobalConstants.ROLES_ALLOWED_FOR_COMMENTS_CONTROLLER)]
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class CommentsController : GenericController<Comment, CommentRequest, CommentResponse>
    {
        public CommentsController(IServiceGeneric<Comment> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
