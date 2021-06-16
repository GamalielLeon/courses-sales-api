using Domain.Validations.RequestValidations;
using System;

namespace Domain.DTOs.Request
{
    public class InstructorRequest
    {
        [RequiredField]
        [StringLengthField(50, 3)]
        public string FirstName { get; set; }
        [StringLengthField(50, 3)]
        public string LastName { get; set; }
        [StringLengthField(100, 3)]
        public string Degree { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
