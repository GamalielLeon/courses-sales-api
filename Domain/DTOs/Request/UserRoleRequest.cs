using Domain.Validations.RequestValidations;
using System.Collections.Generic;

namespace Domain.DTOs.Request
{
    public class UserRoleRequest
    {
        [RequiredField]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value.ToUpper(); }
        }
        [RequiredField]
        public ICollection<string> RoleCodes { get; set; }

        private string _userName;
    }
}
