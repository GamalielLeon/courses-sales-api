using Domain.Contracts.Entity;
using Domain.DTOs.Response;
using System;
using System.Collections.Generic;

namespace Domain.DTOs.Pagination
{
    public class CoursesPaged : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishingDate { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Promotion { get; set; }
        public string InstructorsIds { get; set; }
    }
}
