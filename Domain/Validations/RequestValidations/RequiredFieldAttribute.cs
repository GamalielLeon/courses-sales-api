using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    class RequiredFieldAttribute : RequiredAttribute
    {
        public RequiredFieldAttribute()
        {
            AllowEmptyStrings = false;
            ErrorMessage = ConstantsValidations.Errors[ConstantsValidations.REQUIRED_FIELD_ERROR];
        }
    }
}
