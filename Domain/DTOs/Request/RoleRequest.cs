using Domain.Validations.RequestValidations;

namespace Domain.DTOs.Request
{
    public class RoleRequest
    {
        [RequiredField]
        [StringLengthField(30, 5)]
        public string Name { get; set; }
        [RequiredField]
        [StringLengthField(10, 2)]
        public string Code { get; set; }
    }
}
