using Domain.Contracts.Entity;
using System;

namespace Domain.Entities
{
    public class Price : IEntity, IRowVersion
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Promotion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual Course Course { get; set; }
    }
}
