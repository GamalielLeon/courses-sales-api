using System.ComponentModel.DataAnnotations;

namespace Domain.Validations.RequestValidations
{
    class RequiredFieldAttribute : RequiredAttribute
    {        
        public RequiredFieldAttribute(string fieldName = "")
        {
            AllowEmptyStrings = false;
            ErrorMessage = $"{(fieldName == "" ? "" : fieldName + " ")}field must not be null or empty";
        }
    }
}
