using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    public class DateRangeAttribute : RangeAttribute
    {
        private const string _dateFormat = "yyyy-MM-dd";

        public DateRangeAttribute(string minimum, string maximum, string fieldName = "")
          : base(typeof(DateTime), DateTime.Parse(minimum).ToString(_dateFormat), DateTime.Parse(maximum).ToString(_dateFormat)) => ErrorMessage = Error["DateError"](minimum, maximum, fieldName);

        private static Dictionary<string, Func<string, string, string, string>> Error => new()
        {
            {"DateError", (minimum, maximum, fieldName) => $"{(fieldName == "" ? "" : fieldName + " ")}field value must be between {minimum} and {maximum}" }
        };

    }
}
