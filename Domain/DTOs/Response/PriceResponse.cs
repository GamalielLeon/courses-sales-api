using System;

namespace Domain.DTOs.Response
{
    public class PriceResponse
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Promotion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
