using System.Net;
namespace CoursesSaleAPI.Helpers.ErrorHandler
{
    public enum Code
    {
        Ok200 = HttpStatusCode.OK,
        Ok201 = HttpStatusCode.Created,
        Ok204 = HttpStatusCode.NoContent,
        Error400 = HttpStatusCode.BadRequest,
        Error401 = HttpStatusCode.Unauthorized,
        Error404 = HttpStatusCode.NotFound,
        Error405 = HttpStatusCode.MethodNotAllowed,
        Error415 = HttpStatusCode.UnsupportedMediaType,
        Error500 = HttpStatusCode.InternalServerError,
        Error501 = HttpStatusCode.NotImplemented,
    }
}