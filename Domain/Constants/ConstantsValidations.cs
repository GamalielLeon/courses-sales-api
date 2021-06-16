using System.Collections.Generic;

namespace Domain.Constants
{
    public static class ConstantsValidations
    {
        public const string RANGE_ERROR = "RangeError";
        public const string REQUIRED_FIELD_ERROR = "RequiredError";
        public const string LENGTH_ERROR = "LengthError";

        public static Dictionary<string, string> Errors => new()
        {
            { RANGE_ERROR, "{0} value must be between {1} and {2}" },
            { REQUIRED_FIELD_ERROR, "{0} field must not be null or empty" },
            { LENGTH_ERROR, "{0} value must have {2} or more characters and cannot exceed {1} characters" }
        };
    }
}
