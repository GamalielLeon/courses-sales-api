using Domain.Validations.RequestValidations;

namespace Domain.DTOs.Request
{
    public class UserRequest
    {
        [RequiredField]
        [StringLengthField(50, 3)]
        public string FirstName { get; set; }
        [RequiredField]
        [StringLengthField(50, 3)]
        public string LastName { get; set; }
        [RequiredField]
        [StringLengthField(50, 5)]
        public string UserName { get; set; }
        [RequiredField]
        [StringLengthField(50, 10)]
        public string Email { get; set; }
        [RequiredField]
        [StringLengthField(16, 8)]
        public string Password { get; set; }
        [StringLengthField(10, 10)]
        public string PhoneNumber { get; set; }
    }
}
