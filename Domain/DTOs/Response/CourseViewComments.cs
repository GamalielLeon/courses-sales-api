﻿using Domain.Contracts.Entity;
using System;
using System.Collections.Generic;

namespace Domain.DTOs.Response
{
    public class CourseViewComments : ICourseInstructors
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
        public PriceResponse Price { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }
        public ICollection<CourseInstructorResponse> Instructors { get; set; }
    }
}
