using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    public class NumberRangeAttribute : RangeAttribute
    {
        public const string KEY_ERROR = ConstantsValidations.RANGE_ERROR;
        public NumberRangeAttribute(int minimum, int maximum, string fieldName = "") : base(minimum, maximum)
        {
            ErrorMessage = ConstantsValidations.RangeErrors[KEY_ERROR](new string[] { fieldName, minimum.ToString(), maximum.ToString()});
        }

        public NumberRangeAttribute(double minimum, double maximum, string fieldName = "") : base(minimum, maximum)
        {
            ErrorMessage = ConstantsValidations.RangeErrors[KEY_ERROR](new string[] { fieldName, minimum.ToString(), maximum.ToString() });
        }
    }
}
