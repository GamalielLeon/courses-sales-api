using Domain.Validations.RequestValidations;

namespace Domain.DTOs.Request
{
    public class LoginRequest
    {
        [RequiredField]
        public string Email { get; set; }
        [RequiredField]
        public string Password { get; set; }
    }
}
