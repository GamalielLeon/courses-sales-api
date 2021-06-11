using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    class RequiredFieldAttribute : RequiredAttribute
    {
        public const string KEY_ERROR = ConstantsValidations.REQUIRED_FIELD_ERROR;
        public RequiredFieldAttribute(string fieldName = "")
        {
            AllowEmptyStrings = false;
            ErrorMessage = ConstantsValidations.RangeErrors[KEY_ERROR](new string[] { fieldName });
        }
    }
}
