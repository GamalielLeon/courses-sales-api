using Domain.Validations.RequestValidations;

namespace Domain.DTOs.Request
{
    public class UserRequest
    {
        [RequiredField]
        public string FirsName { get; set; }
        [RequiredField]
        public string LastName { get; set; }
        [RequiredField]
        public string UserName { get; set; }
        [RequiredField]
        public string Email { get; set; }
        [RequiredField]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
