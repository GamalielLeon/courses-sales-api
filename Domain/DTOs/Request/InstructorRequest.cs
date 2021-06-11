using Domain.Validations.RequestValidations;
using System;

namespace Domain.DTOs.Request
{
    public class InstructorRequest
    {
        [RequiredField(nameof(FirstName))]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
