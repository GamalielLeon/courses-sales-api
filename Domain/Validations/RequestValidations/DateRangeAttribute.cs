using Domain.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    public class DateRangeAttribute : RangeAttribute
    {
        public const string KEY_ERROR = ConstantsValidations.RANGE_ERROR;
        private const string DATE_FORMAT = "yyyy-MM-dd";

        public DateRangeAttribute(string minimum, string maximum, string fieldName = "")
          : base(typeof(DateTime), DateTime.Parse(minimum).ToString(DATE_FORMAT), DateTime.Parse(maximum).ToString(DATE_FORMAT))
        {
            ErrorMessage = ConstantsValidations.RangeErrors[KEY_ERROR](new string[] { fieldName, minimum, maximum });
        }
    }
}
