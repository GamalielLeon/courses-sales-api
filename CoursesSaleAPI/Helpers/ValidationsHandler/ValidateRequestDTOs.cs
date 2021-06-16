using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CoursesSaleAPI.Helpers.ValidationsHandler
{
    public class ValidateRequestDTOs : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ModelState;
            if (model.ErrorCount > 0)
                //Takes only the first validation error as the exception description. Always returns a code 400.
                throw new CustomException(ConstantsErrors.VALIDATION_ERROR, model?.Values.FirstOrDefault().Errors?.FirstOrDefault()?.ErrorMessage);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Do nothing.
        }
    }
}
