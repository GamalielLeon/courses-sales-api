using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    public class NumberRangeAttribute : RangeAttribute
    {
        public NumberRangeAttribute(int minimum, int maximum, string fieldName = "") : base(minimum, maximum) => ErrorMessage = "";

        public NumberRangeAttribute(double minimum, double maximum, string fieldName = "") : base(minimum, maximum) => ErrorMessage = "";
    }
}
