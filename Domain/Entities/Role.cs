using Domain.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class Role : IdentityRole<Guid>, IEntity, IRowVersion
    {
        //Base class contains: "public Guid Id { get; set; }"
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
