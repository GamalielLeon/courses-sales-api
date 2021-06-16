using Domain.Constants;
using Domain.Contracts.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    class StringLengthFieldAttribute : StringLengthAttribute, IValidation
    {
        public StringLengthFieldAttribute(int maximumLength, int minimumLength = 0) : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessage = ConstantsValidations.Errors[ConstantsValidations.LENGTH_ERROR];
        }
    }
}
