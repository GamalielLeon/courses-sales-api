using System;
using System.Net;

namespace CoursesSaleAPI.Helpers.ErrorHandler
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Description { get; }
        public CustomException(string message, string description, Code code = Code.Error400) : base(message)
        {
            StatusCode = (HttpStatusCode)code;
            Description = description;
        }
    }
}
