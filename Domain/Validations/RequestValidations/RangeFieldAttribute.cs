using Domain.Constants;
using Domain.Contracts.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    public class RangeFieldAttribute : RangeAttribute, IValidation
    {
        private static string MessageError = ConstantsValidations.Errors[ConstantsValidations.RANGE_ERROR];
        private const string DATE_FORMAT = GlobalConstants.YYYY_MM_DD;

        public RangeFieldAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
            ErrorMessage = MessageError;
        }

        public RangeFieldAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
            ErrorMessage = MessageError;
        }

        public RangeFieldAttribute(string minimum, string maximum)
          : base(typeof(DateTime), DateTime.Parse(minimum).ToString(DATE_FORMAT), DateTime.Parse(maximum).ToString(DATE_FORMAT))
        {
            ErrorMessage = MessageError;
        }
    }
}
