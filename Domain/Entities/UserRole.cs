using Domain.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class UserRole : IdentityUserRole<Guid>, IEntity, IRowVersion
    {
        //UserId and RoleId properties are in the IdentityUserRole class.
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
