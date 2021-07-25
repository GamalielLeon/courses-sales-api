using System.Collections.Generic;

namespace Domain.Constants
{
    public static class ConstantsErrors
    {
        public static ICollection<string> MethodsAllowed => new string[] { "GET", "POST", "PUT", "DELETE" };
        public static ICollection<string> MethodsWithJSONContentType => new string[] { "POST", "PUT" };

        public const string ERROR_HANDLED = "Error handled";
        public const string SERVER_ERROR = "Server error";

        public const string BAD_REQUEST = "BadRequest";
        public const string DUPLICATED_EMAIL = "EmailAlreadyExists";
        public const string DUPLICATED_USERNAME = "UsernameAlreadyExists";
        public const string DUPLICATED_CODE = "CodeAlreadyExists";
        public const string DUPLICATED_NAME = "NameAlreadyExists";
        public const string VALIDATION_ERROR = "ValidationError";
        public const string EXCEEDED_RECORDS = "ExceededRecords";
        public const string PROPERTY_ERROR = "PropertyError";
        public const string UNAUTHORIZED = "Unauthorized";
        public const string NOT_FOUND = "NotFound";
        public const string UNSUPPORTED_MEDIA_TYPE = "UnsupportedMediaType"; 
        public const string METHOD_NOT_IMPLEMENTED = "MethodNotImplemented";
        public const string UNEXPECTED_ERROR = "UnexpectedError";

        public static Dictionary<string, string> ERROR_DESCRIPTIONS => new()
        {
            { BAD_REQUEST, "400 Bad Request" },
            { DUPLICATED_EMAIL, "This email is already registered"},
            { DUPLICATED_USERNAME, "This username is already registered"},
            { DUPLICATED_CODE, "This code is already registered" },
            { DUPLICATED_NAME, "This name is already registered" },
            { VALIDATION_ERROR, "One or more field validations failed"},
            { EXCEEDED_RECORDS, "Number of requested records exceeds database records" },
            { PROPERTY_ERROR, "Property was not found"},
            { UNAUTHORIZED, "Invalid credentials"},
            { NOT_FOUND, "Entity or resource does not exist" },
            { UNSUPPORTED_MEDIA_TYPE , "Unsupported Media Type" },
            { METHOD_NOT_IMPLEMENTED, "Method is not implemented or is not recognized by the server" },
            { UNEXPECTED_ERROR, "Unexpected Error" }
        };
    }
}
;