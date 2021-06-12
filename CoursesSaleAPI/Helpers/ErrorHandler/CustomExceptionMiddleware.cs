using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Helpers.ErrorHandler
{
    public class CustomExceptionMiddleware
    {
        private const string _applicationJson = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                HttpRequest request = context.Request;

                if (request.ContentType != _applicationJson) 
                    throw new CustomException(ConstantsErrors.UNSUPPORTED_MEDIA_TYPE, ConstantsErrors.ERROR_DESCRIPTIONS[ConstantsErrors.UNSUPPORTED_MEDIA_TYPE], Code.Error415);

                if(!ConstantsErrors.MethodsAllowed.Contains(request.Method)) 
                    throw new CustomException(ConstantsErrors.METHOD_NOT_IMPLEMENTED, ConstantsErrors.ERROR_DESCRIPTIONS[ConstantsErrors.METHOD_NOT_IMPLEMENTED], Code.Error501);

                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context.Response, exception, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpResponse response, Exception exception, ILogger<CustomExceptionMiddleware> logger)
        {
            Code statusCode = Code.Error500;
            string message = ConstantsErrors.UNEXPECTED_ERROR;
            string description = ConstantsErrors.ERROR_DESCRIPTIONS[message];

            logger.LogError(exception as CustomException is null ? ConstantsErrors.ERROR_HANDLED : ConstantsErrors.SERVER_ERROR);
            CustomErrorResponse errorResponse = exception switch
            {
                CustomException customEx => new CustomErrorResponse()
                    { StatusCode = (int)customEx.StatusCode, Message = customEx.Message, Description = customEx.Description },
                _ => new CustomErrorResponse() { StatusCode = (int)statusCode, Message = message, Description = description }
            };

            response.ContentType = _applicationJson;
            response.StatusCode = errorResponse.StatusCode;
            await response.WriteAsync(errorResponse.ToString());
        }
    }
}
