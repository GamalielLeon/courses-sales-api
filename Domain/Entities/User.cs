using Domain.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>, IEntity, IRowVersion
    {
        //Base class contains: "public Guid Id { get; set; }"
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
