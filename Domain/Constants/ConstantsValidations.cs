using System;
using System.Collections.Generic;

namespace Domain.Constants
{
    public static class ConstantsValidations
    {
        public const string RANGE_ERROR = "RangeError";
        public const string REQUIRED_FIELD_ERROR = "RequiredError";

        public static Dictionary<string, Func<string[], string>> RangeErrors => new()
        {
            { RANGE_ERROR, static info => $"{FieldName(info[0])}ield value must be between {info[1]} and {info[2]}" },
            { REQUIRED_FIELD_ERROR, static info => $"{FieldName(info[0])}ield must not be null or empty" }
        };

        private static string FieldName(string fieldName) => (String.IsNullOrWhiteSpace(fieldName) ? "F" : fieldName + " F");
    }
}
